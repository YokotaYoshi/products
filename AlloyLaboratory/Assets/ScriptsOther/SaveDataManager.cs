using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataManager : MonoBehaviour
{
    GameObject player;//プレイヤー
    PlayerController playerCnt;//プレイヤーコントローラー
    GameObject playerFocus;//プレイヤーの目線
    PlayerFocus playerFocusCS;//PlayerFocusスクリプト
    int savePoint;
    int loadPosX;
    int loadPosY;
    int loadSceneNum;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");//プレイヤーを取得
        if (player != null)
        {
            playerCnt = player.GetComponent<PlayerController>();//プレイヤーコントローラーを取得
        }
        playerFocus = GameObject.FindGameObjectWithTag("PlayerFocus");//プレイヤーの目線を取得
        if (playerFocus != null)
        {
            playerFocusCS = playerFocus.GetComponent<PlayerFocus>();//PlayerFocusスクリプト取得
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFocusCS != null)
        {
            if (playerFocusCS.isSaveReady)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    savePoint = playerFocusCS.savePoint;
                }
            }
        }
        
    }

    //セーブする
    public void SaveData1()
    {
        PlayerPrefs.SetInt("File1", savePoint);
    }

    //ロードする
    public void LoadData1()
    {
        savePoint = PlayerPrefs.GetInt("File1");
        loadSceneNum = savePoint % 10;//ロードするシーンの番号
        loadPosY = (savePoint - loadSceneNum) / 10;//ロードするシーンの中のx座標を整数値で出す
        loadPosX = (savePoint - loadSceneNum - loadPosY * 10) / 100;//ロードするシーンの中のx座標を整数値で出す
        
        
        SceneManager.LoadScene($"Scene{loadSceneNum}");
    }
}
