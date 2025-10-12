using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFocus : MonoBehaviour
{
    //プレイヤーの関与するイベントを主に担当する
    
    //-----------------------位置に関する情報--------------------------
    float amptitude = 0.02f;
    float time = 0f;
    float delta;
    float axisH = 0.0f;
    float axisV = 0.0f;
    float positionX = 0.0f;
    float positionY = 0.0f;
    GameObject player;//プレイヤー
    PlayerController playerCnt;//プレイヤーコントローラー
    
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == GameState.Pause) return;
        //--------------------------位置についての記述-------------------------------
        //ベクトル(axisH, axisV)は(0,0),(+-1,0),(0,+-1)のいずれか
        if (axisV == 0)
        {
            axisH = Input.GetAxisRaw("Horizontal");
        }
        if (axisH == 0)
        {
            axisV = Input.GetAxisRaw("Vertical");
        }
        //直前の入力を保存
        if (axisH == 1.0f)
        {
            positionX = 1.0f;
            positionY = 0.0f;
        }
        else if (axisH == -1.0f)
        {
            positionX = -1.0f;
            positionY = 0.0f;
        }
        else if (axisV == 1.0f)
        {
            positionX = 0.0f;
            positionY = 1.0f;
        }
        else if (axisV == -1.0f)
        {
            positionX = 0.0f;
            positionY = -1.0f;
        }
        //座標はプレイヤーの見ている方向
        //接触判定を出すため振動させる
        time += Time.deltaTime;
        delta = amptitude * Mathf.Sin(time * Mathf.PI);
        transform.position = new Vector2(player.transform.position.x + positionX / 2 + delta, player.transform.position.y + positionY / 2); 
        //Debug.Log(transform.position);
        
    }
        
}
