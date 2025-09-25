using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject saveDatasPanel;
    public GameObject optionPanel;

    public GameObject startButton;
    public GameObject continueButton;
    public GameObject optionButton;
    GameObject[] buttons;
    GameObject buttonFocused;

    Color focusColor;
    Color unfocusColor;
    
    public int buttonNum = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveDatasPanel.SetActive(false);
        optionPanel.SetActive(false);

        buttonFocused = startButton;

        focusColor = new Color(0.7f, 0.7f, 1f);//フォーカスされたボタンの色
        unfocusColor = new Color(1f, 1f, 1f);//その他のボタンの色

        buttons = new GameObject[] { startButton, continueButton, optionButton };
    }

    // Update is called once per frame
    void Update()
    {
        SwitchButtonNum();

        if (InputManager.inputType == InputType.Action)
        {
            if (buttonNum == 0) GameStart();
            else if (buttonNum == 1) OpenSaveDatasPanel();
            else if (buttonNum == 2) OpenOptionPanel();
        }
        else if (InputManager.inputType == InputType.Back)
        {
            if (buttonNum == 1) CloseSaveDatasPanel();
            else if (buttonNum == 2) CloseOptionPanel();
        }
    }

    void SwitchButtonNum()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) buttonNum -= 1;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) buttonNum += 1;
        if (buttonNum >= 3) buttonNum = 0;
        if (buttonNum <= -1) buttonNum = 2;

        buttonFocused = buttons[buttonNum];
        for (int i = 0; i < 3; ++i)
        {
            buttons[i].GetComponent<Image>().color = unfocusColor;
        }
        buttonFocused.GetComponent<Image>().color = focusColor;
    }

    public void GameStart()
    {
        Data.eventProgressMain = 0;
        Data.eventProgressSub = 0;
        PlayerController.startPos = Direction.N;
        SceneManager.LoadScene("HomeStart");
    }


    public void OpenSaveDatasPanel()
    {
        saveDatasPanel.SetActive(true);
    }

    public void CloseSaveDatasPanel()
    {
        saveDatasPanel.SetActive(false);
    }

    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }
}
