using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject saveDatasPanel;
    public GameObject optionPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveDatasPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openSaveDatasPanel()
    {
        saveDatasPanel.SetActive(true);
    }

    public void closeSaveDatasPanel()
    {
        saveDatasPanel.SetActive(false);
    }

    public void openOptionPanel()
    {
        optionPanel.SetActive(true);
    }

    public void closeOptionPanel()
    {
        optionPanel.SetActive(false);
    }
}
