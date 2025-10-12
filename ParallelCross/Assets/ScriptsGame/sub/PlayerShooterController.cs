using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShooterController : MonoBehaviour
{
    float axisV;
    float axisH;
    public float speed = 10f;
    Rigidbody2D rb2d;
    GameObject finalBoss;
    Vector2 bossPosition;
    public GameObject bulletShootBoss;//ボスを狙う弾丸
    public GameObject bulletShootOther;//ボス以外を狙う弾丸
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        finalBoss = GameObject.FindGameObjectWithTag("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        //ここの動きを調節するのは必須
        axisV = (Input.GetAxis("Vertical") + 2 * Input.GetAxisRaw("Vertical")) / 3;
        axisH = (Input.GetAxis("Horizontal") + 2 * Input.GetAxisRaw("Horizontal")) / 3;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackShootBoss();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AttackShootEnemy();
        }
    }

    //自由に動きまわれる
    void FixedUpdate()
    {
        rb2d.linearVelocity = new Vector2(axisH * speed, axisV * speed);
    }

    void SearchEnemy()
    {
        //一番近くの敵の位置を取得する
    }

    void AttackShootEnemy()
    {
        //一番近くの敵を狙う
        Instantiate(bulletShootOther, transform.position, Quaternion.identity);
    }

    void SearchBoss()
    {
        //ボスの位置を取得する
        bossPosition = new Vector2(finalBoss.transform.position.x - transform.position.x,
        finalBoss.transform.position.y - transform.position.y);
    }

    void AttackShootBoss()
    {
        //ボスを狙う
        Instantiate(bulletShootBoss, transform.position, Quaternion.identity);
    }

    void AttackCounter()
    {
        //カウンター攻撃
        //敵の近くで決定ボタンでダメージ無効化、短時間無敵、相手にダメージ
    }
}
