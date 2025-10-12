using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuPanelManager : MonoBehaviour
{
    GameObject buttonFocused;//カーソルあってるボタン
    GameObject buttonUnFocused;//カーソル外れたボタン

    //-------------------アイテムタブ--------------------

    GameObject[] buttonsItem;
    GameObject[] buttonsItemText;

    public GameObject buttonItem0;
    public GameObject buttonItem1;
    public GameObject buttonItem1Text;
    public GameObject buttonItem2;
    public GameObject buttonItem2Text;
    public GameObject buttonItem3;
    public GameObject buttonItem3Text;

    public GameObject buttonItem4;
    public GameObject buttonItem4Text;
    public GameObject buttonItem5;
    public GameObject buttonItem5Text;
    public GameObject buttonItem6;
    public GameObject buttonItem6Text;

    //-------------------操作方法タブ----------------------------
    public GameObject buttonHowToPlay;
    //--------------------オプションタブ------------------------

    GameObject[] buttonsOption;

    public GameObject buttonOption0;
    public GameObject buttonOption1;//難易度切り替え
    public GameObject buttonOption2;//ダッシュ切り替え
    //public GameObject buttonOption3;
    //-----------------------ゲームをやめるタブ---------------------

    public GameObject buttonExit;
    public GameObject buttonExitConfirm;

    //-----------------------未振り分け-----------------------------

    public GameObject[] buttons;
    public GameObject panelItem;
    public GameObject panelItemInfo;
    public Text itemNameText;
    public Text itemInfoText;
    public GameObject panelHowToPlay;
    public GameObject panelOption;
    public GameObject panelExit;
    public GameObject[] panels;

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
        buttonsItemText = new GameObject[] { buttonItem1Text, buttonItem2Text, buttonItem3Text, buttonItem4Text, buttonItem5Text, buttonItem6Text };
        buttonsOption = new GameObject[] { buttonOption0, buttonOption1, buttonOption2};
        panels = new GameObject[] { panelItem, panelHowToPlay, panelOption, panelExit };

        panelItemInfo.SetActive(false);
        panelHowToPlay.SetActive(false);
        panelOption.SetActive(false);
        panelExit.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(buttonNum);
        //----------------所持アイテムを表示する--------------
        for (int i = 0; i < Data.items; ++i)
        {
            buttonsItem[i + 1].SetActive(true);

            //ボタンの表示名を変更
            buttonsItemText[i].GetComponent<Text>().text = Data.itemDataAll[Data.itemDataNum[i]][0];
        }
        for (int i = Data.items; i < 6; ++i)
        {
            buttonsItem[i + 1].SetActive(false);
        }
        //-----------------オプションで今の状態を表示する-----------
        //オプションの説明にデータから取得したテキストを入れる
        if (InputManager.inputType == InputType.Action)
        {
            Debug.Log("オプション操作");
            //決定ボタンを押されたとき関数を呼ぶ
            if (buttonNum == 12) OptionDifficulty();
            if (buttonNum == 13) Exit();
            if (buttonNum == 22) OptionDash();
        }

        switch (Data.currentDifficulty)
        {
            case Difficulty.Auto://オート
                buttonOption1.GetComponentInChildren<Text>().text = "難易度：オート";
                break;
            case Difficulty.VeryHard://とても難しい
                buttonOption1.GetComponentInChildren<Text>().text = "難易度：とても難しい";
                break;
            case Difficulty.Hard://難しい
                buttonOption1.GetComponentInChildren<Text>().text = "難易度：難しい";
                break;
            case Difficulty.Normal://普通
                buttonOption1.GetComponentInChildren<Text>().text = "難易度：普通";
                break;
            case Difficulty.Easy://簡単
                buttonOption1.GetComponentInChildren<Text>().text = "難易度：簡単";
                break;
        }
        if (Data.dashWhilePush)
        {
            buttonOption2.GetComponentInChildren<Text>().text = "シフトを押しているときダッシュする";
        }
        else
        {
            buttonOption2.GetComponentInChildren<Text>().text = "シフトを押しているとき歩く";
        }
        

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
                if (Data.items == 0 && buttonNum == 0)
                {
                    return;
                }
                else
                {
                    buttonNum += 10;
                }

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

                if (buttonNum == 5) buttonNum = 0;
                if (buttonNum > 5 + Data.items * 5) buttonNum = 5 + Data.items * 5;
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
            //アイテムタブボタン
            buttonUnFocused = buttonFocused;
            buttonFocused = buttonsItem[0];
            panelItemInfo.SetActive(false);
        }
        else if (num % 5 == 0)
        {
            //アイテムボタン
            buttonUnFocused = buttonFocused;
            buttonFocused = buttonsItem[(num - 5) / 5];
            //アイテム情報にデータから取得したテキストを入れる
            panelItemInfo.SetActive(true);
            itemNameText.text = Data.itemDataAll[Data.itemDataNum[(num - 10) / 5]][0];
            itemInfoText.text = Data.itemDataAll[Data.itemDataNum[(num - 10) / 5]][1];
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
        //パネルを切り替える
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

    //-------------------ボタンクリック時の処理---------------
    public void ShowPanel(int num)
    {
        preButtonNum = buttonNum;
        buttonNum = num;

        if (buttonNum != preButtonNum)
        {
            SwitchButtonFocused(buttonNum);
        }
    }

    //-------------------オプション操作--------------------
    public void OptionDifficulty()
    {
        switch (Data.currentDifficulty)
        {
            case Difficulty.Auto://オート→とても難しい
                Data.difficulty = Difficulty.VeryHard;
                Data.currentDifficulty = Difficulty.VeryHard;
                break;
            case Difficulty.Easy://簡単→オート
                //Data.difficulty = Difficulty.Auto;//オート設定はGameManagerにて
                Data.currentDifficulty = Difficulty.Auto;
                break;
            case Difficulty.Normal://普通→簡単
                Data.difficulty = Difficulty.Easy;
                Data.currentDifficulty = Difficulty.Easy;
                break;
            case Difficulty.Hard://難しい→普通
                Data.difficulty = Difficulty.Normal;
                Data.currentDifficulty = Difficulty.Normal;
                break;
            case Difficulty.VeryHard://とても難しい→難しい
                Data.difficulty = Difficulty.Hard;
                Data.currentDifficulty = Difficulty.Hard;
                break;
            
        }
    }
    public void OptionDash()
    {
        //デフォルトをダッシュにするか
        //モードを反転
        Data.dashWhilePush = !Data.dashWhilePush;
    }

    //--------------------ゲーム終了----------------------
    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }
}
