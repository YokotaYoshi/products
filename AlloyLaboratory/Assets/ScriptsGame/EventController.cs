using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public bool canRotate;//方向転換できるかどうか
    GameObject player;
    Vector2 playerPosition;
    SpriteRenderer spriteRenderer;
    public Sprite downImage;//下向きの画像
    public Sprite rightImage;//右向きの画像
    public Sprite upImage;//上向きの画像
    public Sprite leftImage;//左向きの画像

    //------------------------テキストイベントについて
    public bool isTextChange = false;//イベントの切り替えがあるかどうか
    public bool isSavePoint = false;//ここでセーブできるかどうか
    //---------------------------------選択肢関連----------------------------------

    public int choiceChatNum  = 0;//何個目のテキスト中に選択肢を表示するか。0ならなし
    public string[] choices;
    //---------------------------------テキスト関連------------------------------------
    
    //-----------------------イベント進行前のテキスト
    public int textNum = 6;//テキストの個数
    [SerializeField]
    string person0;
    [SerializeField, Multiline(2)]
    string text0;
    [SerializeField]
    string person1;
    [SerializeField, Multiline(2)]
    string text1;
    [SerializeField]
    string person2;
    [SerializeField, Multiline(2)]
    string text2;
    [SerializeField]
    string person3;
    [SerializeField, Multiline(2)]
    string text3;
    [SerializeField]
    string person4;
    [SerializeField, Multiline(2)]
    string text4;
    [SerializeField]
    public string person5;
    [SerializeField, Multiline(2)]
    string text5;
    //---------------------------イベント進行後のテキスト
    public int textNum1 = 6;//テキストの個数
    [SerializeField]
    string person10;
    [SerializeField, Multiline(2)]
    string text10;
    [SerializeField]
    string person11;
    [SerializeField, Multiline(2)]
    string text11;
    [SerializeField]
    string person12;
    [SerializeField, Multiline(2)]
    string text12;
    [SerializeField]
    string person13;
    [SerializeField, Multiline(2)]
    string text13;
    [SerializeField]
    string person14;
    [SerializeField, Multiline(2)]
    string text14;
    [SerializeField]
    public string person15;
    [SerializeField, Multiline(2)]
    string text15;
    public string[] people;
    public string[] texts;
    public int eventProgressJunction;//フラグ進行用のイベントポイント
    public int eventProgressGetPoint;//この会話をおこなうことでeventProgressがどれだけ変化するか
    bool eventChange = false;//テキスト切り替えのフラグ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //方向転換できるなら、プレイヤー、SpriteRendererを取得
        if (canRotate)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            spriteRenderer = this.GetComponent<SpriteRenderer>();
        }
        //配列を初期化
        texts = new string[textNum];
        people = new string[textNum];
        
        //texts配列にtextNumの個数分textを代入
        string[] maxTexts = {text0, text1, text2, text3, text4, text5};
        for (int i = 0; i < textNum; i++)
        {
            texts[i] = maxTexts[i];
        
        }
        //people配列にtextNumの個数分personを代入
        string[] maxPeople = {person0, person1, person2, person3, person4, person5};
        for (int i = 0; i < textNum; i++)
        {
            people[i] = maxPeople[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------方向転換できるもの------------------------
        if (canRotate)
        {
            //自分から見たプレイヤーの位置
            playerPosition = new Vector2(player.transform.position.x - transform.position.x, 
            player.transform.position.y - transform.position.y);

            if (playerPosition.y * 1.1f <= playerPosition.x && playerPosition.y * 1.1f <= -playerPosition.x)
            {
                spriteRenderer.sprite = downImage;//下向きの画像を代入
            }
            else if (playerPosition.y * 1.1f <= playerPosition.x && playerPosition.y * 1.1f > -playerPosition.x)
            {
                spriteRenderer.sprite = rightImage;//右向きの画像を代入
            }
            else if (playerPosition.y * 1.1f > playerPosition.x && playerPosition.y * 1.1f <= -playerPosition.x)
            {
                spriteRenderer.sprite = leftImage;//左向きの画像を代入
            }
            else 
            {
                spriteRenderer.sprite = upImage;//上向きの画像を代入
            }
        }


        /*
        if (isTextChange)
        {
            Debug.Log(eventProgressChange);
        }
        */
        //--------------------------テキストが変化するもの-----------------------
        if (isTextChange && GameManager.eventProgress >= eventProgressJunction)
        {
            eventChange = true;//イベント切り替え
            eventProgressGetPoint = 0;//これ以上は変化しない
        }
        if (eventChange)
        {
            textNum = textNum1;//テキストの個数を更新
            //texts配列、people配列を初期化
            texts = new string[textNum1];
            people = new string[textNum1];
            //イベントが進行したら
            //texts配列にtextNumの個数分textを代入
            string[] maxTexts = {text10, text11, text12, text13, text14, text15};
            for (int i = 0; i < textNum1; i++)
            {
                texts[i] = maxTexts[i];
            
            }
            //people配列にtextNumの個数分personを代入
            string[] maxPeople = {person10, person11, person12, person13, person14, person15};
            for (int i = 0; i < textNum1; i++)
            {
                people[i] = maxPeople[i];
            }
            eventChange = false;//切り替えoff
        }
    }
}
