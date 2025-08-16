using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    Start,
    Playing,
    Pause,
    GameOver,
}

public enum InputType
{
    Null,
    Action,
    Back,
}
public class GameManager : MonoBehaviour
{
    //Fungusの切り替えとかに置き換えていく
    //UIを担当
    //主にパネルのONOFFを行う
    //ONにしたら、そのあとは個別に処理する
    //float debugTime = 0;//デバッグ用のタイマー

    GameState gameState = GameState.Playing;
    public static InputType inputType = InputType.Null;

    //------------------左端の情報パネル----------------------
    public GameObject informationPanel;//左端の情報パネル

    //hp処理
    public Image hp1;
    public Image hp2;
    public Image hp3;

    //----------------------セーブデータパネル
    public GameObject saveDatasPanel;
    //--------------------その他-------------------------

    GameObject player;//プレイヤー
    PlayerController playerCnt;//プレイヤーコントローラー
    GameObject playerFocus;//プレイヤーの目線
    PlayerFocus playerFocusCS;//PlayerFocusスクリプト
    //イベントのフラグ
    public static int eventProgress = 0;//この数値を切り替えることでイベント進行
    float debugTime = 0f;
    public GameObject menuPanel;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //--------------------最初は非表示のもの------------------------
        if (saveDatasPanel != null)
        {
            saveDatasPanel.SetActive(false);
        }
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }



        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーを取得
        playerCnt = player.GetComponent<PlayerController>();//プレイヤーコントローラーを取得
        playerFocus = GameObject.FindGameObjectWithTag("PlayerFocus");//プレイヤーの目線を取得
        playerFocusCS = playerFocus.GetComponent<PlayerFocus>();//PlayerFocusスクリプト取得
    }

    // Update is called once per frame
    void Update()
    {
        

        switch (gameState)
        {
            case GameState.Start:
                //Debug.Log("スタート画面");
                break;
            case GameState.Playing:
                //Debug.Log("プレイ中");
                break;
            case GameState.Pause:
                //Debug.Log("ポーズ中");
                break;
            case GameState.GameOver:
                //Debug.Log("ゲームオーバー");
                break;
        }

        //入力があれば状態変化
        InputTypeManagement();

        if (inputType == InputType.Back)
        {
            MenuPanelButton();
        }

        //----------------------------------セーブポイント-----------------------------------
        if (playerFocusCS.isSaveReady)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 0;//ゲーム停止

                saveDatasPanel.SetActive(true);//セーブデータ表示
            }
        }

        //--------------------------体力処理---------------------------
        if (playerCnt != null && hp1 != null && hp2 != null && hp3 != null)
        {
            if (PlayerController.hp >= 3)
            {
                hp1.gameObject.SetActive(true);
                hp2.gameObject.SetActive(true);
                hp3.gameObject.SetActive(true);
            }
            else if (PlayerController.hp == 2)
            {
                hp1.gameObject.SetActive(true);
                hp2.gameObject.SetActive(true);
                hp3.gameObject.SetActive(false);
            }
            else if (PlayerController.hp == 1)
            {
                hp1.gameObject.SetActive(true);
                hp2.gameObject.SetActive(false);
                hp3.gameObject.SetActive(false);
            }
            else
            {
                //体力0=ゲームオーバー
                debugTime += Time.deltaTime;
                Debug.Log(debugTime);
                hp1.gameObject.SetActive(false);
                hp2.gameObject.SetActive(false);
                hp3.gameObject.SetActive(false);
                Invoke("GameOver", 1.0f);
                gameState = GameState.GameOver;
            }
        }

        if (inputType != InputType.Null)
        {
            //Debug.Log(inputType);
        }
        
    }

    void FixedUpdate()
    {
        //Debug.Log(Time.deltaTime);
    }

    //------------------------入力切替メソッド-----------------------
    public void InputTypeManagement()
    {
        //入力を変数に変更
        //決定:左クリック、Z、Enter
        //戻る:右クリック、X、Space
        //ダッシュ:左右Shift
        //最初に入力をニュートラルに戻す
        inputType = InputType.Null;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        {
            inputType = InputType.Action;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(1))
        {
            inputType = InputType.Back;
        }
    }

    //-----------------------ゲームオーバーメソッド--------------------
    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    //-------------------セーブ画面を閉じる--------------------------
    public void CloseSavePanel()
    {
        saveDatasPanel.SetActive(false);
        Time.timeScale = 1;
    }
    //-------------------メニューのオンオフ切り替え--------------------
    public void MenuPanelButton()
    {
        
        if (!menuPanel.activeSelf)
        {
            menuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            menuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
