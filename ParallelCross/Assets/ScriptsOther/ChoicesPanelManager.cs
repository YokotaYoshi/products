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
    Color focusColor = new Color(0.7f, 0.7f, 1f);//フォーカスされたボタンの色
    Color unfocusColor = new Color(1f, 1f, 1f);//その他のボタンの色
    int buttonNum = 0;

    //パネルの位置大きさについての数値
    float panelSizeX = 320f;
    float panelPosX = 320f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        blockNames = new string[] { "", "", "", "" };
    }

    // Update is called once per frame
    void Update()
    {
        choicesNum = Data.choicesNum;
        choices = new GameObject[] { choice0, choice1, choice2, choice3 };
        for (int i = 0; i < choicesNum; ++i)
        {
            choices[i].GetComponentInChildren<Text>().text = Data.choices[i];
            blockNames[i] = Data.choices[i];
        }
        //選択肢の数に応じてパネルのサイズ変更
        if (choicesNum == 2)
        {
            rectTransform.sizeDelta = new Vector2(panelSizeX, 160);
            rectTransform.anchoredPosition = new Vector2(panelPosX, -100);
        }
        else if (choicesNum == 3)
        {
            rectTransform.sizeDelta = new Vector2(panelSizeX, 230);
            rectTransform.anchoredPosition = new Vector2(panelPosX, -65);
        }
        else if (choicesNum == 4)
        {
            rectTransform.sizeDelta = new Vector2(panelSizeX, 300);
            rectTransform.anchoredPosition = new Vector2(panelPosX, -30);
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
        Debug.Log(blockNames[num]);
        Debug.Log(num);
        if (blockNames[num] == "")
        {
            flowchart.ExecuteBlock("Button");
        }
        else
        {
            flowchart.ExecuteBlock(blockNames[num]);
        }
        
        gameObject.SetActive(false);
    }
}
