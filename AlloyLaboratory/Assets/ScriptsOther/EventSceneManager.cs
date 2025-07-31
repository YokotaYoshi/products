using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EventSceneManager : MonoBehaviour
{
    public bool isFastLoad;
    //イベント専用シーン
    public GameObject textPanel;
    TextPanelManager textPanelManager;
    public GameObject blackCurtain;
    BlackCurtainManager blackCurtainManager;
    bool isTextDisplaying = true;
    bool isLoading = false;
    public string nextScene;//次のシーン

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textPanelManager = textPanel.GetComponent<TextPanelManager>();
        blackCurtainManager = blackCurtain.GetComponent<BlackCurtainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            textPanel.SetActive(true);
        }

        if (isTextDisplaying)
        {
            isTextDisplaying = textPanelManager.isTextDisplaying;
        }
        if (!isTextDisplaying && !isLoading)
        {
            StartCoroutine(LoadSceneReady());
        }
    }

    IEnumerator LoadSceneReady()
    {
        isLoading = true;

        if (!isFastLoad)
        {
            StartCoroutine(blackCurtainManager.FadeIn());
        }
        
        float time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            yield return null;
            if (isFastLoad)
            {
                break;
            }
            if (time >= 1f)
            {
                break;
            }
        }
        SceneManager.LoadScene(nextScene);
    }
}
