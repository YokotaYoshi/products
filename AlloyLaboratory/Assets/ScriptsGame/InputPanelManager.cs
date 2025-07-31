using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InputPanelManager : MonoBehaviour
{
    public string[] text;
    public GameObject textDisplay;
    Text textDis;
    string showText = "";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = new string[0];
        textDis = textDisplay.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (showText.Length == text.Length)
        {
            return;
        }
        else if (showText.Length < text.Length)
        {
            showText = null;
            for (int i = 0; i < text.Length; i++)
            {
                showText += text[i];
            }
            textDis.text = showText;
        }
        
    }
}
