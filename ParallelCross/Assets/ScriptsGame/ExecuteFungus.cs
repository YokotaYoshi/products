using UnityEngine;
using Fungus;//これが大事
using UnityEngine.UI;

public class ExecuteFungus : MonoBehaviour
{
    //Fungusを起動する
    public Flowchart flowchart;//InspectorからFlowchartを割り当てる

    public string blockName;//実行したいブロック名
    public bool executeOnClick = true;//決定で実行するか、触れただけで実行するか

    //-------------選択肢-----------------

    public string[] choices;
    public string addChoice;

    //----------------謎解き----------------
    public string question;
    public string ans;
    public Sprite sprite;
    public ItemName reward;
    public int eventProgressMainSet;
    public int eventProgressSubSet;
    bool isExecutable = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isExecutable);
        
        if (isExecutable && InputManager.inputType == InputType.Action)
        {
            //すでにイベントに入っていたら入れなくしたい
            if (GameManager.gameState == GameState.Pause) return;

            //フローチャート呼び出し
            flowchart.SetBooleanVariable("event", true);
            flowchart.ExecuteBlock(blockName);//引数はblockの名前

            //選択肢がある場合
            if (choices.Length >= 1)
            {
                //選択肢スクリプトを編集
                for (int i = 0; i < choices.Length; i++)
                {
                    //ボタンの小オブジェクトのテキストを編集
                    ChoicesPanelManager.choices[i].GetComponentInChildren<Text>().text = choices[i];
                    ChoicesPanelManager.blockNames[i] = choices[i];
                }
                ChoicesPanelManager.choicesNum = choices.Length;
            }

            //謎解きで入力がある場合
            if (question != "")
            {
                SetQandA();
            }
            else if (sprite != null)
            {
                SetImage();
            }
        }
        
        
    }

    void OnTriggerEnter2D(UnityEngine.Collider2D other)//UnityEngineをつけないとFungusのと間違える
    {
        if (other.gameObject.tag == "PlayerFocus")
        {
            if (executeOnClick)
            {
                isExecutable = true;
            }
            else
            {
                flowchart.ExecuteBlock(blockName);//引数はblockの名前
            }
        }
    }

    void OnTriggerExit2D(UnityEngine.Collider2D other)//UnityEngineをつけないとFungusのと間違える
    {
        if (other.gameObject.tag == "PlayerFocus" && executeOnClick)
        {
            isExecutable = false;
        }
    }


    public void SetItem()
    {
        //flowchart.SetStringVariable("", Data.itemDataNum[0]);
    }

    public void SetChoices()
    {

    }

    public void SetQandA()
    {
        InputPanelManager.ans = ans;
        InputPanelManager.question = question;
        InputPanelManager.sprite = sprite;
        InputPanelManager.reward = reward;
        InputPanelManager.eventProgressMainSet = eventProgressMainSet;
        InputPanelManager.eventProgressSubSet = eventProgressSubSet;
    }

    public void SetImage()
    {
        GameManager.sprite = sprite;
    }

    public void AddChoice()
    {
        //Fungusから選択肢を追加する
        string[] preChoices = new string[choices.Length];
        for (int i = 0; i < preChoices.Length; ++i)
        {
            preChoices[i] = choices[i];//一時的に保存
        }
        choices = new string[choices.Length + 1];
        choices[0] = addChoice;
        for (int i = 0; i < preChoices.Length; ++i)
        {
            choices[i + 1] = preChoices[i];//新しい選択肢を先頭に持ってきて再構成
        }
    }
}
