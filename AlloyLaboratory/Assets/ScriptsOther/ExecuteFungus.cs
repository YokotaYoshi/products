using UnityEngine;
using Fungus;//これが大事

public class ExecuteFungus : MonoBehaviour
{
    public Flowchart flowchart;//InspectorからFlowchartを割り当てる

    public string blockName;//実行したいブロック名
    public bool executeOnClick = true;//決定で実行するか、触れただけで実行するか
    public bool willDelete = false;//話しかけたら消える
    //-------------選択肢-----------------
    public int choicesNum;
    public string choice0;
    public string choice1;
    public string choice2;
    public string choice3;
    public string[] choices;
    bool isExecutable = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        choices = new string[] { choice0, choice1, choice2, choice3 };
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(flowchart.GetBooleanVariable("event"));
        if (flowchart.GetBooleanVariable("event") == true)
        {
            GameManager.gameState = GameState.Pause;
        }
        else
        {
            GameManager.gameState = GameState.Playing;
        }

        if (isExecutable && InputManager.inputType == InputType.Action)
        {
            //すでにイベントに入っていたら入れなくしたい
            if (GameManager.gameState == GameState.Pause) return;
            flowchart.SetBooleanVariable("event", true);
            flowchart.ExecuteBlock(blockName);//引数はblockの名前
            if (willDelete)
            {
                Destroy(gameObject);
            }
        }

        //fungusのeventProgressをDataクラスのeventProgressと同期させる
        flowchart.SetIntegerVariable("eventProgress", Data.eventProgress);
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

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)//UnityEngineをつけないとFungusのと間違える
    {
        if (collision.gameObject.tag == "Player" && !executeOnClick)
        {
            //fungusの何かしらのブロックを起動する
            flowchart.ExecuteBlock(blockName);//引数はblockの名前
        }
    }

    public void SetItem()
    {
        flowchart.SetStringVariable("", Data.itemData[0][0]);
    }

    public void SetChoices()
    {

    }
}
