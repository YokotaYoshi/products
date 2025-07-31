using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFocusCS : MonoBehaviour
{
    //プレイヤーが視界に入っているとき追いかける場合と無条件に追いかける場合で分けたい
    public GameObject enemy;//敵のゲームオブジェクト
    EnemyChaseController enemyChaseCnt;//敵のスクリプト
    Rigidbody2D enemyRb2d;//敵の物理挙動
    float offset = 0.5f;//敵の中心からの距離
    int axisH = 0;//敵の中心からの横方向
    int axisV = 0;//敵の中心からの縦方向

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyChaseCnt = enemy.GetComponent<EnemyChaseController>();//敵のスクリプトを取得
        enemyRb2d = enemy.GetComponent<Rigidbody2D>();//敵の物理挙動を取得
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(enemyChaseCnt.playerDirectionDegree);
        //座標をプレイヤーの方向に向ける
        //実際に動く方向を決定
        if (enemyChaseCnt.playerDirectionDegree >= -50 && enemyChaseCnt.playerDirectionDegree < 50)
        {
            //プレイヤーが右のほうにいる
            //敵の右に移動
            transform.position = new Vector2(enemy.transform.position.x + offset, enemy.transform.position.y);
            axisH = 1;
            axisV = 0;
        }
        else if (enemyChaseCnt.playerDirectionDegree >= 50 && enemyChaseCnt.playerDirectionDegree < 130)
        {
            //プレイヤーが上のほうにいる
            //敵の敵の上に移動
            transform.position = new Vector2(enemy.transform.position.x, enemy.transform.position.y + offset);
            axisH = 0;
            axisV = 1;
        }
        else if (enemyChaseCnt.playerDirectionDegree >= -130 && enemyChaseCnt.playerDirectionDegree < -50)
        {
            //プレイヤーが下のほうにいる
            //敵の敵の下に移動
            transform.position = new Vector2(enemy.transform.position.x, enemy.transform.position.y - offset);
            axisH = 0;
            axisV = -1;
        }
        else
        {
            //プレイヤーが左のほうにいる
            //敵の左に移動
            transform.position = new Vector2(enemy.transform.position.x - offset, enemy.transform.position.y);
            axisH = -1;
            axisV = 0;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Block")
        {
            enemyChaseCnt.isBlocked = true;//プレイヤー以外のオブジェクトが目の前にあることを通知
            if (axisH == 1)
            {
                //プレイヤーが右のほうにいるなら、上下によける動き
                enemyChaseCnt.down = other.GetComponent<BlockScript>().down;//接触したブロックの情報を取得して代入
                enemyChaseCnt.right = 0.0f;
                enemyChaseCnt.up = other.GetComponent<BlockScript>().up;//接触したブロックの情報を取得して代入
                enemyChaseCnt.left = 0.0f;
            }
            else if (axisH == -1)
            {
                //プレイヤーが左のほうにいるなら、上下によける動き
                enemyChaseCnt.down = other.GetComponent<BlockScript>().down;//接触したブロックの情報を取得して代入
                enemyChaseCnt.right = 0.0f;
                enemyChaseCnt.up = other.GetComponent<BlockScript>().up;//接触したブロックの情報を取得して代入
                enemyChaseCnt.left = 0.0f;
            }
            else if (axisV == 1)
            {
                //プレイヤーが上のほうにいるなら、左右に避ける動き
                enemyChaseCnt.down = 0.0f;
                enemyChaseCnt.right = other.GetComponent<BlockScript>().right;//接触したブロックの情報を取得して代入
                enemyChaseCnt.up = 0.0f;
                enemyChaseCnt.left = other.GetComponent<BlockScript>().left;//接触したブロックの情報を取得して代入
            }
            else
            {
                //プレイヤーが下のほうにいるなら、左右によける動き
                enemyChaseCnt.down = 0.0f;
                enemyChaseCnt.right = other.GetComponent<BlockScript>().right;//接触したブロックの情報を取得して代入
                enemyChaseCnt.up = 0.0f;
                enemyChaseCnt.left = other.GetComponent<BlockScript>().left;//接触したブロックの情報を取得して代入
            }
            
        }
        else if (other.gameObject.tag != "Player")
        {
            //プレイヤー以外のオブジェクトが目の前にあることを通知
            enemyChaseCnt.isBlocked = true;
            enemyChaseCnt.down = 1.0f;
            enemyChaseCnt.right = 1.0f;
            enemyChaseCnt.up = 1.0f;
            enemyChaseCnt.left = 1.0f;
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            //目の前が開けている
            enemyChaseCnt.isBlocked = false;
        }
    }
}
