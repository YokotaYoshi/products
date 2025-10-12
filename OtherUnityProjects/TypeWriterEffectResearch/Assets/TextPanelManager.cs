using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum GameState
{
    start,
    playing
}
public class TextPanelManager : MonoBehaviour
{
    int chatNum = 0;//会話の何番目のテキストか
    //float textOrderingTime;//テキストを表示させる時間
    public bool isTextDisplaying = false;//テキスト表示中かどうか
    public float textDisplayingTime = 8f;//テキスト表示時間
    bool isTextComposing = true;//テキストが生成中かどうか

    public GameObject chatText;//文章
    public GameObject timeText;//時間を表示
    public float charaPerSec = 60f;
    public GameState currentState = GameState.start;


    //テキスト
    string[] texts;
    public int textNum;
    [SerializeField]
    string text0;
    [SerializeField]
    string text1;
    [SerializeField]
    string text2;
    [SerializeField]
    string text3;
    [SerializeField]

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texts = new string[textNum];
        texts[0] = text0;
        texts[1] = text1;
        texts[2] = text2;
        texts[3] = text3;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.start:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    currentState = GameState.playing;
                    isTextComposing = false;
                }
                break;
            case GameState.playing:
                if (!isTextComposing)
                {
                    Debug.Log(chatNum);
                    StartCoroutine(TextFlow(texts[chatNum]));
                }
                break;
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
                if (textOrderingTime > 1f / charaPerSec * (float)i)
                {
                    chatText.GetComponent<Text>().text = text.Substring(0, i);
                }
            }

            timeText.GetComponent<Text>().text = "Time: " + Mathf.Floor(textDisplayingTime - textOrderingTime);

            yield return null;

            if (textOrderingTime >= (float)text.Length / charaPerSec)
            {
                chatText.GetComponent<Text>().text = text;
            }

            if (textOrderingTime >= textDisplayingTime)
            {
                //時間経過で強制的に次のテキストへ
                //テキストの表示が完了する
                chatNum += 1;
                charaPerSec += 10f;
                isTextComposing = false;
                yield break;
            }
        }
    }
}
