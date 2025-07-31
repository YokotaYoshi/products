using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    //--------------------------プレイヤー関係------------------------------------------------
    GameObject player;
    PlayerController playerCnt;
    Vector2 playerPosition;
    Vector2 playerDirection;
    float playerDistance;

    //---------------------------自分関係------------------------------------------------------
    Rigidbody2D rb2d;
    bool isAttacking = false;
    bool isMoving = false;
    bool willMove = false;
    public float speed = 5f;//通常の移動速度
    public float dashAttackSpeed = 8f;//ダッシュ攻撃の速度
    public GameObject attackArea;


    //プレイヤーの位置に関するフラグ
    bool playerNear;
    bool playerFar;
    bool playerRight;
    bool playerLeft;
    bool playerUp;
    bool playerDown;

    //球を飛ばす
    public GameObject bullet;

    //今の状態について
    enum BossState { Idle, Move, AttackSwing, AttackShoot, AttackDash}
    BossState currentState = BossState.Idle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーを取得
        playerCnt = player.GetComponent<PlayerController>();//プレイヤーコントローラー取得
        rb2d = GetComponent<Rigidbody2D>();//Rigidbody2Dを取得
    }

    // Update is called once per frame
    void Update()
    {
        //自分からみたプレイヤーの位置を取得
        playerPosition = new Vector2(player.transform.position.x - transform.position.x,
        player.transform.position.y - transform.position.y);
        playerDirection = playerPosition.normalized;
        //プレイヤーとの距離を取得
        playerDistance = playerPosition.magnitude;

        //Debug.Log(playerDirection);


        //プレイヤーの位置に応じてフラグを立てる
        if (playerDistance <= 2f) playerNear = true;
        else playerNear = false;
        if (playerDistance >= 5f) playerFar = true;
        else playerFar = false;

        if (playerPosition.y >= -1f && playerPosition.y <= 1f)
        {
            if (playerPosition.x > 1f) playerRight = true;
            else playerRight = false;
            if (playerPosition.x < -1f) playerLeft = true;
            else playerLeft = false;

            playerUp = false;
            playerDown = false;
        }

        if (playerPosition.x >= -1f && playerPosition.x <= 1f)
        {
            if (playerPosition.y > 1f) playerUp = true;
            else playerUp = false;
            if (playerPosition.y < -1f) playerDown = true;
            else playerDown = false;

            playerRight = false;
            playerLeft = false;
        }
        
        //状態遷移を判定
        switch (currentState)
        {
            case BossState.Idle:
                if (playerNear) currentState = BossState.AttackSwing;
                else if (playerFar) currentState = willMove ? BossState.Move : BossState.AttackShoot;
                break;
        }
    }

    void FixedUpdate()
    {
        if (isAttacking == false)
        {
            if (playerNear)
            {
                //プレイヤーと接近したら
                //Debug.Log("スイング");
                StopAllCoroutines();
                StartCoroutine(AttackSwing());
            }
            else if (playerFar)
            {
                //プレイヤーから離れたら
                if (willMove && !isMoving)
                {
                    //動く番で、まだ動いていなかったら
                    StopAllCoroutines();
                    StartCoroutine(Move());
                    
                    //Debug.Log("移動");
                }
                else
                {
                    //弾を撃つ
                    //Debug.Log("ショット");
                    StopAllCoroutines();
                    StartCoroutine(AttackShoot());
                }
            }
            else
            {
                StopAllCoroutines();
                if (playerRight) StartCoroutine(AttackDash(1f, 0f));
                else if (playerLeft) StartCoroutine(AttackDash(-1f, 0f));
                else if (playerUp) StartCoroutine(AttackDash(0f, 1f));
                else if (playerDown) StartCoroutine(AttackDash(0f, -1f));
                else
                {
                    //Debug.Log("移動");
                    StartCoroutine(Move());
                }
                    
            }
            //障害となるブロックを破壊しながら進みたい
        }
        
    }

    //通常の移動
    IEnumerator Move()
    {
        Vector2 direction = playerDirection;

        //direction.x ** 2 + direction.y ** 2 = 1;

        isAttacking = true;
        isMoving = true;
        float time = 0f;
        //transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        Debug.Log(direction);

        while (true)
        {
            time += Time.deltaTime;

            //playerDirectionに応じて縦横に移動
            if (direction.y >= direction.x)
            {
                if (direction.y >= -direction.x)
                {
                    //プレイヤーが上にいるとき
                    rb2d.linearVelocity = new Vector2(0f, speed);
                }
                else
                {
                    //左
                    rb2d.linearVelocity = new Vector2(-speed, 0f);
                }
            }
            else
            {
                if (direction.y >= -direction.x)
                {
                    //プレイヤーが右にいるとき
                    rb2d.linearVelocity = new Vector2(speed, 0f);
                }
                else
                {
                    //下
                    rb2d.linearVelocity = new Vector2(0f, -speed);
                }
            }
            
            yield return null;
            if (time >= 0.6f) break;
        }


        //Debug.Log("移動終わり");
        //速度をゼロに
        rb2d.linearVelocity = Vector2.zero;
        if (playerFar)
        {
            willMove = false;//次は発砲
        }
        isMoving = false;
        transform.position = new Vector2(Mathf.Round(transform.position.x),
        Mathf.Round(transform.position.y));
        isAttacking = false;
    }

    IEnumerator AttackShoot()
    {
        isAttacking = true;
        Instantiate(bullet, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        willMove = true;//次は移動
        isAttacking = false;
    }

    //プレイヤーに接近すると攻撃
    IEnumerator AttackSwing()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.2f);

        //攻撃判定を出してプレイヤーを攻撃する
        Instantiate(attackArea, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.2f);

        isAttacking = false;
        //Debug.Log("攻撃終わり");
    }



    //プレイヤーが飛車の移動範囲にいるとき、突撃攻撃
    //突撃開始時のプレイヤーの位置より少し行き過ぎる。
    IEnumerator AttackDash(float x, float y)
    {
        //Debug.Log("とつげき");

        //最初に格子点に移動
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        //Vector2 attackDirection = new Vector2(x, y);

        isAttacking = true;
        float attackTime = 0;
        float moveDistance = playerDistance;


        while (true)
        {
            attackTime += Time.deltaTime;

            rb2d.linearVelocity = new Vector2(x * dashAttackSpeed, y * dashAttackSpeed);


            if (attackTime >= moveDistance / dashAttackSpeed + 0.1f)
            {
                //ちょっと通り過ぎて
                break;
            }

            yield return null;
        }

        //速度をゼロに
        rb2d.linearVelocity = Vector2.zero;

        attackTime = 0f;
        //Debug.Log("攻撃終わり");
        //最後に格子点に移動
        transform.position = new Vector2(Mathf.Round(transform.position.x),
        Mathf.Round(transform.position.y));
        
        //攻撃終わり
        isAttacking = false;
    }
}
