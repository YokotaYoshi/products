using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Fungus;

public class InputPanelManager : MonoBehaviour
{
    public static string question;
    public static string ans;
    public static Sprite sprite;

    public static string text = "";
    string newText = "";
    public GameObject textDisplay;
    public GameObject textQuestion;
    public GameObject hintImage;
    public Flowchart flowchart;
    public static ItemName reward;
    public static int eventProgressMainSet;
    public static int eventProgressSubSet;
    Text textDis;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textDis = textDisplay.GetComponent<Text>();

    }

    void OnEnable()
    {
        text = "";
        hintImage.GetComponent<Image>().sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        //キーボードとボタン両方に対応
        //アルファベットに対応完了
        //ひらがなに対応させることできないかな？
        textDis.text = text.ToUpper();
        //Debug.Log(text.Length);
        //if (text == ans) Debug.Log("あってる");
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("一文字削除");
            newText = "";
            for (int i = 0; i < text.Length - 1; i++)
            {
                newText = string.Concat(newText, text[i]);
            }
            text = newText;
            //text = text.Substring(0, text.Length -1)
            return;
        }
        if (InputManager.inputType == InputType.Action)
        {
            if (text.ToUpper() == ans)
            {
                question = "正解";
                Debug.Log("正解");
                Data.ItemAdd(reward);
                Invoke("Success", 1.0f);
                return;
                //eventProgressも変化させる
            }
            //Data.inputString = text;
            Invoke("Close", 0.1f);

        }
        if (text.Length >= 10) return;//とりあえず10文字まで
        text = string.Concat(text, Input.inputString);


        textQuestion.GetComponent<Text>().text = question;

    }

    void Close()
    {
        flowchart.SetBooleanVariable("event", false);
        GameManager.gameState = GameState.Playing;
        gameObject.SetActive(false);
    }

    void Success()
    {
        flowchart.SetBooleanVariable("event", false);
        flowchart.SetIntegerVariable("eventProgressMain", eventProgressMainSet);
        flowchart.SetIntegerVariable("eventProgressSub", eventProgressSubSet);
        GameManager.gameState = GameState.Playing;
        gameObject.SetActive(false);
    }
}
