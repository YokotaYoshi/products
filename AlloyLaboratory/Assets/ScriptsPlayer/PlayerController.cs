using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    //操作キャラクターの動きに関する部分を主に担当する
    

    //----------------通常の動き関係----------------
    private Rigidbody2D rb2d;
    public float walkSpeed = 5.0f;//歩きスピード
    public float dashSpeed = 10.0f;//ダッシュスピード
    public static float speed;//移動速度
    float axisH = 0.0f;//左右入力離散値
    float axisV = 0.0f;//上下入力離散値
    Vector2 inputVector;//入力方向
    float preAxisH = 0.0f;//axisHの一時保存
    float preAxisV = 0.0f;//axisVの一時保存

    public bool isMoving = false;//移動中かどうか
    public bool isCoroutineWorking = false;//コルーチン中かどうか
    public Direction moveDirection = Direction.N;

    //--------------------ロード後の座標関係--------------------
    public static Direction startPos = Direction.N;//ロード先の座標
    public float startPosX = 0f;//手動で設定する場合のX座標
    public float startPosY = 0f;//手動で設定する場合のY座標
    

    //-------------------------HP関連------------------------
    public int maxHp = 3;//最大hp
    public static int hp = 3;//hp
    public bool isInvincible = false;//くらい無敵
    //GameObject enemy;

    //------------------------カメラ関係-----------------------
    GameObject mainCamera;
    CameraController cameraCnt;

    //---------------------上下左右にモノがあるか-----------------
    public bool isCollisionUp = false;
    public bool isCollisionDown = false;
    public bool isCollisionRight = false;
    public bool isCollisionLeft = false;
    //--------------------その他----------------------------------
    public bool isAttacked = false;//攻撃された！
    public Vector2 blownDirection;//吹っ飛ばされる方向
    Vector2 nearestGrid;


    void Awake()
    {
        if (hp <= 1) hp = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();//Rigidbody2Dの取得
        //enemy = GameObject.FindGameObjectWithTag("Damage1");
        
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraCnt = mainCamera.GetComponent<CameraController>();

        

        switch (startPos)
        {
            case Direction.Right:
                transform.position = new Vector2(Data.loadPosX + 1.0f, Data.loadPosY);
                break;
            case Direction.Left:
                transform.position = new Vector2(Data.loadPosX - 1.0f, Data.loadPosY);
                break;
            case Direction.Up:
                transform.position = new Vector2(Data.loadPosX, Data.loadPosY + 1.0f);
                break;
            case Direction.Down:
                transform.position = new Vector2(Data.loadPosX, Data.loadPosY - 1.0f);
                break;
            case Direction.N:
                transform.position = new Vector2(startPosX, startPosY);
                break;
        }
    }

    // Update is called once per frame

    void Update()
    {
        //if (rb2d.linearVelocity.x < 5.0f) Debug.Log(rb2d.linearVelocity.x);
        

        nearestGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        if (hp <= 0)
        {
            //hpが0なら入力を受け付けない
            StartCoroutine(Dead());
            return;
            
        }

        if (GameManager.gameState == GameState.Pause) return;

        switch (Data.difficulty)//難易度に応じて速度変更
        {
            case Difficulty.VeryHard:
                dashSpeed = 10.0f;
                walkSpeed = 5.0f;
                break;
            case Difficulty.Hard:
                dashSpeed = 8.0f;
                walkSpeed = 5.0f;
                break;
            case Difficulty.Normal:
                dashSpeed = 8.0f;
                walkSpeed = 5.0f;
                break;
            case Difficulty.Easy:
                dashSpeed = 7.0f;
                walkSpeed = 4.0f;
                break;
        }

        //if (Input.GetAxisRaw("Vertical") != 0f) Debug.Log("あ");
        //if (Input.GetKey(KeyCode.LeftShift)) Debug.Log("い");
        //シフトでダッシュ状態
        //シーン移動時に長押しでダッシュ状態が維持できない
        if (Data.dashWhilePush)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = dashSpeed;
            }
            else
            {
                speed = walkSpeed;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = walkSpeed;
            }
            else
            {
                speed = dashSpeed;
            }
        }


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
        if (axisH != 0 || axisV != 0)
        {
            //上下左右いずれかの入力があるなら、少なくとも動いている状態である
            isMoving = true;
        }
        inputVector = new Vector2(axisH, axisV);//入力ベクトル



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

        //とまったら位置調整
        if (!isMoving) transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        //攻撃をくらった
        if (isAttacked)
        {
            StartCoroutine(Attacked(blownDirection));
        }
    }

    void FixedUpdate()
    {

        if (hp <= 0)
        {
            //hpが0なら入力を受け付けない
            return;
        }

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


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            //吹っ飛ばされているときになにかにぶつかったら最も近い格子点へ
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        StopAllCoroutines();
        transform.position = nearestGrid;
        rb2d.linearVelocity = Vector2.zero;
        isMoving = false;
        isCoroutineWorking = false;
        StartCoroutine(Invincible());
    }

    //--------------上下左右の入力終了後の自動運転--------------------
    private IEnumerator Move(Direction direction)
    {
        isCoroutineWorking = true;//コルーチン始動フラグ

        //すでに移動開始したので、コルーチンが重複しないよう方向フラグoff
        moveDirection = Direction.N;

        float distance = 1.0f;//プレイヤーの現在位置から格子点までの距離
        float isGoal = 0.1f;//ゴールまでの距離がこれ以下だったらゴールとする
        float time = 0f;


        while (true)
        {
            //ゴールまでの距離が一定以下なら以下の処理を毎フレーム行う

            time += Time.deltaTime;
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
        rb2d.linearVelocity = Vector2.zero;
        isMoving = false;
        isCoroutineWorking = false;
        //Debug.Log(time);
    }

    //--------------------被弾モーション--------------------

    IEnumerator Attacked(Vector2 direction)
    {
        isAttacked = false;
        isInvincible = true;//無敵時間
        isCoroutineWorking = true;//入力を受け付けない
        cameraCnt.Vib();
        float time = 0f;
        float waitingTime = 0.3f;

        //ゴールとなる格子点を計算

        Vector2 hitPosition = transform.position;
        float point;

        Vector2 targetGrid = new Vector2(Mathf.Round(transform.position.x + direction.x), Mathf.Round(transform.position.y + direction.y));
        while (true)
        {
            point = -10f * (time - waitingTime) * (time - waitingTime) + 1f;

            transform.position = Vector2.Lerp(hitPosition, targetGrid, point);

            
            time += Time.deltaTime;
            if (time > waitingTime) break;
            
            yield return null;
        }

        transform.position = targetGrid;
        rb2d.linearVelocity = Vector2.zero;
        isCoroutineWorking = false;//入力をまた受け付けるように

        StartCoroutine(Invincible());
    }

    //無敵時間
    IEnumerator Invincible()
    {
        isInvincible = true;

        yield return new WaitForSeconds(1f);//1s無敵

        isInvincible = false;//無敵解除
    }
    
  
    //-------------------やられモーション---------------------------
    IEnumerator Dead()
    {
        //アニメーションを流す
        //入力を拒否する
        float time = 0.0f;
        rb2d.linearVelocity = Vector2.zero;//いったんその場で停止
        while (true)
        {
            time += Time.deltaTime;
            yield return null;
            if (time >= 1.0f) break;
        }
        
    }

    

}
    
    