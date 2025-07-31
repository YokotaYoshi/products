using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public SpotLightController spotLightCnt;
    public PlayerController playerCnt;
    public GameObject gameOverPanel;
    public GameObject clearPanel;
    public GameObject informationPanel;
    bool isRetryable = false;
    public string sceneName;
    public string nextSceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverPanel.SetActive(false);
        clearPanel.SetActive(false);
        Invoke("CloseInformation", 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (spotLightCnt.isGameOver == true)
        {
            GameOver();
        }

        if (playerCnt.isGoal == true)
        {
            //クリア処理
            clearPanel.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }

        if (isRetryable)
        {
            //ゲームオーバー処理
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("HomeScene");
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        isRetryable = true;
    }

    void CloseInformation()
    {
        if (informationPanel != null)
        {
            informationPanel.SetActive(false);
        }
    }
}
