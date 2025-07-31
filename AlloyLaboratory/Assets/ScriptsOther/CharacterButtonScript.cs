using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterButtonScript : MonoBehaviour
{
    public string key;
    GameObject inputPanel;
    InputPanelManager inputPanelManager;
    int textLength;
    string[] text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputPanelManager = GetComponentInParent<InputPanelManager>();
        text = new string[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InputKey()
    {
        textLength = inputPanelManager.text.Length;
        if (textLength == 0)
        {
            inputPanelManager.text = new string[1];
            inputPanelManager.text[0] = key;
            //Debug.Log(key);
            //Debug.Log(inputPanelManager.text[0]);
        }
        else
        {
            text = new string[textLength];
            for (int i = 0; i < textLength; i++)
            {
                text[i] = inputPanelManager.text[i];
            }
            inputPanelManager.text = new string[textLength + 1];
            for (int i = 0; i < textLength; i++)
            {
                inputPanelManager.text[i] = text[i];
            }
            inputPanelManager.text[textLength] = key;
        }
        
    }
}
