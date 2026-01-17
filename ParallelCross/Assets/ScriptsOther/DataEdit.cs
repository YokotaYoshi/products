using UnityEngine;

public class DataEdit : MonoBehaviour
{
    //オブジェクトにアタッチしてFungusから呼び出す
    //なんのデータをいじる？
    public DataType dataType;
    public CharaName charaName;
    public ItemName itemName;

    //bool isEditable = false;

    //public bool executeOnStart = false;//シーン読み込み時点でデータをいじるか

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
        //Debug.Log("キャラとおわかれ");
        if (dataType == DataType.Member)
        {
            Data.MemberSub(charaName);
        }
        else if (dataType == DataType.Item)
        {
            Data.ItemSub(itemName);
        }
    }

    public void DataAdd()
    {
        //何かしらのデータを追加
        if (dataType == DataType.Member)
        {
            Data.MemberAdd(charaName);
        }
        else if (dataType == DataType.Item)
        {
            Data.ItemAdd(itemName);
        }
    }
    public void DataSub()
    {
        //何かしらデータを削除
        if (dataType == DataType.Member)
        {
            Data.MemberSub(charaName);
        }
        else if (dataType == DataType.Item)
        {
            Data.ItemSub(itemName);
        }
    }

    public void EditGameState()
    {
        GameManager.gameState = GameState.Run;
    }
}
