using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFocusCS : MonoBehaviour
{
    //プレイヤーが視界に入っているとき追いかける場合と無条件に追いかける場合で分けたい
    public GameObject enemy;//敵のゲームオブジェクト
    EnemyChaseController enemyChaseCnt;//敵のスクリプト
    public float offset = 0.5f;//敵の中心からの距離
    BlockScript blockScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyChaseCnt = enemy.GetComponent<EnemyChaseController>();//敵のスクリプトを取得
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
        }
        else if (enemyChaseCnt.playerDirectionDegree >= 50 && enemyChaseCnt.playerDirectionDegree < 130)
        {
            //プレイヤーが上のほうにいる
            //敵の上に移動
            transform.position = new Vector2(enemy.transform.position.x, enemy.transform.position.y + offset);
        }
        else if (enemyChaseCnt.playerDirectionDegree >= -130 && enemyChaseCnt.playerDirectionDegree < -50)
        {
            //プレイヤーが下のほうにいる
            //敵の下に移動
            transform.position = new Vector2(enemy.transform.position.x, enemy.transform.position.y - offset);
        }
        else
        {
            //プレイヤーが左のほうにいる
            //敵の左に移動
            transform.position = new Vector2(enemy.transform.position.x - offset, enemy.transform.position.y);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "PlayerFocus" && other.gameObject.tag != "Censor")
        {
            enemyChaseCnt.isBlocked = true;//プレイヤー以外のオブジェクトが目の前にあることを通知

            blockScript = other.GetComponent<BlockScript>();
            if (blockScript != null)
            {
                enemyChaseCnt.right = blockScript.right;
                enemyChaseCnt.left = blockScript.left;
                enemyChaseCnt.up = blockScript.up;
                enemyChaseCnt.down = blockScript.down;
            }
            else
            {
                enemyChaseCnt.right = 1f;
                enemyChaseCnt.left = 1f;
                enemyChaseCnt.up = 1f;
                enemyChaseCnt.down = 1f;
            }
            
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "PlayerFocus" && other.gameObject.tag != "Censor")
        {
            //目の前が開けている
            enemyChaseCnt.isBlocked = false;
        }
    }
}
