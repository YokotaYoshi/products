using UnityEngine;
using Fungus;//これが大事

public class ExecuteFungus : MonoBehaviour
{
    public Flowchart flowchart;//InspectorからFlowchartを割り当てる
    public string blockName = "NewBlock1";//実行したいブロック名
    public bool executeOnClick = true;//決定で実行するか、触れただけで実行するか

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(UnityEngine.Collider2D other)//UnityEngineをつけないとFungusのと間違える
    {
        if (other.gameObject.tag == "PlayerFocus" && executeOnClick)
        {
            //Debug.Log("イベントに入れる");
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //fungusの何かしらのブロックを起動する
                flowchart.ExecuteBlock(blockName);//引数はblockの名前

            }

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
}
