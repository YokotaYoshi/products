using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject startButton;

    //セーブデータをロードする
    public GameObject continueButton;
    public GameObject saveDatasPanel;
    //public GameObject buttonSaveData1;//21.いったんセーブデータはひとつだけ

    //オプション設定。static変数をいじる
    public GameObject optionButton;
    public GameObject optionPanel;
    //public GameObject buttonOption1;//41
    //public GameObject buttonOption2;




    GameObject[] buttons;
    GameObject buttonFocused;

    Color focusColor;
    Color unfocusColor;

    int buttonNum = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuPanel.SetActive(false);
        saveDatasPanel.SetActive(false);
        optionPanel.SetActive(false);

        buttonFocused = startButton;

        focusColor = new Color(0.7f, 0.7f, 1f);//フォーカスされたボタンの色
        unfocusColor = new Color(1f, 1f, 1f);//その他のボタンの色

        buttons = new GameObject[] { startButton, continueButton, optionButton };
    }

    // Update is called once per frame
    void Update()
    {
        if (menuPanel.activeSelf)
        {
            SwitchButtonNum();
            if (InputManager.inputType == InputType.Action)
            {
                if (buttonNum == 0) GameStart();
                else if (buttonNum == 20) Continue();
                else if (buttonNum == 40) GameEnd();
                menuPanel.SetActive(true);
            }
        }
        else if (InputManager.inputType == InputType.Action)
        {
            menuPanel.SetActive(true);
        }
        
        /*
        else if (InputManager.inputType == InputType.Back)
        {
            if (buttonNum / 20 == 1) CloseSaveDatasPanel();
            else if (buttonNum / 20 == 2) CloseOptionPanel();
        }

        if (Data.dashWhilePush)
        {
            buttonOption1.GetComponentInChildren<Text>().text = "シフトを押しているときダッシュする";
        }
        else
        {
            buttonOption1.GetComponentInChildren<Text>().text = "シフトを押しているとき歩く";
        }
        */
    }

    void SwitchButtonNum()
    {
        //ブランチ上部
        //0:ゲーム開始
        //20~:ロード
        //40~:オプション
        if (buttonNum % 20 == 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) buttonNum -= 20;
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) buttonNum += 20;
            if (buttonNum >= 60) buttonNum = 0;
            if (buttonNum <= -1) buttonNum = 40;


            buttonFocused = buttons[buttonNum / 20];
            for (int i = 0; i < 3; ++i)
            {
                buttons[i].GetComponent<Image>().color = unfocusColor;
            }
            buttonFocused.GetComponent<Image>().color = focusColor;
        }
        /*
        else
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) buttonNum -= 1;
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) buttonNum += 1;
            if (buttonNum >= 60) buttonNum = 0;
            if (buttonNum <= -1) buttonNum = 40;
        }

        if (InputManager.inputType == InputType.Back)
        {
            buttonNum = (buttonNum / 20) * 20;
        }
        */
    }

    public void GameStart()
    {
        Data.eventProgressMain = 0;
        Data.eventProgressSub = 0;
        PlayerController.startPos = Direction.N;
        SceneManager.LoadScene("HomeStart");
    }

    public void Continue()
    {
        //データをロード
        SceneManager.LoadScene(PlayerPrefs.GetString("シーン名"));
        Data.eventProgressMain = PlayerPrefs.GetInt("eventProgressMain");
        Data.eventProgressSub = PlayerPrefs.GetInt("eventProgressSub");
        Data.items = PlayerPrefs.GetInt("items");
        for (int i = 0; i < PlayerPrefs.GetInt("items"); ++i)
        {
            Data.itemDataNum[i] = PlayerPrefs.GetInt($"item{i}");
        }
        Data.charas = PlayerPrefs.GetInt("charas");
        for (int i = 0; i < PlayerPrefs.GetInt("charas"); ++i)
        {
            Data.charaDataNum[i] = PlayerPrefs.GetInt($"chara{i}");
        }
    }


    public void OpenSaveDatasPanel()
    {
        saveDatasPanel.SetActive(true);
        buttonNum = 21;
    }

    public void CloseSaveDatasPanel()
    {
        saveDatasPanel.SetActive(false);
        buttonNum = 20;
    }

    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
        buttonNum = 41;
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
        buttonNum = 40;
    }

    //--------------------セーブデータをロード----------------

    public void LoadSaveData()
    {
        //ロードする情報は
        /*
        シーンの名前
        プレイヤーの座標
        仲間情報
        アイテム情報
        eventProgressMain
        eventProgressSub→疑問。謎解きの途中でセーブした場合はどうすべきか
        アイテムをロードする際に矛盾が生じそうなのでロードするべき

        保存した情報を
        buttonNum - 21
        PlayerPrefs.Get???("キー", 中身)
        */
    }

    //--------------------オプション操作---------------------

    public void OptionDash()
    {
        //デフォルトをダッシュにするか
        //モードを反転
        Data.dashWhilePush = !Data.dashWhilePush;
    }

    public void GameEnd()
    {
        //ウィンドウを閉じる
        Debug.Log("ウィンドウを閉じる");
    }
}
