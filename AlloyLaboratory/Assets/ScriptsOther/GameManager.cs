using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Fungusの切り替えとかに置き換えていく
    //UIを担当
    //主にパネルのONOFFを行う
    //ONにしたら、そのあとは個別に処理する
    //float debugTime = 0;//デバッグ用のタイマー
    public string gameState = "playing";
    //------------------左端の情報パネル----------------------
    public GameObject informationPanel;//左端の情報パネル
    public Image charaIcon;//操作キャラクターのアイコン
    public Sprite chara1;//誰かのアイコン画像
    public bool isPanelOn = true;//パネルを表示するかどうか
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

        //--------------------最初表示するか、表示しないか------------------------
        if (informationPanel != null)
        {
            if (isPanelOn)
            {
                informationPanel.SetActive(true);//情報パネル表示
            }
            else
            {
                informationPanel.SetActive(false);//情報パネル非表示
            }
        }

        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーを取得
        playerCnt = player.GetComponent<PlayerController>();//プレイヤーコントローラーを取得
        playerFocus = GameObject.FindGameObjectWithTag("PlayerFocus");//プレイヤーの目線を取得
        playerFocusCS = playerFocus.GetComponent<PlayerFocus>();//PlayerFocusスクリプト取得
    }

    // Update is called once per frame
    void Update()
    {


        //----------------------------------セーブポイント-----------------------------------
        if (playerFocusCS.isSaveReady)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 0;//ゲーム停止
                
                saveDatasPanel.SetActive(true);//セーブデータ表示
            }
        }

        //------------------まだ先に進めないところに行こうとしたときに引き留める----------------------
        /*
        if (playerFocusCS.isPrevented)
        {
            Time.timeScale = 0;//ゲームストップ
            textPanel.SetActive(true);//テキストボックス表示
            ///nameText.GetComponent<Text>().text = playerFocusCS.people[0];//配列の1番目の名前を表示
            //debugTime += Time.deltaTime;

            //StartCoroutine(TextFlow(playerFocusCS.texts[0]));
            
            
            if (!isTextDisplaying && !isTextComposing)
            {
                //テキスト非表示かつ
                //テキスト生成が進んでいなかったら

                //StartCoroutine(TextFlow(playerFocusCS.texts[0]));
            }
            
            

            if(isTextDisplaying && !isTextComposing && Input.GetKeyDown(KeyCode.Return))
            {
                //テキスト表示中で
                //テキストの生成は終わった状態で
                //Enterキーを押すと
                
                Time.timeScale = 1;//ゲーム再開
                //nameText.GetComponent<Text>().text = null;//名前をなにもなしに
                //chatText.GetComponent<Text>().text = null;//テキストをなにもなしに
                textPanel.SetActive(false);//テキストボックス非表示
                isTextDisplaying = false;//テキストが非表示になった
                playerFocusCS.isPrevented = false;//動かせる状態に
            }
        }
        */

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
                gameState = "gameOver";
            }
        }

    }

    void FixedUpdate()
    {
        //Debug.Log(Time.deltaTime);
    }

    //ゲームオーバーメソッド
    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }



    //セーブ画面を閉じる
    public void CloseSavePanel()
    {
        saveDatasPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowMenuPanel()
    {
        menuPanel.SetActive(true);
    }
}
