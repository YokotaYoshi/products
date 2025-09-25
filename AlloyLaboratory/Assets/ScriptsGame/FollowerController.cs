using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FollowerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Rigidbody2D playerRb2d;//プレイヤーのRigidbody2D
    GameObject player;//プレイヤー
    PlayerController playerCnt;//プレイヤーコントローラー
    Vector2 playerPosition;//キャラから見たプレイヤーの位置

    bool isMoving = false;//動いているかどうか
    bool isCoroutineWorking = false;//コルーチン中かどうか
    Vector2 targetDirection;//自動移動時のゴールの方向
    Vector2 targetPosition;//移動先
    
    float distance;//プレイヤーとの距離

    float speed = 5.0f;//移動速度
    float gap = 1.0f;//ゴールまでの距離
    Vector2 nearestGrid;
    public float startPosX;
    public float startPosY;
    public Direction moveDirection = Direction.N;
    public Direction playerDirection = Direction.N;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //プレイヤーを取得
        player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーのRigidbody2Dを取得
        playerRb2d = player.GetComponent<Rigidbody2D>();
        //プレイヤーコントローラーを取得
        playerCnt = player.GetComponent<PlayerController>();
        //Rigidbody2Dを取得
        rb2d = GetComponent<Rigidbody2D>();

        transform.position = new Vector2(Data.loadPosX, Data.loadPosY);
        nearestGrid = new Vector2(Data.loadPosX, Data.loadPosY);
        Debug.Log(transform.position);

        //プレイヤーと離れていた場合について
    }


    // Update is called once per frame
    void Update()
    {
        if (Data.charaData[1][0] == null) Destroy(gameObject);

        CheckPlayerPosition();

        speed = PlayerController.speed;//スピードはプレイヤーと常に一致させる

        isMoving = playerCnt.isMoving;//プレイヤーが移動しているなら移動している

        if (Input.GetAxisRaw("Horizontal") == 1f) moveDirection = Direction.Right;
        else if (Input.GetAxisRaw("Horizontal") == -1f) moveDirection = Direction.Left;
        else if (Input.GetAxisRaw("Vertical") == 1f) moveDirection = Direction.Up;
        else if (Input.GetAxisRaw("Vertical") == -1f) moveDirection = Direction.Down;
        else if (!isMoving) moveDirection = Direction.N;

        //最も近い格子点
        nearestGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        //入力方向と自機方向が一致するかどうかで動きを変更

        //離れすぎたときに位置修正
        
        if (distance >= 1.5f)
        {
            if (playerDirection == Direction.Right)
            {
                transform.position = new Vector2(player.transform.position.x - 1f, player.transform.position.y);
            }
            else if (playerDirection == Direction.Left)
            {
                transform.position = new Vector2(player.transform.position.x + 1f, player.transform.position.y);
            }
            else if (playerDirection == Direction.Up)
            {
                transform.position = new Vector2(player.transform.position.x, player.transform.position.y - 1f);
            }
            else if (playerDirection == Direction.Down)
            {
                transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.0f);
            }
        }
        
    }

    void FixedUpdate()
    {
        if (isMoving && !isCoroutineWorking)
        {
            //コルーチン開始していなかったら開始
            if (playerDirection == moveDirection)
            {
                if (playerDirection == Direction.Right)
                {
                    rb2d.linearVelocity = new Vector2(speed, 0f);
                }
                else if (playerDirection == Direction.Left)
                {
                    rb2d.linearVelocity = new Vector2(-speed, 0f);
                }
                else if (playerDirection == Direction.Up)
                {
                    rb2d.linearVelocity = new Vector2(0f, speed);
                }
                else
                {
                    rb2d.linearVelocity = new Vector2(0f, -speed);
                }
            }
            else
            {
                StartCoroutine(Follow());
            }
            
        }

        if (!isMoving)
        {
            transform.position = nearestGrid;
            rb2d.linearVelocity = Vector2.zero;
        }

    }

    //プレイヤーとの位置関係
    void CheckPlayerPosition()
    {
        //プレイヤーとの距離を計測
        playerPosition = new Vector2(player.transform.position.x - transform.position.x,
        player.transform.position.y - transform.position.y);
        distance = playerPosition.magnitude;

        if (playerPosition.y > playerPosition.x && playerPosition.y > -playerPosition.x)
            playerDirection = Direction.Up;
        else if (playerPosition.y < playerPosition.x && playerPosition.y < -playerPosition.x)
            playerDirection = Direction.Down;
        else if (playerPosition.y < playerPosition.x && playerPosition.y > -playerPosition.x)
            playerDirection = Direction.Right;
        else
            playerDirection = Direction.Left;
    }

    IEnumerator Follow()
    {
        //動いているフラグ立て
        isCoroutineWorking = true;
        float isGoal = 0.1f;//ゴールまでの距離がこれ以下だったらゴールとする

        //ゴールはプレイヤーの座標に最も近い格子点
        targetPosition = new Vector2(Mathf.Round(player.transform.position.x), Mathf.Round(player.transform.position.y));
        //ゴールの方向の正規ベクトル
        targetDirection = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y).normalized;

        //まず格子点に
        //transform.position = nearestGrid;

        while (true)
        {
            //ゴールまでの距離が一定以下なら以下の処理を毎フレーム行う


            //ゴールの方向の正規ベクトルを更新
            targetDirection = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y).normalized;
            //速度を更新し、ダッシュに対応する
            rb2d.linearVelocity = new Vector2(targetDirection.x * speed, targetDirection.y * speed);
            //ゴールまでの距離を更新
            gap = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y).magnitude;

            //ゴールに十分近づいたらおわり
            if (gap < isGoal)
            {
                break;
            }

            if (playerCnt.isMoving == false)
            {
                //プレイヤーが止まっていたら
                Debug.Log("プレイヤー停止");
                //動いているフラグおろし
                isCoroutineWorking = false;

                //速度をゼロに
                rb2d.linearVelocity = Vector2.zero;
                //座標を格子点に
                transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
                //コルーチン停止
                yield break;
            }

            isGoal = 0.01f * speed;

            yield return null;
        }

        //動いているフラグおろし
        isCoroutineWorking = false;

        //速度をゼロに
        //rb2d.linearVelocity = Vector2.zero;
        //座標を格子点に
        //transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }
    
    
}
