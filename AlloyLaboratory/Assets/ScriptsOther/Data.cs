using UnityEngine;

public class Data : MonoBehaviour
{
    public static string[] itemData;
    string item0 = "a";
    string item1 = "b";
    string item2 = "c";
    string item3 = "d";
    public static string[] charaData;
    public string chara0 = "A";
    public string chara1 = "B";
    public string chara2 = "C";
    public string chara3 = "D";

    //ここに所持アイテムを保存して、このスクリプトを参照する

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string[] itemData = { item0, item1, item2, item3 }; 
        string[] charaData = {chara0, chara1, chara2, chara3};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
