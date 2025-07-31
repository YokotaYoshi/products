using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStraightController : MonoBehaviour
{
    public float baseSpeedX = 0.0f;//X方向基準となる速度
    public float baseSpeedY = 0.0f;//Y方向基準となる速度
    Rigidbody2D rb2d;
    float speedAmplitudeX;//x方向速度振動の振幅
    float speedDisplacementX;//x方向速度変位
    float speedAmplitudeY;//y方向速度振動の振幅
    float speedDisplacementY;//y方向速度変位
    public float time = 0f;//負の値にすると最初停止させられる
    float t;
    //bool isMoving = true;//動いているかどうか
    public GameObject explosion;//爆発オブジェクト

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();//Rigidbody2Dを取得
        speedAmplitudeX = baseSpeedX / 10;//x速度の振動の振幅は仮に基準の10%
        speedAmplitudeY = baseSpeedY / 10;//y速度の振動の振幅は仮に基準の10%
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.hp >= 1)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = -1.0f;
        }
        
        if (time >= 2.0f)
        {
            time = 0;//タイマーリセット
        }
        t = time * Mathf.PI;
        speedDisplacementX = speedAmplitudeX * Mathf.Sin(t);//周期は2秒
        speedDisplacementY = speedAmplitudeY * Mathf.Sin(t);//周期は2秒

    }

    void FixedUpdate()
    {
        if (time >= 0.0f)
        {
            //速度を更新
            rb2d.linearVelocity = new Vector2(baseSpeedX + speedDisplacementX, baseSpeedY + speedDisplacementY);
        }
        else
        {
            //timeが負の値なら停止
            rb2d.linearVelocity = Vector2.zero;
        }
        //Debug.Log(rb2d.linearVelocity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            time = -1f;
            //timeを負の値にして停止させる。
        }
        else
        {
            Explode();//壁にぶつかると爆発
        }
    }

    //爆発メソッド
    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
        //爆発オブジェクトを生成したい
    }
}
