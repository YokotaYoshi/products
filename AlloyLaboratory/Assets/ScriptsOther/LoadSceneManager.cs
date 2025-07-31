using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    GameObject blackCurtain;
    BlackCurtainManager blackCurtainManager;
    public string sceneName;//移動先のシーン名
    public bool eventOnStart;//移動先でイベントから入るかどうか
    public bool isEventOnly;//ここがイベントだけのシーンかどうか
    GameObject playerFocus;
    PlayerFocus playerFocusCS;

    string[] texts;//移動先のイベントのテキスト
    string[] people;//移動先のイベントの話し手

    public int textNum;//テキストの個数
    [SerializeField]
    string person0;
    [SerializeField, Multiline(2)]
    string text0;
    [SerializeField]
    string person1;
    [SerializeField, Multiline(2)]
    string text1;
    [SerializeField]
    string person2;
    [SerializeField, Multiline(2)]
    string text2;
    [SerializeField]
    string person3;
    [SerializeField, Multiline(2)]
    string text3;
    [SerializeField]
    string person4;
    [SerializeField, Multiline(2)]
    string text4;
    [SerializeField]
    public string person5;
    [SerializeField, Multiline(2)]
    string text5;
    [SerializeField]
    public string person6;
    [SerializeField, Multiline(2)]
    string text6;
    
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
        

        //texts配列、people配列を初期化
        texts = new string[textNum];
        people = new string[textNum];
        //texts配列にtextNumの個数分textを代入
        string[] maxTexts = {text0, text1, text2, text3, text4, text5, text6};
        for (int i = 0; i < textNum; i++)
        {
            texts[i] = maxTexts[i];
        }
        //people配列にtextNumの個数分personを代入
        string[] maxPeople = {person0, person1, person2, person3, person4, person5, person6};
        for (int i = 0; i < textNum; i++)
        {
            people[i] = maxPeople[i];
        }

        if (isEventOnly)
        {
            playerFocusCS.textNum = textNum;
            playerFocusCS.texts =  new string[textNum];
            playerFocusCS.people =  new string[textNum];
            for (int i = 0; i < textNum; i++)
            {
                
                //Debug.Log(texts[i]);
                
                playerFocusCS.texts[i] = texts[i];
            }
            for (int i = 0; i < textNum; i++)
            {
                playerFocusCS.people[i] = people[i];
            }
            playerFocusCS.textsProtect = true;
            PlayerFocus.eventOnStart = true;
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
            if (eventOnStart)
            {
                playerFocusCS.textNum = textNum;
                playerFocusCS.texts =  new string[textNum];
                playerFocusCS.people =  new string[textNum];
                for (int i = 0; i < textNum; i++)
                {
                    
                    //Debug.Log(texts[i]);
                    
                    playerFocusCS.texts[i] = texts[i];
                }
                for (int i = 0; i < textNum; i++)
                {
                    playerFocusCS.people[i] = people[i];
                }
                playerFocusCS.textsProtect = true;
            }
            else
            {
                playerFocusCS.textNum = 0;
                playerFocusCS.texts =  null;
                playerFocusCS.people =  null;
            }
            
            StartCoroutine(blackCurtainManager.FadeIn());
            Invoke("LoadScene", 1f);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}

