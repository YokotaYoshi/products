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


    // Update is called once per frame
    void Update()
    {
        //キーボードとボタン両方に対応
        //アルファベットに対応
        textDis.text = text.ToUpper();
        hintImage.GetComponent<Image>().sprite = sprite;
        //Debug.Log(text.Length);
        //if (text == ans) Debug.Log("あってる");
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (text.Length > 0)
            {
                text = text.Substring(0, text.Length -1);
            }
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
        text = string.Format("{0}{1}", text, Input.inputString);

        textQuestion.GetComponent<Text>().text = question;

    }

    void Close()
    {
        text = "";
        flowchart.SetBooleanVariable("event", false);
        GameManager.gameState = GameState.Playing;
        gameObject.SetActive(false);
    }

    void Success()
    {
        flowchart.SetBooleanVariable("event", false);
        if (eventProgressMainSet != 0)
            flowchart.SetIntegerVariable("eventProgressMain", eventProgressMainSet);
        if (eventProgressSubSet != 0)
        flowchart.SetIntegerVariable("eventProgressSub", eventProgressSubSet);
        GameManager.gameState = GameState.Playing;
        gameObject.SetActive(false);
    }
}
