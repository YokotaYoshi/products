using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseController : MonoBehaviour
{
    public bool chaseWhenSeePlayer;
    EnemyGuardianController enemyGCnt;
    GameObject player;//プレイヤー
    //PlayerController playerCnt;//プレイヤーコントローラー
    public float speed = 3.0f;//追跡速度
    bool isMoving = false;//動いているかどうか
    Rigidbody2D rb2d;//Rigidbody2D;
    CircleCollider2D enemyCollider;//CircleCollider2D;
    
    float moveTime;//移動にかかる時間
    Vector2 playerDirection;//自分から見たプレイヤーの位置
    public float playerDirectionDegree;//自分から見たプレイヤーの角度
    Vector2 moveDirection;//実際に動く方向

    //-------------何かに衝突した時に使う--------------
    public float waitTime = 0.5f;//一時停止時間
    bool breakCoroutine = false;//コルーチン脱出フラグ
    public bool isBlocked = false;//壁衝突フラグ。プレイヤーの方向にいけるかどうか
    
    public float down = 0.0f;//ブロックを避けるための下方向移動量
    public float right = 0.0f;//ブロックを避けるための右方向移動量
    public float up = 0.0f;//ブロックを避けるための上方向移動量
    public float left = 0.0f;//ブロックを避けるための左方向移動量
    float dx = 0.0f;//ブロックを避けるために実際に横方向に移動する量
    float dy = 0.0f;//ブロックを避けるために実際に縦方向に移動する量

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーを取得
        //playerCnt = player.GetComponent<PlayerController>();//プレイヤーコントローラーを取得
        rb2d = GetComponent<Rigidbody2D>();//Rigidbody2Dを取得
        enemyCollider = GetComponent<CircleCollider2D>();//CircleCollider2Dを取得

        if (chaseWhenSeePlayer)
        {
            enemyGCnt = gameObject.GetComponent<EnemyGuardianController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.hp <= 0)
        {
            breakCoroutine = true;
        }
        
        //自分から見たプレイヤーの位置
        playerDirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

        //自分から見たプレイヤーの角度
        playerDirectionDegree = Mathf.Atan2(playerDirection.y, playerDirection.x)*Mathf.Rad2Deg;
        //Debug.Log(playerDirectionDegree);

        //実際に動く方向を決定
        if (playerDirectionDegree >= -50 && playerDirectionDegree < 50)
        {
            //プレイヤーが右のほうにいる
            //右に移動
            moveDirection = new Vector2(1.0f, 0f);
        }
        else if (playerDirectionDegree >= 50 && playerDirectionDegree < 130)
        {
            //プレイヤーが上のほうにいる
            //上に移動
            moveDirection = new Vector2(0f, 1.0f);
        }
        else if (playerDirectionDegree >= -130 && playerDirectionDegree < -50)
        {
            //プレイヤーが下のほうにいる
            //下に移動
            moveDirection = new Vector2(0f, -1.0f);
        }
        else
        {
            //プレイヤーが左のほうにいる
            //左に移動
            moveDirection = new Vector2(-1.0f, 0f);
        }
        //Debug.Log(isBlocked);
        //Debug.Log(moveDirection);
        //Debug.Log(breakCoroutine);
        //Debug.Log(playerDirectionDegree);
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            if (chaseWhenSeePlayer)
            {
                //プレイヤーが視界に入った時だけ追いかける
                if (breakCoroutine)
                {
                    return;
                }
                if (!isMoving&&enemyGCnt.isChasing)
                {
                    //コルーチンが終了したら、コルーチンをスタートさせる
                    if (!isBlocked)
                    {
                        //プレイヤーの方向に障害物がないならプレイヤーの方向に
                        StartCoroutine(Move(moveDirection));
                        //Debug.Log(moveDirection);
                    }
                    else
                    {
                        //プレイヤーの方向に障害物があるならよけるように
                        AvoidBlock(down, right, up, left);
                    }
                }
            }
            else
            {
                //プレイヤーを常に追いかける
                if (breakCoroutine)
                {
                    return;
                }
                if (!isMoving)
                {
                    //コルーチンが終了したら、コルーチンをスタートさせる
                    if (!isBlocked)
                    {
                        //プレイヤーの方向に障害物がないならプレイヤーの方向に
                        StartCoroutine(Move(moveDirection));
                        //Debug.Log(moveDirection);
                    }
                    else
                    {
                        //プレイヤーの方向に障害物があるならよけるように
                        AvoidBlock(down, right, up, left);
                    }
                }
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーに衝突したら
            breakCoroutine = true;
            rb2d.linearVelocity = Vector2.zero;//停止
            enemyCollider.enabled = false;//当たり判定を消去
            StartCoroutine(Restart());//再度追いかける。当たり判定を復活する
        }
    }


    IEnumerator Move(Vector2 direction)
    {
        //格子点にいたる毎にプレイヤーの位置を参照し、今の格子点の隣であり
        //かつプレイヤーに最も近くの格子点をゴールとして移動する

        //動いているフラグ立て
        isMoving = true;

        //時間計測
        float time = 0.0f;

        //1マスの移動にかかる時間を計算
        moveTime = direction.magnitude / speed;

        //移動方向に合わせて速度ベクトルを設定
        if (direction.x > 0.5f)
        {
            rb2d.linearVelocity = new Vector2(speed, 0.0f);
        }
        else if (direction.x < -0.5f)
        {
            rb2d.linearVelocity = new Vector2(-speed, 0.0f);
        }
        else if (direction.y > 0.5f)
        {
            rb2d.linearVelocity = new Vector2(0.0f, speed);
        }
        else if (direction.y < -0.5f)
        {
            rb2d.linearVelocity = new Vector2(0.0f, -speed);
        }

        
        
        while(true)
        {
            time += Time.deltaTime;//時間計測

            if (time >= moveTime)
            {
                break;//時間的に移動し終わったら解除
            }
            if (breakCoroutine)
            {
                //Debug.Log("停止");
                yield break;//コルーチン停止命令が出たら停止
            }

            yield return null;
        }

        //Debug.Log(time);
        //動いているフラグおろし
        isMoving = false;
        //速度をゼロに
        rb2d.linearVelocity = Vector2.zero;
        //座標を格子点に
        transform.position = new Vector2(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y));
    }

    

    void AvoidBlock(float down, float right, float up, float left)
    {
        //Debug.Log("ブロック回避");

        //自分から見たプレイヤーの位置
        Vector2 playerDirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

        //自分から見たプレイヤーの角度
        float playerDirectionDegree = Mathf.Atan2(playerDirection.y, playerDirection.x)*Mathf.Rad2Deg;

        //2番目にプレイヤーとの距離を縮められる方向に動く
        if ((playerDirectionDegree >= -90 && playerDirectionDegree < -50)||
        (playerDirectionDegree >= 50 && playerDirectionDegree < 90))
        {
            //プレイヤーが右のほうにいる
            //右に移動
            //Debug.Log("右");
            dx = right;
            dy = 0;
        }
        else if ((playerDirectionDegree >= 130 && playerDirectionDegree <= 180)||
        (playerDirectionDegree >= 0 && playerDirectionDegree < 50))
        {
            //プレイヤーが上のほうにいる
            //上に移動
            //Debug.Log("上");
            //Debug.Log(playerDirectionDegree);
            dx = 0;
            dy = up;
        }
        else if ((playerDirectionDegree >= -50 && playerDirectionDegree < 0)||
        (playerDirectionDegree >= -180 && playerDirectionDegree < -130))
        {
            //プレイヤーが下のほうにいる
            //下に移動
            //Debug.Log("下");
            dx = 0;
            dy = -down;
        }
        else
        {
            //プレイヤーが左のほうにいる
            //左に移動
            //Debug.Log("左");
            dx = -left;
            dy = 0;
        }
        //Debug.Log(new Vector2(dx, dy));
        StartCoroutine(Move(new Vector2(dx, dy)));
    }

    IEnumerator Restart()
    {
        isMoving = true;//動いていることにしてMove()の起動阻止

        //少し時間をかけて近くの格子点に移動する
        float time = 0;//吹っ飛ばされてからの時間
        float blownTime = 0.5f;//吹っ飛ばされる時間
        float blownSpeed;//吹っ飛ばされる速さ

        //敵の方向と反対方向に0.4吹き飛ばす
        //吹っ飛ばす方向の正規ベクトル
        Vector2 blownDirection = new Vector2((transform.position.x - player.transform.position.x),(transform.position.y - player.transform.position.y)).normalized;
        //吹っ飛ばされる先はもっとも近い格子点
        Vector2 blownGoal = new Vector2(Mathf.Round(transform.position.x + blownDirection.x * 0.5f), Mathf.Round(transform.position.y + blownDirection.y * 0.5f));
        //吹っ飛ばされる強さ
        //吹っ飛ばされる先までの距離に比例
        float blownForce = new Vector2(blownGoal.x - transform.position.x, blownGoal.y - transform.position.y).magnitude * 30f;


        //Debug.Log(blownForce);

        while (time < blownTime)
        {
            if (PlayerController.hp <= 0)
            {
                //ゲームオーバーなら停止
                rb2d.linearVelocity = Vector2.zero;
                yield break;
            }
            blownSpeed = (blownTime - time)*blownForce;
            rb2d.linearVelocity = new Vector2((blownGoal.x - transform.position.x) * blownSpeed, (blownGoal.y - transform.position.y) * blownSpeed);
            time += Time.deltaTime;
            yield return null;
        }
        //Debug.Log(time);
        
        //数フレーム待機した後、当たり判定を復活させ追跡を再開する。
        yield return new WaitForSeconds(waitTime);

        enemyCollider.enabled = true;//当たり判定復活
        breakCoroutine = false;//コルーチン脱出フラグoff
        isMoving = false;//動いていない状態＝これから動ける状態にする
    }
}
