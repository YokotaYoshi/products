using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxScript : MonoBehaviour
{
    //倉庫番の箱のように、押すことができる
    GameObject player;
    PlayerController playerCnt;
    float speed;//箱を押すスピード
    Vector2 playerPosition;
    Rigidbody2D rb2d;
    float axisH;
    float axisV;
    Vector2 inputVector;//入力方向
    Vector2 checkVector;
    float preAxisH = 0.0f;//axisHの一時保存
    float preAxisV = 0.0f;//axisVの一時保存

    public bool isMoving = false;//移動中かどうか
    public bool isCoroutineWorking = false;//コルーチン中かどうか
    bool isRight = false;//右方向コルーチン開始フラグ
    bool isLeft = false;//左方向コルーチン開始フラグ
    bool isUp = false;//上方向コルーチン開始フラグ
    bool isDown = false;//下方向コルーチン開始フラグ

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

        

        //ベクトル(axisH, axisV)は(0,0),(+-1,0),(0,+-1)のいずれかである
        if (axisV == 0)
        {
            axisH = Input.GetAxisRaw("Horizontal");
        }
        if (axisH == 0)
        {
            axisV = Input.GetAxisRaw("Vertical");
        }

        inputVector = new Vector2(axisH, axisV);//入力ベクトル

        //プレイヤーの位置に入力ベクトルを足す。これがほぼ0なら箱を押していることになる

        checkVector = new Vector2(playerPosition.x + inputVector.x, playerPosition.y + inputVector.y);

        if (checkVector.magnitude <= 0.1f || isCoroutineWorking)
        {
            speed = playerCnt.speed;
        }
        else
        {
            speed = 0f;
        }
        //Debug.Log(inputVector);

        

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
            else if(preAxisH > 0.5f)
            {
                isRight = true;
            }
            else if(preAxisH < -0.5f)
            {
                isLeft = true;
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
            else if(preAxisV > 0.5f)
            {
                isUp = true;
            }
            else if(preAxisV < -0.5f)
            {
                isDown = true;
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
    }

    void FixedUpdate()
    {
        if (isRight)
        {
            //右移動フラグオンなら
            StartCoroutine(Move(1.0f, 0.0f));
        }
        else if (isLeft)
        {
            //左移動フラグオンなら
            StartCoroutine(Move(-1.0f, 0.0f));
        }
        else if (isUp)
        {
            //上移動フラグオンなら
            StartCoroutine(Move(0.0f, 1.0f));
        }
        else if (isDown)
        {
            //下移動フラグオンなら
            StartCoroutine(Move(0.0f, -1.0f));
        }

        if (!isMoving)
        {
            //動いていない状態なら
            //速度を更新
            rb2d.linearVelocity = new Vector2(speed*axisH, speed*axisV);

            if (axisH != 0 || axisV != 0)
            {
                //上下左右いずれかの入力があるなら、少なくとも動いている状態である
                isMoving = true;
            }
            else
            {
                transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
            }
        }
        else if(!isCoroutineWorking)
        {
            //動いている状態かつコルーチン未始動なら
            //速さのみ更新→ダッシュに対応
            rb2d.linearVelocity = new Vector2(speed*axisH, speed*axisV);
        }
    }
    
    //----------------------上下左右の入力終了後の自動運転-------------------------
    private IEnumerator Move(float x, float y)
    {
        isCoroutineWorking = true;//コルーチン始動フラグ

        //すでに移動開始したので、コルーチンが重複しないよう方向フラグoff
        isRight = false;
        isLeft = false;
        isUp = false;
        isDown = false;

        float distance = 1.0f;//プレイヤーの現在位置から格子点までの距離
        float isGoal = 0.1f;//ゴールまでの距離がこれ以下だったらゴールとする


        while(true)
        {
            //ゴールまでの距離が一定以下なら以下の処理を毎フレーム行う
            //格子点までの距離を計測
            if (x > 0.5f)
            {
                //右方向
                distance = Mathf.Ceil(transform.position.x)-transform.position.x;
                
            }
            else if (x < -0.5f)
            {
                //左方向
                distance = transform.position.x - Mathf.Floor(transform.position.x);
            }
            else if (y > 0.5f)
            {
                //上方向
                distance = Mathf.Ceil(transform.position.y)-transform.position.y;
            }
            else if (y < -0.5f)
            {
                //下方向
                distance = transform.position.y - Mathf.Floor(transform.position.y);
            }

            //ゴールに近づいたらループを抜ける
            if (isGoal > distance)
            {
                break;
            }
            //distanceの値がおかしくなった時用
            //場所がほぼ格子点上ならそこで止める
            if (new Vector2(transform.position.x - Mathf.Round(transform.position.x), 
            transform.position.y - Mathf.Round(transform.position.y)).magnitude < 0.1f)
            {
                break;
            }

            //速度を更新
            rb2d.linearVelocity = new Vector2(speed * x, speed * y);
            yield return null;
        }

        //格子点についたら座標を整数値にし、速度を0にし、動いていない状態にする
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        rb2d.linearVelocity = Vector2.zero;
        isMoving = false;
        isCoroutineWorking = false;
    }
}
