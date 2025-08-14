using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuPanelManager : MonoBehaviour
{
    GameObject buttonFocused;//カーソルあってるボタン
    GameObject buttonUnFocused;//カーソル外れたボタン

    GameObject[] buttonsItem;
    public GameObject buttonItem0;
    public GameObject buttonItem1;
    public GameObject buttonItem2;
    public GameObject buttonItem3;

    public GameObject buttonItem4;
    public GameObject buttonItem5;
    public GameObject buttonItem6;
    public GameObject buttonHowToPlay;

    GameObject[] buttonsOption;

    public GameObject buttonOption0;
    public GameObject buttonOption1;
    public GameObject buttonOption2;
    public GameObject buttonOption3;

    public GameObject buttonExit;
    public GameObject buttonExitConfirm;

    public GameObject[] buttons;
    public GameObject panelItem;
    public GameObject panelHowToPlay;
    public GameObject panelOption;
    public GameObject panelExit;
    public GameObject[] panels;



    public int items = 6;
    public int options = 3;

    Color focusColor;
    Color unfocusColor;
    int preButtonNum = 0;
    public int buttonNum = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //方向キー操作で押すボタンがわかるようにしたい
    void Start()
    {
        buttonFocused = buttonItem0;//最初はアイテムボタンにフォーカス
        buttonUnFocused = buttonHowToPlay;//最初は仮にフォーカスされていないボタンを
        focusColor = new Color(0.7f, 0.7f, 1f);//フォーカスされたボタンの色
        unfocusColor = new Color(1f, 1f, 1f);//その他のボタンの色

        buttons = new GameObject[] { buttonItem0, buttonHowToPlay, buttonOption0, buttonExit };
        buttonsItem = new GameObject[] { buttonItem0, buttonItem1, buttonItem2, buttonItem3, buttonItem4, buttonItem5, buttonItem6 };
        buttonsOption = new GameObject[] { buttonOption0, buttonOption1, buttonOption2, buttonOption3};
        panels = new GameObject[] { panelItem, panelHowToPlay, panelOption, panelExit };

        panelHowToPlay.SetActive(false);
        panelOption.SetActive(false);
        panelExit.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(buttonNum);
        SwitchButtonNum();
        FocusButton();

        PushButton();
    }


    //-------------入力に応じてbuttonNumを切り替え---------------
    void SwitchButtonNum()
    {
        preButtonNum = buttonNum;
        //ボタンナンバー切りかえ
        if (buttonNum <= 9)
        {
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
            //10~は小項目
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

            if (buttonNum % 5 == 0)
            {
                //アイテム欄
                //数値の上限


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
                
                if (buttonNum > 5 + items * 5) buttonNum = 5 + items * 5;
            }
            else if (buttonNum % 10 == 1)
            {
                buttonNum = 1;
            }
            else if (buttonNum % 10 == 2)
            {
                //数値の上限
                if (buttonNum > 2 + options * 10) buttonNum = 2 + options * 10;
            }
            else if (buttonNum % 10 == 3)
            {
                if (buttonNum > 13) buttonNum = 13;
            }

        }

        if (buttonNum != preButtonNum)
        {
            SwitchButtonFocused(buttonNum);
        }
    }

    //-----------------buttonNumに応じてフォーカスするボタンを設定-------------
    void SwitchButtonFocused(int num)
    {
        //数値が切り替わった時だけ呼び出したい
        if (num == 0)
        {
            //アイテムボタン
            buttonUnFocused = buttonFocused;
            buttonFocused = buttonsItem[0];
        }
        else if (num % 5 == 0)
        {
            //アイテムボタン
            buttonUnFocused = buttonFocused;
            buttonFocused = buttonsItem[(num-5) / 5];
        }
        else if (num == 1)
        {
            //操作方法ボタン
            buttonUnFocused = buttonFocused;
            buttonFocused = buttonHowToPlay;
        }
        else if (num % 10 == 2)
        {
            //オプションボタン
            buttonUnFocused = buttonFocused;
            buttonFocused = buttonsOption[(num - 2) / 10];
        }
        else if (num == 3)
        {
            //ゲームをやめるかどうか聞くボタン
            buttonUnFocused = buttonFocused;
            buttonFocused = buttonExit;
        }
        else if (num == 13)
        {
            buttonUnFocused = buttonExit;
            buttonFocused = buttonExitConfirm;
        }
    }

    //---------------buttonFocusedに色付け------------------------
    void FocusButton()
    {
        buttonUnFocused.GetComponent<Image>().color = unfocusColor;
        buttonFocused.GetComponent<Image>().color = focusColor;
    }

    //--------------決定ボタンが押された時の処理------------------
    void PushButton()
    {
        for (int i = 0; i < 4; i++)
        {
            panels[i].SetActive(false);
        }
        if (buttonNum % 5 == 0)
        {
            panelItem.SetActive(true);
        }
        else if (buttonNum % 10 == 1)
        {
            panelHowToPlay.SetActive(true);
        }
        else if (buttonNum % 10 == 2)
        {
            panelOption.SetActive(true);
        }
        else if (buttonNum % 10 == 3)
        {
            panelExit.SetActive(true);
        }
    }

    //-------------------各ボタンクリック時の処理---------------
    public void ShowItems()
    {
        preButtonNum = buttonNum;
        buttonNum = 0;

        if (buttonNum != preButtonNum)
        {
            SwitchButtonFocused(buttonNum);
        }
    }

    public void ShowHowToPlay()
    {
        preButtonNum = buttonNum;
        buttonNum = 1;

        if (buttonNum != preButtonNum)
        {
            SwitchButtonFocused(buttonNum);
        }
    }
    public void ShowOptions()
    {
        preButtonNum = buttonNum;
        buttonNum = 2;

        if (buttonNum != preButtonNum)
        {
            SwitchButtonFocused(buttonNum);
        }
    }
    public void ShowExitGame()
    {
        preButtonNum = buttonNum;
        buttonNum = 3;

        if (buttonNum != preButtonNum)
        {
            SwitchButtonFocused(buttonNum);
        }
    }
}
