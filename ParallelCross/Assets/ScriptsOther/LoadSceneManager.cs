using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    GameObject blackCurtain;
    BlackCurtainManager blackCurtainManager;
    public string sceneName;//移動先のシーン名
    public float loadPosX;//ロード先のX座標
    public float loadPosY;//ロード先のY座標
    public Direction startPos;//ロード先の追従者
    public float loadTime = 0.5f;//ロードするまでの時間。頻繁にロードする場面では短めに設定するべし
    GameObject player;
    PlayerController playerCnt;
    GameObject playerFocus;
    PlayerFocus playerFocusCS;
    


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCnt = player.GetComponent<PlayerController>();
        }
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

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(blackCurtainManager.FadeOut());
            Invoke("LoadScene", loadTime);
        }
    }

    public void LoadScene()
    {

        Data.loadPosX = loadPosX;
        Data.loadPosY = loadPosY;
        PlayerController.startPos = startPos;
        SceneManager.LoadScene(sceneName);
    }
    
    public void LoadSceneEnd()
    {
        //最後の逃げ切りシーン
        //難易度に応じて遷移先を変更
        //normal,easyは通常エンド
        //hard, veryhardは真エンド
        if (Data.difficulty == Difficulty.Easy || Data.difficulty == Difficulty.Normal)
        {
            SceneManager.LoadScene("EndNormal0");
        }
        else if(Data.difficulty == Difficulty.Hard || Data.difficulty == Difficulty.VeryHard)
        {
            SceneManager.LoadScene("EndTrue0");
        }
        
    }
}

