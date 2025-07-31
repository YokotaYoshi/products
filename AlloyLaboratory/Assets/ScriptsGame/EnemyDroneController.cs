using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDroneController : MonoBehaviour
{
    public float speed = 3.0f;//追跡速度
    GameObject player;//プレイヤープレイヤー
    public GameObject explosion;//爆発オブジェクト
    bool isCheckTime = true;//コルーチン開始フラグ
    Rigidbody2D rb2d;
    public float timeLimit = 999f;//自爆までの時間
    float explosionCount = 0.0f;//カウント

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーを取得
        rb2d = GetComponent<Rigidbody2D>();//Rigidbody2Dを取得
    }

    // Update is called once per frame
    void Update()
    {
        explosionCount += Time.deltaTime;
        if (explosionCount >= timeLimit)
        {
            Explode();
        }
    }

    void FixedUpdate()
    {
        if (isCheckTime)
        {
            //チェックタイムになったら追跡
            StartCoroutine(Chase());
        }
    }

    IEnumerator Chase()
    {
        float time = 0.0f;//コルーチン内の時間
        //数秒おきにプレイヤーの位置を参照し、そこに向かって突撃する
        isCheckTime = false;
        //プレイヤーの方向
        Vector2 playerPosition = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        Vector2 playerDirection = playerPosition.normalized;//正規ベクトル
        //速度を決定
        rb2d.linearVelocity = new Vector2(playerDirection.x * speed, playerDirection.y * speed);
        while(true)
        {
            time += Time.deltaTime;//カウントアップ
            //速度を更新。滑らかに動くように
            yield return null;
            if (time >= 1.0f)
            {
                break;
            }
        }

        isCheckTime = true;//もう一度コルーチンを開始できるように
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーに衝突したら爆発
            Explode();
        }
    }

    //自爆メソッド
    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
