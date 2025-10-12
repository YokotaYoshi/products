using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveData()
    {
        //fungusから起動する
        PlayerPrefs.SetString("シーン名", sceneName);
        PlayerPrefs.SetInt("eventProgressMain", Data.eventProgressMain);
        PlayerPrefs.SetInt("eventProgressSub", Data.eventProgressSub);
        PlayerPrefs.SetInt("playerLevel", Data.playerLevel);
        PlayerPrefs.SetInt("items", Data.items);
        PlayerPrefs.SetFloat("loadPosX", transform.position.x);
        PlayerPrefs.SetFloat("loadPosY", transform.position.y - 1f);
        for (int i = 0; i < Data.items; i++)
        {
            //アイテムは対応する数値を順番に記憶する
            //ロードするときはitemDataAllから順に取り出す
            PlayerPrefs.SetInt($"item{i}", Data.itemDataNum[i]);
        }
        PlayerPrefs.SetInt("charas", Data.charas);
        for (int i = 0; i < Data.charas; i++)
        {
            //アイテムは対応する数値を順番に記憶する
            //ロードするときはcharaDataAllから順に取り出す
            PlayerPrefs.SetInt($"chara{i}", Data.charaDataNum[i]);
        }
    }
}
