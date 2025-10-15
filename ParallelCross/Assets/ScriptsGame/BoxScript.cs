using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxScript : MonoBehaviour
{
    //倉庫番の箱のように、押すことができる
    public int boxID = 1;
    GameObject player;
    PlayerController playerCnt;
    float speed;//箱を押すスピード
    Vector2 playerPosition;
    Rigidbody2D rb2d;
    float axisH;
    float axisV;
    float preAxisH;
    float preAxisV;
    Vector2 inputVector;//入力方向
    Vector2 checkVector;

    public bool isMoving = false;//移動中かどうか
    bool isCoroutineWorking;
    //IEnumerator moveCoroutine;//コルーチン
    Vector2 nearestGrid;

    Direction moveDirection = Direction.N;
    //---------------------上下左右にモノがあるか-----------------
    public bool isCollisionUp = false;
    public bool isCollisionDown = false;
    public bool isCollisionRight = false;
    public bool isCollisionLeft = false;

    public GameObject boxBroken;//壊れた箱

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCnt = player.GetComponent<PlayerController>();
        }
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //こっちから見たプレイヤーの位置
        playerPosition = new Vector2(player.transform.position.x - transform.position.x,
        player.transform.position.y - transform.position.y);

        nearestGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        //ベクトル(axisH, axisV)は(0,0),(+-1,0),(0,+-1)のいずれかである
        if (axisV == 0)
        {
            axisH = Input.GetAxisRaw("Horizontal");
            if (isCollisionRight && axisH >= 0.5f) axisH = 0f;
            if (isCollisionLeft && axisH <= -0.5f) axisH = 0f;
        }
        if (axisH == 0)
        {
            axisV = Input.GetAxisRaw("Vertical");
            if (isCollisionUp && axisV >= 0.5f) axisV = 0f;
            if (isCollisionDown && axisV <= -0.5f) axisV = 0f;
        }


        //入力が1,-1から変更されたらコルーチン開始フラグオン
        if (preAxisH != 0 && preAxisH != axisH)
        {
            if (isCoroutineWorking)
            {
                //すでにコルーチンが働いていたら、入力の保存だけして抜ける
                preAxisH = axisH;
                preAxisV = axisV;
                return;
            }
            else if (preAxisH > 0.5f)
            {
                moveDirection = Direction.Right;
            }
            else if (preAxisH < -0.5f)
            {
                moveDirection = Direction.Left;
            }
            preAxisH = axisH;
            preAxisV = axisV;
        }
        else if (preAxisV != 0 && preAxisV != axisV)
        {
            //Debug.Log(preAxisV);
            if (isCoroutineWorking)
            {
                //すでにコルーチンが働いていたら、入力の保存だけして抜ける
                preAxisH = axisH;
                preAxisV = axisV;
                return;
            }
            else if (preAxisV > 0.5f)
            {
                moveDirection = Direction.Up;
            }
            else if (preAxisV < -0.5f)
            {
                moveDirection = Direction.Down;
            }
            preAxisH = axisH;
            preAxisV = axisV;
        }
        else
        {
            //入力を一時保存する
            preAxisH = axisH;
            preAxisV = axisV;
        }

        inputVector = new Vector2(axisH, axisV);//入力ベクトル

        //プレイヤーの位置に入力ベクトルを足す。これがほぼ0なら箱を押していることになる

        checkVector = new Vector2(playerPosition.x + inputVector.x, playerPosition.y + inputVector.y);

        if (checkVector.magnitude <= 0.2f)
        {
            speed = PlayerController.speed;
            isMoving = true;
        }

        if (!isMoving)
        {
            transform.position = nearestGrid;
            //Debug.Log("位置リセット");
        }
    }

    void FixedUpdate()
    {
        if (moveDirection != Direction.N)
        {
            //Debug.Log("コルーチン開始");
            StartCoroutine(Move(moveDirection));
        }
        if (!isCoroutineWorking)
        {
            //コルーチン未始動なら速度更新
            rb2d.linearVelocity = new Vector2(speed * axisH, speed * axisV);
        }
    }

    //--------------上下左右の入力終了後の自動運転--------------------
    private IEnumerator Move(Direction direction)
    {
        isCoroutineWorking = true;//コルーチン始動フラグ

        //すでに移動開始したので、コルーチンが重複しないよう方向フラグoff
        moveDirection = Direction.N;

        float distance = 1.0f;//プレイヤーの現在位置から格子点までの距離
        float isGoal = 0.1f;//ゴールまでの距離がこれ以下だったらゴールとする

        while (true)
        {
            //ゴールまでの距離が一定以下なら以下の処理を毎フレーム行う
            //ゴールに近づいたらループを抜ける
            if (isGoal > distance)
            {
                break;
            }

            //distanceの値がおかしくなった時用
            //場所がほぼ格子点上ならそこで止める
            if (new Vector2(transform.position.x - Mathf.Round(transform.position.x),
            transform.position.y - Mathf.Round(transform.position.y)).magnitude < isGoal)
            {
                break;
            }

            switch (direction)
            {
                case Direction.Right:
                    distance = Mathf.Ceil(transform.position.x) - transform.position.x;
                    rb2d.linearVelocity = new Vector2(speed, 0f);
                    break;
                case Direction.Left:
                    distance = transform.position.x - Mathf.Floor(transform.position.x);
                    rb2d.linearVelocity = new Vector2(-speed, 0f);
                    break;
                case Direction.Up:
                    distance = Mathf.Ceil(transform.position.y) - transform.position.y;
                    rb2d.linearVelocity = new Vector2(0f, speed);
                    break;
                case Direction.Down:
                    distance = transform.position.y - Mathf.Floor(transform.position.y);
                    rb2d.linearVelocity = new Vector2(0f, -speed);
                    break;
            }

            isGoal = 0.01f * speed;

            yield return null;
        }

        //格子点についたら座標を整数値にし、速度を0にし、動いていない状態にする
        transform.position = nearestGrid;
        speed = 0.0f;
        isMoving = false;
        isCoroutineWorking = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //ロードする場所をふさぐことがないように
        if (other.gameObject.tag == "LoadPoint")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Enemy")
        {
            //敵が接触すると踏みつぶされる
            Instantiate(boxBroken, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Carry")
        {
            transform.position = nearestGrid;
            rb2d.linearVelocity = Vector2.zero;
        }
    }
}
