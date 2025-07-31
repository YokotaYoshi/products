using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuPanelManager : MonoBehaviour
{

    public GameObject itemPanel;
    bool onoff = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        itemPanel.SetActive(onoff);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onoff = !onoff;
        }
    }
}
