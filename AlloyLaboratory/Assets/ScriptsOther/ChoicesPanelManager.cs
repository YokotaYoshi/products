using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Fungus;

public class ChoicesPanelManager : MonoBehaviour
{
    RectTransform rectTransform;
    public Flowchart flowchart;
    public static int choicesNum;
    public GameObject choice0;
    public GameObject choice1;
    public GameObject choice2;
    public GameObject choice3;
    public string blockName;
    public static string[] blockNames;
    public static GameObject[] choices;
    Color focusColor;
    Color unfocusColor;
    int buttonNum = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        focusColor = new Color(0.7f, 0.7f, 1f);//フォーカスされたボタンの色
        unfocusColor = new Color(1f, 1f, 1f);//その他のボタンの色
        choices = new GameObject[] { choice0, choice1, choice2, choice3 };
        blockNames = new string[] { "", "", "", "" };
    }

    // Update is called once per frame
    void Update()
    {
        //選択肢の数に応じてパネルのサイズ変更
        if (choicesNum == 2)
        {
            rectTransform.sizeDelta = new Vector2(320, 160);
            rectTransform.anchoredPosition = new Vector2(-160, -100);
        }
        else if (choicesNum == 3)
        {
            rectTransform.sizeDelta = new Vector2(320, 230);
            rectTransform.anchoredPosition = new Vector2(-160, -65);
        }
        else if (choicesNum == 4)
        {
            rectTransform.sizeDelta = new Vector2(320, 300);
            rectTransform.anchoredPosition = new Vector2(-160, -30);
        }
        
        //選択肢の数だけボタン表示
        for (int i = 0; i < 4; i++)
        {
            if (i < choicesNum) choices[i].SetActive(true);
            else choices[i].SetActive(false);
        }

        //上下でカーソル移動
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) buttonNum -= 1;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) buttonNum += 1;
        if (buttonNum >= choicesNum) buttonNum = 0;
        if (buttonNum <= -1) buttonNum = choicesNum - 1;

        //カーソルあっているボタンの色変え
        for (int i = 0; i < 4; i++)
        {
            if (i == buttonNum) choices[i].GetComponent<Image>().color = focusColor;
            else choices[i].GetComponent<Image>().color = unfocusColor;
        }


        if (InputManager.inputType == InputType.Action && !Input.GetMouseButton(0))
        {
            SetChoiceNum(buttonNum);
        }
    }

    public void SetChoiceNum(int num)
    {
        //ボタンを押したときの処理
        flowchart.ExecuteBlock(blockNames[num]);
        gameObject.SetActive(false);
    }
}
