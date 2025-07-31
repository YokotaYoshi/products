using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCarController : MonoBehaviour
{
    public float speed = 5.0f;
    Rigidbody2D rb2d;
    float axisH = 0.0f;
    float axisV = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Rigidbody2Dを取得
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //水平方向入力チェック
        axisH = Input.GetAxisRaw("Horizontal");
        //鉛直方向入力チェック
        axisV = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        //速度を更新
        rb2d.linearVelocity = new Vector2(axisH, axisV).normalized*speed;
    }
}
