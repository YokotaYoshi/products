using UnityEngine;

public enum DataType
{
    Member,
    Item,
}

public enum ItemName
{
    Smaho,
    Watch,
}

public enum CharaName
{
    Rino,
    Mikoru,
}

public static class Data
{
    //ここに所持アイテムを保存して、このスクリプトを参照する

    //----------------------アイテム関係のデータ-------------------------
    //public ItemName itemName;

    public static int items = 0;
    public static string[][] itemData;

    public static string[] itemSmaho = { "スマホ", "命の次に大事" };
    public static string[] itemWatch = { "腕時計", "最近はスマホで時間を確認するから使う機会がない" };
    public static string[][] itemDataAll;


    //----------------------キャラクター関係のデータ----------------------
    public static int charas = 1;

    public static string[][] charaData;
    public static string chara0 = "charaImageRino1";
    public static string chara1 = "charaImageMikoru1";
    public static string chara2 = "machu";
    public static string[] charaRino = { "リノ", "charaImageRino1" };
    public static string[] charaMikoru = { "ミコル", "charaImageMikoru1" };
    public static string[] charaNull = { null, null };
    public static string[][] charaDataAll;
    public static Sprite charaImage0;
    public static Sprite charaImage1;
    public static Sprite charaImage2;
    //public static GameObject itemButton0;

    static Data()
    {
        itemData = new string[][] { null, null, null, null, null, null };

        itemDataAll = new string[][] { itemSmaho, itemWatch };

        charaData = new string[][] { charaRino, charaNull, charaNull};

        charaDataAll = new string[][] { charaRino, charaMikoru };

    }

    //---------------------アイテム関係の関数--------------------------
    public static void ItemAdd(ItemName itemName)
    {
        //新しく取得したアイテムは配列の先頭に並べたい

        for (int i = 0; i < items; ++i)
        {
            if (itemDataAll[(int)itemName] == itemData[i]) return;//アイテムの重複は許さない
        }
        for (int i = items; i > 0; --i)
        {
            //すべてのアイテムを一つ後ろにずらす
            itemData[i] = itemData[i - 1];
        }
        itemData[0] = itemDataAll[(int)itemName];
        items += 1;
    }

    public static void ItemSub(int i)
    {
        //並べなおしたい
    }

    public static void LoadItem()
    {

    }

    //----------------------キャラクター関係------------------------

    public static void MemberAdd(CharaName charaName)
    {
        //charaDataの最後にcharaを追加
        //Debug.Log(chara);
        for (int i = 0; i < charas; ++i)
        {
            //重複は許さない
            if (charaDataAll[(int)charaName] == charaData[i]) return;
        }
        charaData[charas] = charaDataAll[(int)charaName];
        charas += 1;
        LoadMember();
    }
    public static void MemberSub(int i)
    {

    }

    public static void LoadMember()
    {
        charaImage0 = Resources.Load<Sprite>(charaData[0][1]);
        charaImage1 = Resources.Load<Sprite>(charaData[1][1]);
        charaImage2 = Resources.Load<Sprite>(charaData[2][1]);
    }
}
   
