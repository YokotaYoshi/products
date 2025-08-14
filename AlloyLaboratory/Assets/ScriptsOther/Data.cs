using UnityEngine;

public static class Data
{
    //ここに所持アイテムを保存して、このスクリプトを参照する
    public static string[] itemData;
    public static string item0 = "a";
    public static string item1 = "b";
    public static string item2 = "c";
    public static string item3 = "d";
    
    public static string[] charaData;
    public static string chara0 = "charaImage1";
    public static string chara1 = "charaImage_m_1";
    public static string chara2 = "machu";
    public static Sprite charaImage0;
    public static Sprite charaImage1;
    public static Sprite charaImage2;

    static Data()
    {
        itemData = new string[] { item0, item1, item2, item3 };
        charaData = new string[] { chara0, chara1, chara2};
    }

    public static void LoadSprites()
    {
        charaImage0 = Resources.Load<Sprite>(charaData[0]);
        charaImage1 = Resources.Load<Sprite>(charaData[1]);
        charaImage2 = Resources.Load<Sprite>(charaData[2]);
    }
}
