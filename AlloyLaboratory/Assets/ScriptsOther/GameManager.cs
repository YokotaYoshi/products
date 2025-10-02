using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fungus;


public class GameManager : MonoBehaviour
{
    //Fungusの切り替えとかに置き換えていく
    //UIを担当
    //主にパネルのONOFFを行う
    //ONにしたら、そのあとは個別に処理する
    //float debugTime = 0;//デバッグ用のタイマー

    public static GameState gameState;

    //------------------左端の情報パネル----------------------
    //public GameObject informationPanel;//左端の情報パネル

    //hp処理
    public Image hp1;
    public Image hp2;
    public Image hp3;

    //----------------------セーブデータパネル
    //public GameObject saveDatasPanel;
    //--------------------その他-------------------------

    GameObject player;//プレイヤー
    PlayerController playerCnt;//プレイヤーコントローラー
    GameObject playerFocus;//プレイヤーの目線
    PlayerFocus playerFocusCS;//PlayerFocusスクリプト
    
    public GameObject menuPanel;
    public GameObject enemy;
    bool isGameOverStarting = false;
    public GameObject choicesPanel;
    //--------------Fungus------------------------------
    public Flowchart flowchart;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //--------------------最初は非表示のもの------------------------
        
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

        if (gameState == GameState.Run)
        {
            //敵出現コルーチン
            StartCoroutine(AppearEnemy());
        }

        choicesPanel.SetActive(false);


        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーを取得
        playerCnt = player.GetComponent<PlayerController>();//プレイヤーコントローラーを取得
        playerFocus = GameObject.FindGameObjectWithTag("PlayerFocus");//プレイヤーの目線を取得
        playerFocusCS = playerFocus.GetComponent<PlayerFocus>();//PlayerFocusスクリプト取得
    }

    // Update is called once per frame
    void Update()
    {
        //DataクラスのeventProgressをfungusのeventProgressと同期させる
        Data.eventProgressMain = flowchart.GetIntegerVariable("eventProgressMain");
        Data.eventProgressSub = flowchart.GetIntegerVariable("eventProgressSub");

        
        //ゲームステート切り替え
        switch (gameState)
        {
            case GameState.Start:
                //Debug.Log("スタート画面");
                break;
            case GameState.Playing:
                //Debug.Log("プレイ中");
                //通常のプレイ中
                break;
            case GameState.Run:
                //Debug.Log("逃げ中");
                //逃げ中。このとき会話とかできなくしたい
                break;
            case GameState.Pause:
                //Debug.Log("ポーズ中");
                //イベント中。いろいろ動かない
                break;
            case GameState.GameOver:
                Debug.Log("ゲームオーバー");
                //ゲームオーバーシーンをロード
                break;
        }
        //ゲームステート切り替え
        if (flowchart.GetBooleanVariable("event") == true)
        {
            gameState = GameState.Pause;
        }
        else if (gameState == GameState.Pause)
        {
            //敵がいなかったらplaying
            gameState = GameState.Playing;
        }


        if (InputManager.inputType == InputType.Back)
        {
            MenuPanelButton();
        }


        //----------------------体力処理---------------------------
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
                hp1.gameObject.SetActive(false);
                hp2.gameObject.SetActive(false);
                hp3.gameObject.SetActive(false);

                StartCoroutine(GameOver());
                
            }
        }

    }

    

    //-----------------------ゲームオーバーメソッド--------------------
    IEnumerator GameOver()
    {
        if (gameState == GameState.GameOver) yield break;
        //時間ゆっくりにしたりしたい
        
        float unscaledTime = 0f;
        while (true)
        {
            unscaledTime += Time.unscaledDeltaTime;
            
            Time.timeScale = Mathf.Max(1f - unscaledTime, 0f);
            yield return null;
            if (unscaledTime >= 1.0f) break;
        }
        Time.timeScale = 1f;
        gameState = GameState.GameOver;
        SceneManager.LoadScene("GameOver");
        
    }
    //-------------------セーブ画面を閉じる--------------------------
    public void CloseSavePanel()
    {
        //saveDatasPanel.SetActive(false);
        Time.timeScale = 1;
    }
    //-------------------メニューのオンオフ切り替え--------------------
    public void MenuPanelButton()
    {
        if (menuPanel == null) return;
        if (gameState == GameState.Pause) return;//会話中はメニューを開けない
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

    //-----------------選択肢表示---------------------
    public void ShowChoices()
    {
        choicesPanel.SetActive(true);
    }

    //ロードした時に時間差で敵を出現させる
    IEnumerator AppearEnemy()
    {
        float time = 0f;
        while (true)
        {
            yield return null;
            if (gameState == GameState.Pause) yield break;
            else time += Time.deltaTime;
            if (time > Data.timeWaitEnemy) break;
        }
        Vector2 appearPos = new Vector2(Data.loadPosX, Data.loadPosY);
        Instantiate(enemy, appearPos, Quaternion.identity);
    }
}
