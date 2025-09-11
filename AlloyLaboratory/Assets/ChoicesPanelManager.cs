using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Fungus;

public class ChoicesPanelManager : MonoBehaviour
{
    public Flowchart flowchart;
    public GameObject choice0;
    public GameObject choice1;
    public GameObject choice2;
    public GameObject choice3;
    public string blockName;
    GameObject[] choices;
    Color focusColor;
    Color unfocusColor;
    int buttonNum = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        focusColor = new Color(0.7f, 0.7f, 1f);//フォーカスされたボタンの色
        unfocusColor = new Color(1f, 1f, 1f);//その他のボタンの色
        choices = new GameObject[] { choice0, choice1, choice2, choice3 };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) buttonNum -= 1;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) buttonNum += 1;

        if (buttonNum >= 4) buttonNum = 0;
        if (buttonNum <= -1) buttonNum = 3;

        for (int i = 0; i < 4; i++)
        {
            if (i == buttonNum) choices[i].GetComponent<Image>().color = focusColor;
            else choices[i].GetComponent<Image>().color = unfocusColor;
        }

        blockName = flowchart.GetStringVariable("blockName");
    }

    public void SetChoiceNum(int num)
    {
        //ボタンを押したときの処理
        flowchart.SetIntegerVariable("choiceNum", num);
        flowchart.ExecuteBlock(blockName);
        gameObject.SetActive(false);
    }
}
