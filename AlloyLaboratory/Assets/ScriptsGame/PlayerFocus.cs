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
    
    //----------------------------会話イベント------------------------------
    public int eventProgressGetPoint = 0;//イベント進行
    EventController eventCnt;//イベントコントローラー
    public bool eventFlag = false;//会話イベントに入れる状態かどうか
    public int choiceNum;//選択肢。無ければ0
    public string[] choices;
    public int choiceChatNum;
    public int textNum;//テキスト数
    public string[] texts;//会話テキスト
    public string[] people;//会話の話者
    public bool textsProtect = false;
    //ロード中にテキストを保存
    public static int textNumStatic;
    public static string[] textsStatic;
    public static string[] peopleStatic;
    //---------------------------アイテムの取得------------------------------
    EventItemController eventItemCnt;//アイテムコントローラー
    //---------------------------セーブ関連---------------------
    public bool isSaveReady = false;
    int savePosX;
    int savePosY;
    int saveSceneNum;
    public int savePoint;
    
    //-----------------------------まだ通れないところ------------------------
    public bool isPrevented = false;
    string preventDirection;
    //------------------------------シーンの移動関係--------------------
    public static bool eventOnStart;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        //会話イベント準備
        textNum = textNumStatic;
        texts = new string[textNum];
        people = new string[textNum];

        for (int i = 0; i < textNumStatic; i++)
        {
            texts[i] = textsStatic[i];
            //Debug.Log(texts[i]);
        }
        for (int i = 0; i < textNumStatic; i++)
        {
            people[i] = peopleStatic[i];
        }
        textsProtect = false;
        //eventOnStart = false;
    }

    // Update is called once per frame
    void Update()
    {
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
        
        //------------------------------------イベントについての記述----------------------------------
        //Debug.Log(eventProgress);
        if (textsProtect)
        {
            textNumStatic = textNum;
            textsStatic = new string[textNumStatic];
            peopleStatic = new string[textNumStatic];
            for (int i = 0; i < textNum; i++)
            {
                textsStatic[i] = texts[i];
            }
            for (int i = 0; i < textNum; i++)
            {
                peopleStatic[i] = people[i];
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("イベントフラグオン");
        

        //---------------------------今は先に進めない場所-------------------------------
        if (other.gameObject.tag == "Prevent")
        {
            isPrevented = true;//まだいけないよ

            EventProtector eventProtector = other.GetComponent<EventProtector>();
            preventDirection = eventProtector.awayDirection;


            //何かしゃべる。例えば「まだやることがあった」
            //texts配列を初期化
            texts = new string[1];
            //テキスト情報を配列に収納
            texts[0] = eventProtector.text;
            //people配列を初期化
            people = new string[1];
            //テキスト情報を配列に収納
            people[0] = eventProtector.person;
            
        }

        //-----------------------------シーンの移動--------------------------------
        
        if (other.gameObject.tag == "LoadPoint")
        {
            eventOnStart = other.GetComponent<LoadSceneManager>().eventOnStart;//シーン移動先でイベントから始まるかどうか
            //Debug.Log(eventOnStart);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Event")
        {
            //プレイヤーがイベント相手を見ていたら
            //イベントに入れる状態である
            eventFlag = true;
            //対象のイベントコントローラーを取得
            eventCnt = other.GetComponent<EventController>();
            //選択肢の個数を取得
            choiceNum = eventCnt.choices.Length;
            //何個目のテキストで選択肢が表示されるか
            choiceChatNum = eventCnt.choiceChatNum;
            choices = new string[choiceNum];
            if (choiceChatNum > 0)
            {
                for (int i = 0; i < choiceNum; i++)
                {
                    choices[i] = eventCnt.choices[i];
                }
            }
            
            //テキスト数を取得
                textNum = eventCnt.textNum;
            //texts配列を初期化
            texts = new string[textNum];
            //eventProgressChangeを取得
            eventProgressGetPoint = eventCnt.eventProgressGetPoint;
            //Debug.Log(eventProgressGetPoint);
            //テキスト情報を配列に収納
            for (int i = 0; i < textNum; i++)
            {
                texts[i] = eventCnt.texts[i];
                //Debug.Log(texts[i]);
            }
            //people配列を初期化
            people = new string[textNum];
            //テキスト情報を配列に収納
            for (int i = 0; i < textNum; i++)
            {
                people[i] = eventCnt.people[i];
                //Debug.Log(texts[i]);
            }
        }
        
        

        if (other.gameObject.tag == "SavePoint")
        {
            //プレイヤーがセーブポイントを見ていたら
            //セーブができる状態である
            isSaveReady = true;

            savePosX = other.GetComponent<SavePointManager>().savePosX;
            savePosY = other.GetComponent<SavePointManager>().savePosY;
            saveSceneNum = other.GetComponent<SavePointManager>().saveSceneNum;
            savePoint = 100 * savePosX + 10 * savePosY + saveSceneNum;
        }

        if (other.gameObject.tag == "Prevent")
        {
            if (preventDirection == "down")
            {
                //下に行かせないように
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    isPrevented = true;
                }
            }
            else if (preventDirection == "right")
            {
                //右に行かせないように
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    isPrevented = true;
                }
            }
            else if (preventDirection == "up")
            {
                //上に行かせないように
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    isPrevented = true;
                }
            }
            else if (preventDirection == "left")
            {
                //左に行かせないように
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    isPrevented = true;
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Event")
        {
            //プレイヤーがイベント相手から目をそらしたら
            //イベントに入れない状態にする
            eventFlag = false;
            eventProgressGetPoint = 0;//eventProgressが動かないように
        }
        if (other.gameObject.tag == "SavePoint")
        {
            isSaveReady = false;//セーブできない状態
        }
        //取得したイベントコントローラーを捨てる
        eventCnt = null;
    }
}
