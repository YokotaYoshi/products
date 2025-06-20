using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    //--------------------------プレイヤー関係------------------------------------------------
    GameObject player;
    PlayerController playerCnt;
    Vector2 playerPosition;
    float playerDistance;

    //---------------------------自分関係------------------------------------------------------
    Rigidbody2D rb2d;
    bool isAttacking = false;
    public float speed = 5f;//通常の移動速度
    public float dashAttackSpeed = 8f;//ダッシュ攻撃の速度
    public GameObject attackArea;
    float attackCoolTime = 3f;
    float attackReadyTime = 3f;


    //プレイヤーの位置に関するフラグ
    bool playerNear;
    bool playerFar;
    bool playerRight;
    bool playerLeft;
    bool playerUp;
    bool playerDown;

    //球を飛ばす
    public GameObject bullet;


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
        //プレイヤーとの距離を取得
        playerDistance = playerPosition.magnitude;

        attackReadyTime += Time.deltaTime;

        if (!isAttacking)
        Debug.Log(isAttacking);

        //プレイヤーの位置に応じてフラグを立てる
        if (playerDistance <= 2f) playerNear = true;
        else playerNear = false;
        if (playerDistance >= 8f) playerFar = true;
        else playerFar = false;
        if (playerPosition.y >= -1f && playerPosition.y <= 1f)
        {
            if (playerPosition.x > 1f) playerRight = true;
            else playerRight = false;
            if (playerPosition.x < -1f) playerLeft = true;
            else playerLeft = false;
        }
        else if (playerPosition.x >= -1f && playerPosition.x <= 1f)
        {
            if (playerPosition.y > 1f) playerUp = true;
            else playerUp = false;
            if (playerPosition.y < -1f) playerDown = true;
            else playerDown = false;
        }
    }

    void FixedUpdate()
    {
        if (isAttacking == false)
        {
            if (playerNear)
            {
                //プレイヤーと接近したら
                
                StartCoroutine(AttackSwing());
            }
            else if (playerFar)
            {
                //プレイヤーから離れたら
                //レーザー打つか、ミサイル飛ばすかする
                //Instantiate(bullet, transform.position + new Vector3(2f, 0f, 0f), Quaternion.identity);
            }
            else
            {
                if (playerRight) StartCoroutine(AttackDash(1f, 0f));
                else if (playerLeft) StartCoroutine(AttackDash(-1f, 0f));
                else if (playerUp) StartCoroutine(AttackDash(0f, 1f));
                else if (playerDown) StartCoroutine(AttackDash(0f, -1f));
                else StartCoroutine(Move(1f, 1f));
            }

            //障害となるブロックを破壊しながら進みたい
            
        }
        
    }

    //通常の移動
    IEnumerator Move(float x, float y)
    {
        Debug.Log("1");
        isAttacking = true;
        float time = 0f;
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        while(true)
        {
            time += Time.deltaTime;
            rb2d.linearVelocity = new Vector2(x * speed, y * speed);
            yield return null;
            if (time >= 0.2f)  break;
        }
        isAttacking = true;
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }

    //プレイヤーに接近すると攻撃
    IEnumerator AttackSwing()
    {
        isAttacking = true;
        attackReadyTime = 2f;
        //攻撃判定を出してプレイヤーを攻撃する
        Instantiate(attackArea, transform.position, Quaternion.identity);

        while (attackReadyTime <= attackCoolTime)
        {
            yield return null;
        }

        isAttacking = false;
    }

    //プレイヤーが飛車の移動範囲にいるとき、突撃攻撃
    //突撃開始時のプレイヤーの位置より少し行き過ぎる。
    IEnumerator AttackDash(float x, float y)
    {
        //最初に格子点に移動
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        //Vector2 attackDirection = new Vector2(x, y);

        isAttacking = true;
        float attackTime = 0;
        float moveDistance = playerDistance;

        while(true)
        {
            attackTime += Time.deltaTime;

            rb2d.linearVelocity = new Vector2(x * dashAttackSpeed, y * dashAttackSpeed);
            

            if (attackTime >= moveDistance / dashAttackSpeed + 0.3f)
            {
                //ちょっと通り過ぎて
                break;
            }

            yield return null;
        }

        //速度をゼロに
        rb2d.linearVelocity = Vector2.zero;
        //攻撃終わり
        isAttacking = false;
        attackTime = 0f;
        //最後に格子点に移動
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }
}
