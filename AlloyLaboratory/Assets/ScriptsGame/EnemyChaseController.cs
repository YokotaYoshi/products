using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseController : MonoBehaviour
{
    //追いかけ状態でロードしたら出現しないようにしたい
    public bool isFirstAppearance = false;//最初に登場する＝シーン切り替え時に登場するやつじゃない
    public int eventProgressStartChasing;
    public bool chaseWhenSeePlayer;
    EnemyGuardianController enemyGCnt;
    GameObject player;//プレイヤー
    //PlayerController playerCnt;//プレイヤーコントローラー
    public float baseSpeed;//基準となる追跡速度
    float speed;//追跡速度
    Rigidbody2D rb2d;//Rigidbody2D;
    CircleCollider2D enemyCollider;//CircleCollider2D;
    
    Vector2 playerDirection;//自分から見たプレイヤーの位置
    public float playerDirectionDegree;//自分から見たプレイヤーの角度
   
    
    Direction moveDirectionEnum;
    float distance = 1f;

    //-------------何かに衝突した時に使う--------------
    public bool isBlocked = false;//壁衝突フラグ。プレイヤーの方向にいけるかどうか
    
    public float down = 0.0f;//ブロックを避けるための下方向移動量
    public float right = 0.0f;//ブロックを避けるための右方向移動量
    public float up = 0.0f;//ブロックを避けるための上方向移動量
    public float left = 0.0f;//ブロックを避けるための左方向移動量


    GridMove gridMove;
    Vector2 nearestGrid;
    bool isWaiting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーを取得
        //playerCnt = player.GetComponent<PlayerController>();//プレイヤーコントローラーを取得
        rb2d = GetComponent<Rigidbody2D>();//Rigidbody2Dを取得
        enemyCollider = GetComponent<CircleCollider2D>();//CircleCollider2Dを取得

        if (GameManager.gameState == GameState.Run && isFirstAppearance) Destroy(gameObject);

        if (chaseWhenSeePlayer)
        {
            enemyGCnt = gameObject.GetComponent<EnemyGuardianController>();
        }


        gridMove = this.GetComponent<GridMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //難易度におうじて速度変更
        switch (Data.difficulty)
        {
            case (Difficulty.VeryHard):
                speed = baseSpeed + 3.0f;
                break;
            case (Difficulty.Hard):
                speed = baseSpeed + 1.0f;
                break;
            case (Difficulty.Normal):
                speed = baseSpeed;
                break;
            case (Difficulty.Easy):
                speed = baseSpeed - 2.0f;
                break;
        }

        SetMoveDirection();
        nearestGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        //gameState切り替え
        if (GameManager.gameState == GameState.Pause)
        {
            gridMove.speed = 0.0f;
        }
        else if (GameManager.gameState != GameState.GameOver)
        {
            gridMove.speed = speed;
            GameManager.gameState = GameState.Run;
        }

        if (!isWaiting)
        {
            StartCoroutine(gridMove.Move(moveDirectionEnum, distance));
        }
        
    }

    void FixedUpdate()
    {
        if (PlayerController.hp <= 0)
        {
            rb2d.linearVelocity = Vector2.zero;
        }
    }

    void SetMoveDirection()
    {
        //どの方向にいくか
        //自分から見たプレイヤーの位置
        playerDirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

        //自分から見たプレイヤーの角度
        playerDirectionDegree = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        //Debug.Log(playerDirectionDegree);

        //目の前を防がれていない場合
        if (!isBlocked)
        {
            //実際に動く方向を決定
            distance = 1f;//とりあえず1マス動く
            if (playerDirectionDegree >= -50 && playerDirectionDegree < 50)
            {
                //プレイヤーが右のほうにいる
                moveDirectionEnum = Direction.Right;
            }
            else if (playerDirectionDegree >= 50 && playerDirectionDegree < 130)
            {
                //プレイヤーが上のほうにいる
                moveDirectionEnum = Direction.Up;
            }
            else if (playerDirectionDegree >= -130 && playerDirectionDegree < -50)
            {
                //プレイヤーが下のほうにいる
                moveDirectionEnum = Direction.Down;
            }
            else
            {
                //プレイヤーが左のほうにいる
                moveDirectionEnum = Direction.Left;
            }
        }
        else
        {
            //Debug.Log("障害物を避ける");
            //2番目にプレイヤーとの距離を縮められる方向に動く
            if ((playerDirectionDegree >= -90 && playerDirectionDegree < -50) ||
            (playerDirectionDegree >= 50 && playerDirectionDegree < 90))
            {
                //プレイヤーが右のほうにいる
                //右に移動
                moveDirectionEnum = Direction.Right;
                distance = right;
            }
            else if ((playerDirectionDegree >= 130 && playerDirectionDegree <= 180) ||
            (playerDirectionDegree >= 0 && playerDirectionDegree < 50))
            {
                //プレイヤーが上のほうにいる
                //上に移動
                //Debug.Log("上");

                moveDirectionEnum = Direction.Up;
                distance = up;
            }
            else if ((playerDirectionDegree >= -50 && playerDirectionDegree < 0) ||
            (playerDirectionDegree >= -180 && playerDirectionDegree < -130))
            {
                //プレイヤーが下のほうにいる
                //下に移動

                moveDirectionEnum = Direction.Down;
                distance = down;
            }
            else
            {
                //プレイヤーが左のほうにいる

                moveDirectionEnum = Direction.Left;
                distance = left;
            }
        }
        
        if (PlayerController.hp <= 0)
        {
            //プレイヤーが死んだら動かない
            moveDirectionEnum = Direction.N;
            distance = 0f;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerCollider")
        {
            Debug.Log("衝突");
            //プレイヤーに衝突したら
            StopCoroutine(gridMove.Move(moveDirectionEnum, distance));
            
            StartCoroutine(HitPlayer());//再度追いかける。当たり判定を復活する
        }
    }
    IEnumerator HitPlayer()
    {
        //近くの格子点に移動し停止
        //当たり判定を削除
        if (isWaiting) yield break;
        isWaiting = true;

        Vector2 targetGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        float point;

        //少し時間をかけて近くの格子点に移動する
        //動いていた方向と反対方向に少しのけぞる
        switch (moveDirectionEnum)
        {
            case Direction.Right:
                targetGrid = new Vector2(Mathf.Round(transform.position.x - 0.2f), Mathf.Round(transform.position.y));
                break;
            case Direction.Left:
                targetGrid = new Vector2(Mathf.Round(transform.position.x + 0.2f), Mathf.Round(transform.position.y));
                break;
            case Direction.Up:
                targetGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y - 0.2f));
                break;
            case Direction.Down:
                targetGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y + 0.2f));
                break;
        }
        

        Vector2 hitPosition = transform.position;

        float time = 0;//吹っ飛ばされてからの時間
        float waitingTime = 0.3f;//吹っ飛ばされる時間

        rb2d.linearVelocity = Vector2.zero;

        Debug.Log(targetGrid);

        while (time < waitingTime)
        {
            if (PlayerController.hp <= 0)
            {
                //ゲームオーバーなら停止
                rb2d.linearVelocity = Vector2.zero;
                yield break;
            }

            //Vector2.Lerpで位置調整
            point = -10f * (time - waitingTime) * (time - waitingTime) + 1f;

            transform.position = Vector2.Lerp(hitPosition, targetGrid, point);

            time += Time.deltaTime;
            yield return null;
        }
        //Debug.Log(time);
        rb2d.linearVelocity = Vector2.zero;//停止
        
        //数フレーム待機した後、当たり判定を復活させ追跡を再開する。
        yield return new WaitForSeconds(1f);

        isWaiting = false;
        enemyCollider.enabled = true;//当たり判定復活
    }
}
