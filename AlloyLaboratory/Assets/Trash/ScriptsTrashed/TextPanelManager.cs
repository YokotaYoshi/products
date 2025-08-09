using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class TextPanelManager : MonoBehaviour
{
    //テキストの管理はなるべくこっちでやりたい
    public bool isEventOnly = false;//イベントだけのシーンかどうか
    public bool isTextLoop = true;//テキストを　ループさせるかどうか
    int chatNum = 0;//会話の何番目のテキストか
    //float textOrderingTime;//テキストを表示させる時間
    public bool isTextDisplaying = false;//テキスト表示中かどうか
    bool isTextComposing = false;//テキストが生成中かどうか

    public bool isChoicesAppearing = false;//選択肢が表示されているかどうか
    public int choiceChatNum;//選択肢を表示する番号
    int choiceNum;//選択肢の個数

    GameObject player;//プレイヤー
    PlayerController playerCnt;//プレイヤーコントローラー
    GameObject playerFocus;//プレイヤーの目線
    PlayerFocus playerFocusCS;//PlayerFocusスクリプト

    public GameObject choicePanel;//選択肢のパネル
    ChoicePanelManager choicePanelManager;//選択肢のスクリプト
    public Image speakerIcon;//話者アイコン
    public Image textBox;//テキストボックス
    public GameObject nameText;//名前
    public GameObject chatText;//文章


    //テキスト
    string[] choices;
    string[] people;
    string[] texts;
    bool[] willPopUp;
    public int textNum;
    [SerializeField]
    string person0;
    [SerializeField]
    string text0;
    [SerializeField]
    bool willPopUp0;
    [SerializeField]
    string person1;
    [SerializeField]
    string text1;
    [SerializeField]
    bool willPopUp1;
    [SerializeField]
    string person2;
    [SerializeField]
    string text2;
    [SerializeField]
    bool willPopUp2;
    [SerializeField]
    string person3;
    [SerializeField]
    string text3;
    [SerializeField]
    bool willPopUp3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーを取得
        if (player != null)
        {
            playerCnt = player.GetComponent<PlayerController>();//プレイヤーコントローラーを取得
        }
        
        playerFocus = GameObject.FindGameObjectWithTag("PlayerFocus");//プレイヤーの目線を取得
        if (playerFocus != null)
        {
            playerFocusCS = playerFocus.GetComponent<PlayerFocus>();//PlayerFocusスクリプト取得
        }

        if (choicePanel != null)
        {
            choicePanelManager = choicePanel.GetComponent<ChoicePanelManager>();
        }
        
        
        if (isEventOnly)
        {

            isTextDisplaying = true;
            people = new string[textNum];
            texts = new string[textNum];
            willPopUp = new bool[textNum];

            string[] maxPeople = { person0, person1, person2, person3 };
            string[] maxTexts = { text0, text1, text2, text3 };
            bool[] maxWillPopUp = { willPopUp0, willPopUp1, willPopUp2, willPopUp3 };

            for (int i = 0; i < textNum; i++)
            {
                people[i] = maxPeople[i];
                texts[i] = maxTexts[i];
                willPopUp[i] = maxWillPopUp[i];
            }

            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //---------------------------会話イベント-------------------------------

        //Debug.Log(chatNum);

        
        if (isEventOnly)
        {
            //イベントだけのばあい
            if (textNum > chatNum)
            {
                nameText.GetComponent<Text>().text = people[chatNum];
                if (!isTextComposing)
                {
                    if (willPopUp[chatNum])
                    {
                        //ポップアップさせるテキストなら
                        StartCoroutine(TextPopUp(texts[chatNum]));
                    }
                    else
                    {
                        StartCoroutine(TextFlow(texts[chatNum]));
                    }
                }
            }
        }
        else
        {
            //ゲーム内の会話シーン
            textNum = playerFocusCS.textNum;
            choiceChatNum = playerFocusCS.choiceChatNum;
            if (choiceChatNum > 0)
            {
                choices = new string[choiceChatNum];
                for (int i = 0; i < choiceChatNum; i++)
                {
                    choices[i] = playerFocusCS.choices[i];
                }
            }
            
            if (chatNum < playerFocusCS.textNum)
                {
                    if (!isTextComposing)
                    {
                        nameText.GetComponent<Text>().text = playerFocusCS.people[chatNum];
                        StartCoroutine(TextFlow(playerFocusCS.texts[chatNum]));
                    }
                }
        }
        
    }

    //-----------------------テキスト送りコルーチン-------------------
    IEnumerator TextFlow(string text)
    {
        //0.02sにつき1文字ずつ表示させたい
        //そのうえでEnterでスキップ、一瞬で表示
        //全部表示させてからEnterでテキスト非表示

        isTextComposing = true;//テキスト生成途中
        float textOrderingTime = 0f;

        chatText.GetComponent<Text>().text = text.Substring(0);

        while (true)
        {
            //テキストを生成する

            textOrderingTime += Time.unscaledDeltaTime;
            //Debug.Log(textOrderingTime);
            
            for (int i = 0; i <= text.Length; i++)
            {
                if (textOrderingTime > 0.05f * (float)i)
                {
                    chatText.GetComponent<Text>().text = text.Substring(0, i);
                }
            }
            
            yield return null;

            if (textOrderingTime >= (float)text.Length*0.05f)
            {
                break;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                break;
            }
        }

        chatText.GetComponent<Text>().text = text;
        //テキストの表示が完了する

        //選択肢を表示する
        if (chatNum == choiceChatNum)
        {
            isChoicesAppearing = true;
            choicePanel.SetActive(true);//選択肢表示
            choicePanelManager.choiceNum = playerFocusCS.choiceNum;
        }

        while (true)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (isChoicesAppearing)
                {
                    yield return null;//選択肢表示中なら、テキストを消さない
                }
                else
                {
                    //次のテキストを生成する準備
                    chatNum += 1;
                    isTextComposing = false;
                    if (chatNum >= textNum)
                    {
                        //今のが最後のテキストだったら破壊
                        if (isTextLoop)
                        {
                            //次もテキストを表示する
                            chatNum = 0;
                            Time.timeScale = 1;
                            isTextDisplaying = false;
                            if (playerFocus != null)
                            {
                                GameManager.eventProgress += playerFocusCS.eventProgressGetPoint;//ものによってはイベント進行
                            }
                            gameObject.SetActive(false);
                        }
                        else
                        {
                            chatNum += 1;
                            isTextDisplaying = false;
                        }
                        /*
                        Time.timeScale = 1;
                        isTextDisplaying = false;
                        if (playerFocus != null)
                        {
                            GameManager.eventProgress += playerFocusCS.eventProgressGetPoint;//ものによってはイベント進行
                        }
                        gameObject.SetActive(false);
                        */
                    }
                    yield break;
                }
            }
        }
    }

    //-----------------------------テキストポップアップコルーチン-------------------
    IEnumerator TextPopUp(string text)
    {
        isTextComposing = true;
        float textPoppingUpTime = 0f;
        
        chatText.GetComponent<Text>().text = text;//テキスト表示
        Vector2 textStartPosition = chatText.transform.position;//テキストの初期位置

        //ポジションに代入するようの値
        float vivX = 0f;
        float vivY = 0f;

        //文字の大きさの倍率
        float scale = 1f;

        while (true)
        {
            textPoppingUpTime += Time.unscaledDeltaTime;

            //表示したテキストを揺らしたい sin
            //少しずつ減衰させる exp
            //一瞬だけ文字を大きくしたり 

            vivY = Mathf.Sin(15f * textPoppingUpTime * Mathf.PI) * Mathf.Exp(-8f * textPoppingUpTime);
            vivX = Mathf.Sin(5f * textPoppingUpTime * Mathf.PI) * Mathf.Exp(-4f * textPoppingUpTime);

            scale = 2f - Mathf.Exp(textPoppingUpTime - 1f);

            chatText.transform.localScale = new Vector2(1f, 1f);
            chatText.transform.position = new Vector2(0f, textStartPosition.y + vivY);

            yield return null;

            if (textPoppingUpTime >= 1f)
            {
                //文字を静止させる
                chatText.transform.localScale = new Vector2(1f, 1f);
                chatText.transform.position = textStartPosition;
                break;
            }
        }

        while (true)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                //次のテキストを生成する準備
                chatNum += 1;
                isTextComposing = false;
                if (chatNum >= textNum)
                {
                    //今のが最後のテキストだったら破壊
                    if (isTextLoop)
                    {
                        //次もテキストを表示する
                        chatNum = 0;
                    }
                    else
                    {
                        chatNum += 1;
                    }
                    Time.timeScale = 1;
                    isTextDisplaying = false;
                    if (playerFocus != null)
                    {
                        GameManager.eventProgress += playerFocusCS.eventProgressGetPoint;//ものによってはイベント進行
                    }
                    gameObject.SetActive(false);
                }
                yield break;
            }
        }
    }
}
