using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    GameObject blackCurtain;
    BlackCurtainManager blackCurtainManager;
    public string sceneName;//移動先のシーン名
    GameObject playerFocus;
    PlayerFocus playerFocusCS;



    // Start is called before the first frame update
    void Start()
    {
        playerFocus = GameObject.FindGameObjectWithTag("PlayerFocus");
        if (playerFocus != null)
        {
            playerFocusCS = playerFocus.GetComponent<PlayerFocus>();
        }

        //暗転用のスクリプトを取得
        blackCurtain = GameObject.FindGameObjectWithTag("BlackCurtain");
        if (blackCurtain != null)
        {
            blackCurtainManager = blackCurtain.GetComponent<BlackCurtainManager>();
        }


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(eventOnStart);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(blackCurtainManager.FadeIn());
            Invoke("LoadScene", 1f);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void LoadSceneFromFungus(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

