using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    //雑魚敵
    //バットで一撃で？倒せる
    //通常の移動のみ
    //移動せず、射撃してくる
    //移動して、切りかかってくる
    //の三種類くらい用意すれば十分か
    //知能は低め＝壁に向かっていってもおけ
    //
    public float speed;
    public float attackRange = 1.5f;//プレイヤーとの距離がこれ以下なら攻撃
    public float interval = 1f;//移動をこの秒数おきに行う
    bool isCoroutineWorking = false;
    bool isFreezing = false;
    public bool isCollisionUp;
    public bool isCollisionDown;
    public bool isCollisionRight;
    public bool isCollisionLeft;
    float time;
    public int hp = 1;
    int random;
    Direction direction;
    Rigidbody2D rb2d;
    Vector2 nearestGrid;
    public GameObject effect;
    public GameObject attack;//攻撃。斬撃だったり射撃だったり
    GameObject player;
    Vector2 playerDirection;
    Animator animator;
    string animCurrent;
    string animSet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //rb2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        //必要な情報を取得するターン
        nearestGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        if (player != null)
        {
            //プレイヤーの位置を把握
            playerDirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        }
        MoveDirection();
        

        //いろいろ処理するターン
        
        if (hp <= 0)
        {
            //やられ処理。軽めに爆発四散
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (isFreezing) return;

        if (!isCoroutineWorking)
        {
            StartCoroutine(Move(direction));
            if (playerDirection.magnitude < attackRange)
            {
                //ちかづいたら攻撃
                StartCoroutine(Attack());
            }
            
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttack" && !isFreezing)
        {
            
            StartCoroutine(Attacked(playerDirection.normalized));
        }
    }

    public void MoveDirection()
    {
        //プレイヤーに近づく2方向をランダムで選択
        random = Random.Range(0, 2);
        if (playerDirection.x >= 0f && playerDirection.y >= 0f)
        {
            if (isCollisionRight && isCollisionUp)
            {
                if (random == 0)
                {
                    direction = Direction.Left;
                }
                else
                {
                    direction = Direction.Down;
                }
            }
            else if (isCollisionRight)
            {
                direction = Direction.Up;
            }
            else if (isCollisionUp)
            {
                direction = Direction.Right;
            }
            else
            {
                if (random == 0)
                {
                    direction = Direction.Right;
                }
                else
                {
                    direction = Direction.Up;
                }
            }
        }
        else if (playerDirection.x <= 0f && playerDirection.y >= 0f)
        {
            if (isCollisionLeft && isCollisionUp)
            {
                if (random == 0)
                {
                    direction = Direction.Down;
                }
                else
                {
                    direction = Direction.Right;
                }
            }
            else if (isCollisionLeft)
            {
                direction = Direction.Up;
            }
            else if (isCollisionUp)
            {
                direction = Direction.Left;
            }
            else
            {
                if (random == 0)
                {
                    direction = Direction.Left;
                }
                else
                {
                    direction = Direction.Up;
                }
            }
        }
        else if (playerDirection.x <= 0f && playerDirection.y <= 0f)
        {
            if (isCollisionLeft && isCollisionDown)
            {
                if (random == 0)
                {
                    direction = Direction.Right;
                }
                else
                {
                    direction = Direction.Up;
                }
            }
            else if (isCollisionLeft)
            {
                direction = Direction.Down;
            }
            else if (isCollisionDown)
            {
                direction = Direction.Left;
            }
            else
            {
                if (random == 0)
                {
                    direction = Direction.Left;
                }
                else
                {
                    direction = Direction.Down;
                }
            }
        }
        else
        {
            if (isCollisionRight && isCollisionDown)
            {
                if (random == 0)
                {
                    direction = Direction.Left;
                }
                else
                {
                    direction = Direction.Up;
                }
            }
            else if (isCollisionRight)
            {
                direction = Direction.Down;
            }
            else if (isCollisionDown)
            {
                direction = Direction.Right;
            }
            else
            {
                if (random == 0)
                {
                    direction = Direction.Right;
                }
                else
                {
                    direction = Direction.Down;
                }
            }
        }
    }

    

    public IEnumerator Move(Direction direction)
    {
        if (isCoroutineWorking) yield break;
        else isCoroutineWorking = true;
        float time = 0f;
        //基本の1マス移動
        switch (direction)
        {//アニメーション
            case Direction.Up:
                animator.Play("EnemyWalkUp");//アニメーション
                break;
            case Direction.Down:
                animator.Play("EnemyWalkDown");
                break;
            case Direction.Right:
                animator.Play("EnemyWalkRight");
                break;
            case Direction.Left:
                animator.Play("EnemyWalkLeft");
                break;
            default:
                break;
        }
        transform.position = nearestGrid;
        rb2d.linearVelocity = Vector2.zero;

        //移動前隙
        yield return new WaitForSeconds(interval);

        while (true)
        {
            time += Time.deltaTime;
            yield return null;
            switch (direction)
            {
                case Direction.Up:
                    rb2d.linearVelocity = new Vector2(0f, speed);
                    break;
                case Direction.Down:
                    rb2d.linearVelocity = new Vector2(0f, -speed);
                    break;
                case Direction.Right:
                    rb2d.linearVelocity = new Vector2(speed, 0f);
                    break;
                case Direction.Left:
                    rb2d.linearVelocity = new Vector2(-speed, 0f);
                    break;
                default:
                    break;
            }
            //Debug.Log(time);
            //Debug.Log(speed);
            if (time >= 1f / speed)
            {
                break;
            }
        }
        isCoroutineWorking = false;
        transform.position = nearestGrid;
        rb2d.linearVelocity = Vector2.zero;
    }

    public virtual IEnumerator Attack()//親クラスではvirtualをつける
    {
        if (attack == null)
        {
            yield return null;
            isCoroutineWorking = false;
            yield break;
        }
        isCoroutineWorking = true;
        //前隙
        yield return new WaitForSeconds(0.5f);
        //攻撃生成
        Instantiate(attack, transform.position, Quaternion.identity);
        //後隙
        yield return new WaitForSeconds(0.5f);
        isCoroutineWorking = false;
    }

    public virtual IEnumerator Attacked(Vector2 direction)
    {
        //ヒットストップいれたい
        isCoroutineWorking = true;
        isFreezing = true;

        float hitStopTime = 0.1f;
        
        Time.timeScale = 0f;
        while (true)
        {
            hitStopTime -= Time.unscaledDeltaTime;
            yield return null;
            if (hitStopTime < 0f)
            {
                Time.timeScale = 1f;
                break;
            }
        }
        
        hp -= 1;
        
        float time = 0f;
        float waitingTime = 0.3f;

        //ゴールとなる格子点を計算

        Vector2 hitPosition = transform.position;
        float point;

        //上下左右に物体があるかどうかでゴールを変更
        //playerCntのとdirectionが逆向きなので注意
        if (isCollisionUp && direction.y < 0f) direction.y = 0;
        if (isCollisionDown && direction.y > 0f) direction.y = 0;
        if (isCollisionRight && direction.x < 0f) direction.x = 0;
        if (isCollisionLeft && direction.x > 0f) direction.x = 0;

        Vector2 targetGrid = new Vector2(Mathf.Round(transform.position.x - direction.x), Mathf.Round(transform.position.y - direction.y));
        while (true)
        {
            point = -10f * (time - waitingTime) * (time - waitingTime) + 1f;//二次関数的に減速

            transform.position = Vector2.Lerp(hitPosition, targetGrid, point);
            
            time += Time.deltaTime;
            if (time > waitingTime)
            {
                time = 0f;
                break;
            }
            yield return null;
        }

        transform.position = targetGrid;
        rb2d.linearVelocity = Vector2.zero;
        isCoroutineWorking = false;
        isFreezing = false;//また動くように
    }
}
