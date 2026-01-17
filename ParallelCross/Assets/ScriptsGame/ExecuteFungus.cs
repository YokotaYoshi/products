using UnityEngine;
using Fungus;//これが大事
using UnityEngine.UI;

public class ExecuteFungus : MonoBehaviour
{
    //Fungusを起動する
    public Flowchart flowchart;//InspectorからFlowchartを割り当てる

    public string blockName;//実行したいブロック名
    public string[] blockNames;//所持アイテムに応じて実行するブロックを変更
    public ItemName[] keyItems;//ブロックを変更するためのアイテム
    //複数のキーアイテムがある場合は？
    //分岐させたいブロックが3つ以上ある場合は？
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

        //所持しているアイテムに応じて呼び出すブロックを変更できるようにしたい
        if (blockNames != null && blockNames.Length >= 1)
        {
            //分岐がある場合
            for (int i = 0; i < blockNames.Length; ++i)
            {
                for (int j = 0; j < Data.items; ++j)
                {
                    //所持アイテムを探索してキーアイテムに一致するかどうか
                    if (Data.itemDataNum[j] == (int)keyItems[i])
                    {
                        blockName = blockNames[i];
                    }
                }
            }
        }
        
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
                Data.choices = new string[4];
                Data.choicesNum = choices.Length;
                //選択肢スクリプトを編集
                for (int i = 0; i < choices.Length; i++)
                {
                    //ボタンの小オブジェクトのテキストを編集
                    //Debug.Log(choices[i]);
                    //一回Dataクラスに渡す
                    
                    Data.choices[i] = choices[i];
                }
                //ChoicesPanelManager.choicesNum = choices.Length;
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
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "PlayerFocus")
        {
            if (executeOnClick)
            {
                isExecutable = true;
            }
            else
            {
                Debug.Log(blockName);
                flowchart.ExecuteBlock(blockName);//引数はblockの名前
            }
        }

        if (other.gameObject.tag == "Player" && !executeOnClick)
        {
            //flowchart.ExecuteBlock(blockName);
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
        //ImagePanelで表示するやつ
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
