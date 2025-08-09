using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuPanelManager : MonoBehaviour
{

    public GameObject buttonItem;
    public GameObject buttonHowToPlay;
    public GameObject buttonOption;
    public GameObject buttonStopPlaying;
    int buttonNum = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //方向キー操作で押すボタンがわかるようにしたい
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(buttonNum);
        //ボタンナンバー
        if (buttonNum <= 9)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (buttonNum == 0)
                {
                    //アイテムパネル表示
                }
                else if (buttonNum == 1)
                {
                    //操作方法パネル表示
                }
                else if (buttonNum == 0)
                {
                    //オプションパネル表示
                }
                else if (buttonNum == 0)
                {
                    //ゲームをやめるかどうか聞くパネル表示
                }
            }

            //0~3はタブ
            if (buttonNum >= 4)
            {
                buttonNum = 0;
            }
            if (buttonNum <= -1)
            {
                buttonNum = 3;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                //→
                buttonNum += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                //←
                buttonNum -= 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                //↓
                buttonNum += 10;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //buttonNumに対応したアイテムの説明を
                //そしてそれを使うかどうか
            }
            //10~はアイテム
                if (buttonNum <= 14)
                {
                    buttonNum = 10;
                }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                //→
                buttonNum += 5;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                //←
                buttonNum -= 5;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                //↑
                buttonNum -= 10;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                //↓
                buttonNum += 10;
            }
        }
    }
}
