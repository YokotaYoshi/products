using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinalBossController : MonoBehaviour
{
    GameObject playerShooter;
    Vector2 playerPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerShooter = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        SearchPlayer();
    }

    //自機の位置を取得
    void SearchPlayer()
    {
        playerPosition = new Vector2(playerShooter.transform.position.x - transform.position.x,
        playerShooter.transform.position.y - transform.position.y);

    }

    //ファンネルを放出する
    void AttackDrone()
    {

    }

    //自機が近くに来た時範囲攻撃
    void AttackSwing()
    {

    }
    //自機が遠くにいるとき発砲
    void AttackShoot()
    {

    }

}
