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
    public GameObject optionButton;

    InputManager inputManager;
    
   
    GameObject[] buttons;
    GameObject buttonFocused;

    Color focusColor;
    Color unfocusColor;

    int buttonNum = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuPanel.SetActive(false);
        

        buttonFocused = startButton;

        inputManager = GetComponent<InputManager>();

        focusColor = new Color(0.7f, 0.7f, 1f);//フォーカスされたボタンの色
        unfocusColor = new Color(1f, 1f, 1f);//その他のボタンの色

        buttons = new GameObject[] { startButton, continueButton, optionButton };
    }

    // Update is called once per frame
    void Update()
    {
        if (menuPanel.activeSelf)
        {
            if (inputManager != null) inputManager.isUION = true;
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
        Data.playerLevel = PlayerPrefs.GetInt("playerLevel");
        Data.items = PlayerPrefs.GetInt("items");
        Data.loadPosX = PlayerPrefs.GetFloat("loadPosX");
        Data.loadPosY = PlayerPrefs.GetFloat("loadPosY");
        PlayerController.startPos = Direction.Down;
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


    public void GameEnd()
    {
        //ウィンドウを閉じる
        Debug.Log("ウィンドウを閉じる");
    }
}
