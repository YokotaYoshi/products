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
    GameObject player;
    PlayerController playerCnt;
    GameObject playerFocus;
    PlayerFocus playerFocusCS;
    GameObject enemy;
    


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

        enemy = GameObject.FindGameObjectWithTag("Enemy");

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
            blackCurtainManager.FadeOut();
            Invoke("LoadScene", 0.3f);
        }
    }

    public void LoadScene()
    {

        Data.loadPosX = loadPosX;
        Data.loadPosY = loadPosY;
        PlayerController.startPos = startPos;

        if (enemy != null)
        {
            //敵がいたら敵からの距離に応じて移動先で敵が現れるまでの時間を計測
            Data.timeWaitEnemy = new Vector2(transform.position.x - enemy.transform.position.x, transform.position.y - enemy.transform.position.y).magnitude / 3f + 0.1f;
        }
        
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneFromFungus(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Data.loadPosX = loadPosX;
        Data.loadPosY = loadPosY;
    }
}

