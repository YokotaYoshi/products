using UnityEngine;

public class DataEdit : MonoBehaviour
{
    //なんのデータをいじる？
    public DataType dataType;
    public CharaName charaName;
    public ItemName itemName;

    bool isEditable = false;

    //public bool executeOnStart = false;//シーン読み込み時点でデータをいじるか

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isEditable && Input.GetKeyDown(KeyCode.Return))
        {
            EditData();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerFocus")
        {
            //Dataクラスの数字をいじれる。
            isEditable = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerFocus")
        {
            //Dataクラスの数字をいじれる。
            isEditable = false;
        }
    }

    public void EditData()
    {
        if (dataType == DataType.Member)
        {
            Data.MemberAdd(charaName);
        }
        else if (dataType == DataType.Item)
        {
            Data.ItemAdd(itemName);
        }
    }

    public void SubData()
    {
        if (dataType == DataType.Member)
        {
            Data.MemberSub(charaName);
        }
    }

    public void EditGameState()
    {
        GameManager.gameState = GameState.Run;
    }
}
