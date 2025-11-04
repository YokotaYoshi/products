using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Chase,
    Move,
    Stay,
}
public class EnemyChaseController : MonoBehaviour
{
    //追いかけ状態でロードしたら出現しないようにしたい
    public bool isFirstAppearance = false;//最初に登場する＝シーン切り替え時に登場するやつじゃない
    //public int eventProgressStartChasing;
    public bool chaseWhenSeePlayer;
    float chaseTime;//見失ってもしばらくは追いかける
    //EnemyGuardianController enemyGCnt;
    
    //PlayerController playerCnt;//プレイヤーコントローラー
    //public float baseSpeed = 8.0f;//基準となる追跡速度
    public float speedVeryHard;
    public float speedHard;
    public float speedNormal;
    public float speedEasy;
    float speed;//追跡速度
    Rigidbody2D rb2d;//Rigidbody2D;
    CircleCollider2D enemyCollider;//CircleCollider2D;

    

    //動く方向について
    GameObject player;//プレイヤー
    Vector2 playerDirection;//自分から見たプレイヤーの位置
    public float playerDirectionDegree;//自分から見たプレイヤーの角度
    Vector2 playerGrid;
    GameObject[] searchPoints;
    public Vector2 moveGrid;
    public Vector2 moveGridDirection;
    public Vector2 targetGrid;
    public Direction moveDirection = Direction.N;//外部からいじる
    public bool isCoroutineWorking;
    //Vector2 targetDirection;
    float gap;
    float distance = 1f;

    //-------------何かに衝突した時に使う--------------
    public bool isBlocked = false;//壁衝突フラグ。プレイヤーの方向にいけるかどうか

    public float down = 0.0f;//ブロックを避けるための下方向移動量
    public float right = 0.0f;//ブロックを避けるための右方向移動量
    public float up = 0.0f;//ブロックを避けるための上方向移動量
    public float left = 0.0f;//ブロックを避けるための左方向移動量

    public GameObject damageArea;//攻撃判定
    public float stanTimeBase = 1f;//攻撃されたときにひるむ時間
    float stanTime;
    GridMove gridMove;
    Vector2 nearestGrid;
    //レイキャスト
    RaycastHit2D hit;
    RaycastHit2D hitSP;
    
    public EnemyState enemyState = EnemyState.Chase;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーを取得
        searchPoints = GameObject.FindGameObjectsWithTag("SearchPoint");
        //playerCnt = player.GetComponent<PlayerController>();//プレイヤーコントローラーを取得
        rb2d = GetComponent<Rigidbody2D>();//Rigidbody2Dを取得
        enemyCollider = GetComponent<CircleCollider2D>();//CircleCollider2Dを取得

        if (GameManager.gameState == GameState.Run && isFirstAppearance) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(enemyState);
        //難易度におうじて速度変更
        switch (Data.difficulty)
        {
            case (Difficulty.VeryHard):
                speed = speedVeryHard;
                stanTime = stanTimeBase - 0.1f;
                break;
            case (Difficulty.Hard):
                speed = speedHard;//8
                stanTime = stanTimeBase;
                break;
            case (Difficulty.Normal):
                speed = speedNormal;//6
                stanTime = stanTimeBase;
                break;
            case (Difficulty.Easy):
                speed = Mathf.Min(speedEasy, (float)Data.playerLevel);
                
                stanTime = stanTimeBase + 0.2f;
                break;
        }



        nearestGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        playerGrid = new Vector2(Mathf.Round(player.transform.position.x), Mathf.Round(player.transform.position.y));
        playerDirection = new Vector2(player.transform.position.x - transform.position.x,
        player.transform.position.y - transform.position.y);

        float distance = Vector2.Distance(transform.position, player.transform.position);

        Ray ray = new Ray(transform.position, playerDirection);

        

        /*
        Debug.DrawRay(ray.origin, ray.direction * playerDirection.magnitude, Color.red, 0.5f);
        int excludedLayer = LayerMask.NameToLayer("Enemy");
        int excludedMask = 1 << excludedLayer;
        int invertedMask = ~excludedMask;//~:ビット反転で除外レイヤー以外を対象にする
        */
        int Mask = LayerMask.GetMask("Default");

        hit = Physics2D.Raycast(transform.position, playerDirection, distance, Mask);

        if (enemyState == EnemyState.Chase)
        {
            SetMoveGrid();
            SetMoveDirection();
        }
        else if (enemyState == EnemyState.Move)
        {
            speed = 4f;

            moveDirection = (Direction)Random.Range(0, 4);
            
        }

        //gameState切り替え
        if (GameManager.gameState == GameState.Pause)
        {
            speed = 0.0f;
        }
        else if (GameManager.gameState != GameState.GameOver)
        {
            GameManager.gameState = GameState.Run;
        }

        //物陰に隠れたプレイヤーを探してから追跡する

        

        if (chaseWhenSeePlayer)
        {
            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.gameObject.name.Substring(0, 6));
                
                if (hit.collider.gameObject.tag == "Player")
                {
                    chaseTime = 0f;
                    enemyState = EnemyState.Chase;//プレイヤーを見つけた時だけ追いかける
                }
                
            }
            chaseTime += Time.deltaTime;
            if (chaseTime >= 3f)
            {
                enemyState = EnemyState.Move;//見失ったら3秒であきらめる
            }
        }
    }

    void FixedUpdate()
    {
        if (PlayerController.hp <= 0)
        {
            rb2d.linearVelocity = Vector2.zero;
        }

        if (enemyState != EnemyState.Stay)
        {
            StartCoroutine(Move(moveDirection, distance));
        }
    }

    void SetMoveGrid()
    {
        //見える場所にプレイヤーがいるならプレイヤーに
        //いない場合は見える場所にいるSearchPlayerのなかでpointが最も小さいもの
        int minPoint = 99;
        int searchPoint;
        int searchPointNum = 0;
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                moveGrid = playerGrid;
            }
            else
            {
                //searchPoints[]
                if (searchPoints.Length == 0)
                {
                    moveGrid = playerGrid;
                }
                else
                {
                    for (int i = 0; i < searchPoints.Length; ++i)
                    {
                        Vector2 searchPointDirection = new Vector2(searchPoints[i].transform.position.x - transform.position.x, searchPoints[i].transform.position.y - transform.position.y);//ここから見た捜索点の座標

                        
                        //int excludedLayer = LayerMask.NameToLayer("IgnoreRayCast");//レイヤーのインデックス取得
                        //int excludedMask = 1 << excludedLayer;//左に1ビットシフト。2倍にしている。なぜ
                        
                        int excludedMask = LayerMask.GetMask("Ignore Raycast");
                        
                        int invertedMask = ~excludedMask;//~:ビット反転で除外レイヤー以外を対象にする
                        
                        hitSP = Physics2D.Raycast(transform.position, searchPointDirection, searchPointDirection.magnitude, invertedMask);
                        

                        if (hitSP.collider != null)
                        {
                            Debug.Log(hitSP.collider.gameObject.name);
                            
                           
                            if (hitSP.collider.gameObject.tag == "SearchPoint")//見える位置の捜索点についてのみ
                            {

                                searchPoint = searchPoints[i].GetComponent<SearchPlayer>().point;
                                minPoint = Mathf.Min(minPoint, searchPoint);
                                Debug.Log(i);
                                //Debug.Log(searchPoint);
                                if (minPoint == searchPoint)
                                {
                                    //iをほぞん
                                    searchPointNum = i;
                                    
                                }
                            }
                        }
                        
                        
                    }
                    moveGrid = new Vector2(searchPoints[searchPointNum].transform.position.x, searchPoints[searchPointNum].transform.position.y);
                }
            }
        }
    }

    void SetMoveDirection()
    {
        //どの方向にいくか
        //自分から見たプレイヤーの位置
        //playerDirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        moveGridDirection = new Vector2(moveGrid.x - transform.position.x, moveGrid.y - transform.position.y);


        //自分から見たプレイヤーの角度
        playerDirectionDegree = Mathf.Atan2(moveGridDirection.y, moveGridDirection.x) * Mathf.Rad2Deg;
        //Debug.Log(playerDirectionDegree);

        //目の前を防がれていない場合
        if (!isBlocked)
        {
            //実際に動く方向を決定
            distance = 1f;//とりあえず1マス動く
            if (playerDirectionDegree >= -50 && playerDirectionDegree < 50)
            {
                //プレイヤーが右のほうにいる
                moveDirection = Direction.Right;
            }
            else if (playerDirectionDegree >= 50 && playerDirectionDegree < 130)
            {
                //プレイヤーが上のほうにいる
                moveDirection = Direction.Up;
            }
            else if (playerDirectionDegree >= -130 && playerDirectionDegree < -50)
            {
                //プレイヤーが下のほうにいる
                moveDirection = Direction.Down;
            }
            else
            {
                //プレイヤーが左のほうにいる
                moveDirection = Direction.Left;
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
                moveDirection = Direction.Right;
                distance = right;
            }
            else if ((playerDirectionDegree >= 130 && playerDirectionDegree <= 180) ||
            (playerDirectionDegree >= 0 && playerDirectionDegree < 50))
            {
                //プレイヤーが上のほうにいる
                //上に移動
                //Debug.Log("上");

                moveDirection = Direction.Up;
                distance = up;
            }
            else if ((playerDirectionDegree >= -50 && playerDirectionDegree < 0) ||
            (playerDirectionDegree >= -180 && playerDirectionDegree < -130))
            {
                //プレイヤーが下のほうにいる
                //下に移動

                moveDirection = Direction.Down;
                distance = down;
            }
            else
            {
                //プレイヤーが左のほうにいる

                moveDirection = Direction.Left;
                distance = left;
            }

        }
            

        if (PlayerController.hp <= 0)
        {
            //プレイヤーが死んだら動かない
            moveDirection = Direction.N;
            distance = 0f;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "PlayerCollider")
        {
            //Debug.Log("プレイヤー");
            //プレイヤーに衝突したら
            if (other.gameObject.tag == "PlayerCollider")
            {
                StopCoroutine(Move(moveDirection, distance));

                StartCoroutine(HitPlayer(1f));//再度追いかける。当たり判定を復活する
            }
        }
        if (other.gameObject.tag == "PlayerAttack")
        {
            //Debug.Log("被弾");
            //攻撃に衝突したら
            StopCoroutine(Move(moveDirection, distance));
            StartCoroutine(HitPlayer(stanTime));//再度追いかける。当たり判定を復活する
        }
    }

    public IEnumerator Move(Direction moveDirection, float distance)
    {
        if (isCoroutineWorking) yield break;
        //目標となる格子点まで移動する
        float time = 0f;
        //動いているフラグ立て
        isCoroutineWorking = true;
        float isGoal = 0.1f;//ゴールまでの距離がこれ以下だったらゴールとする

        switch (moveDirection)
        {
            case Direction.Right:
                targetGrid = nearestGrid + new Vector2(distance, 0f);
                rb2d.linearVelocity = new Vector2(speed, 0f);
                break;
            case Direction.Left:
                targetGrid = nearestGrid + new Vector2(-distance, 0f);
                rb2d.linearVelocity = new Vector2(-speed, 0f);
                break;
            case Direction.Up:
                targetGrid = nearestGrid + new Vector2(0f, distance);
                rb2d.linearVelocity = new Vector2(0f, speed);
                break;
            case Direction.Down:
                targetGrid = nearestGrid + new Vector2(0f, -distance);
                rb2d.linearVelocity = new Vector2(0f, -speed);
                break;
        }

        while (true)
        {
            //ゴールまでの距離を更新
            gap = new Vector2(targetGrid.x - transform.position.x, targetGrid.y - transform.position.y).magnitude;

            isGoal = 0.01f * speed;

            time += Time.deltaTime;

            //ゴールに十分近づいたらおわり
            if (gap < isGoal)
            {
                transform.position = nearestGrid;
                moveDirection = Direction.N;
                break;
            }

            //そのほか時間経過でも終わり

            if (time >= 1f / speed)
            {
                transform.position = nearestGrid;
                rb2d.linearVelocity = Vector2.zero;
                moveDirection = Direction.N;
                break;
            }
            yield return null;
        }
        //動いているフラグおろし
        isCoroutineWorking = false;
    }
    public IEnumerator HitPlayer(float wait)
    {
        //waitが全体時間
        //近くの格子点に移動し停止
        //当たり判定を削除
        if (enemyState == EnemyState.Stay) yield break;
        enemyState = EnemyState.Stay;

        if (damageArea != null)
        {
            damageArea.GetComponent<CircleCollider2D>().enabled = false;//攻撃判定停止
        }
        
        //GetComponent<CircleCollider2D>().enabled = false;//攻撃判定停止

        Vector2 targetGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        float point;

        //少し時間をかけて近くの格子点に移動する
        //動いていた方向と反対方向に少しのけぞる
        switch (moveDirection)
        {
            case Direction.Right:
                targetGrid = new Vector2(Mathf.Round(transform.position.x - 0.4f), Mathf.Round(transform.position.y));
                break;
            case Direction.Left:
                targetGrid = new Vector2(Mathf.Round(transform.position.x + 0.4f), Mathf.Round(transform.position.y));
                break;
            case Direction.Up:
                targetGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y - 0.4f));
                break;
            case Direction.Down:
                targetGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y + 0.4f));
                break;
        }
        Vector2 hitPosition = transform.position;

        float time = 0;//吹っ飛ばされてからの時間
        float waitingTime = 0.3f;//吹っ飛ばされる時間

        rb2d.linearVelocity = Vector2.zero;
        while (time < waitingTime)
        {
            if (PlayerController.hp <= 0)
            {
                //ゲームオーバーなら停止
                rb2d.linearVelocity = Vector2.zero;
                yield break;
            }

            //Vector2.Lerpで位置調整
            point = -10f * (time - waitingTime) * (time - waitingTime) + 1f;//二次関数

            transform.position = Vector2.Lerp(hitPosition, targetGrid, point);

            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetGrid;
        rb2d.linearVelocity = Vector2.zero;//停止

        //数フレーム待機した後、当たり判定を復活させ追跡を再開する。
        while (time < wait)
        {
            time += Time.deltaTime;
            yield return null;
        }


        enemyState = EnemyState.Chase;
        if (damageArea != null)
        {
            damageArea.GetComponent<CircleCollider2D>().enabled = true;//攻撃判定復活
        }
        
        //GetComponent<CircleCollider2D>().enabled = true;
        enemyCollider.enabled = true;//当たり判定復活
    }
}
