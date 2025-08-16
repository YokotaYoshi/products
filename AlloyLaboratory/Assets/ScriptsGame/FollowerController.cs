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

    bool isMoving = true;//動いているかどうか
    bool startMoving = false;
    Vector2 targetDirection;//自動移動時のゴールの方向
    Vector2 targetPosition;//移動先
    float distance;//プレイヤーとの距離

    float speed = 5.0f;//移動速度
    float gap = 1.0f;//ゴールまでの距離
    Vector2 nearestGrid;


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

    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerPosition();

        speed = playerCnt.speed;//スピードはプレイヤーと常に一致させる

        //最も近い格子点
        nearestGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        if (player != null)
        {
            if (playerRb2d.linearVelocity.magnitude < 1f)
            {
                //プレイヤーが止まっていたらこちらも格子点で静止
                isMoving = false;

                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    startMoving = true;
                }
            }

            
        }
    }

    void FixedUpdate()
    {
        if (!isMoving)
        {
            //格子点で静止
            transform.position = nearestGrid;
            rb2d.linearVelocity = Vector2.zero;


        }
        //プレイヤーを取得出来たら
        if (startMoving)
        {
            Debug.Log("スタート");
            StartCoroutine(Move());
        }
    }

    //プレイヤーとの位置関係
    void CheckPlayerPosition()
    {
        //プレイヤーとの距離を計測
        distance = new Vector2(player.transform.position.x - transform.position.x,
        player.transform.position.y - transform.position.y).magnitude;
    }

    //距離を調べてポジションをリセットする
    void CheckPosition()
    {
        if (distance < 0.1f)
        {
            ResetPosition();
        }
    }
    //ポジションをリセットする
    void ResetPosition()
    {
        transform.position = nearestGrid;
    }

    IEnumerator Move()
    {
        //動いているフラグ立て
        startMoving = false;
        isMoving = true;
        float time = 0.0f;
        float isGoal = 0.1f;//ゴールまでの距離がこれ以下だったらゴールとする

        //ゴールはプレイヤーの座標に最も近い格子点
        targetPosition = new Vector2(Mathf.Round(player.transform.position.x), Mathf.Round(player.transform.position.y));
        //ゴールの方向の正規ベクトル
        targetDirection = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y).normalized;
        //速度を設定
        rb2d.linearVelocity = new Vector2(targetDirection.x * speed, targetDirection.y * speed);

        //まず格子点に
        transform.position = nearestGrid;

        while (true)
        {
            //ゴールまでの距離が一定以下なら以下の処理を毎フレーム行う

            time += Time.deltaTime;

            if (playerRb2d.linearVelocity.magnitude < 0.1f)
            {
                //プレイヤーが止まっていたら
                //Debug.Log("プレイヤー停止");
                //動いているフラグおろし
                isMoving = false;
                //速度をゼロに
                rb2d.linearVelocity = Vector2.zero;
                //座標を格子点に
                transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
                //コルーチン停止
                yield break;
            }

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

            yield return null;
        }

        //Debug.Log(time);
        //動いているフラグおろし
        isMoving = false;
        //座標を格子点に
        //transform.position = new Vector2(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y));
    }
}
