using UnityEngine;

public enum DataType
{
    Member,
    Item,
    EventProgress,
}

public enum ItemName
{
    //アイテムを追加するときは
    //ここにアイテムの名前を追加
    //Dataクラス内にitemCameraのように追加
    //static Data()内itemDataAllにも追加
    Null,
    Photo,
    Ring,
    NewsPaper,
    Cord,
    Bat,
    Key,
    SmartPhone,
}

public enum CharaName
{
    Null,
    Rino,
    Kurumi,
    RinoF,
    KarinF,
}

public enum Difficulty
{
    Auto,
    VeryHard,
    Hard,
    Normal,
    Easy,
}

public enum GameState
{
    Start,
    Playing,
    Run,
    Pause,
    GameOver,
}



public enum Direction
{
    Up,
    Down,
    Right,
    Left,
    N,
}



public static class Data
{
    //ここに所持アイテムを保存して、このスクリプトを参照する

    //----------------------アイテム関係のデータ-------------------------
    //public ItemName itemName;

    public static int items = 1;
    public static int[] itemDataNum = new int[] {7, 0, 0, 0, 0, 0};
    public static string[] itemNull = { null, null };

    public static string[] itemPhoto = { "家族写真", "3年前に撮った。この帰りの事故で両親は亡くなった"};
    public static string[] itemRing = { "指輪", "美しい。誰のだろう" };
    public static string[] itemNewsPaper = { "新聞紙", "これを持って帰れば良いのだろうか" };
    public static string[] itemCord = { "ひも", "丈夫そう。簡単には切れないだろう" };
    public static string[] itemBat = { "バット", "放てホームラン" };
    public static string[] itemKey = { "鍵", "どこの鍵かな" };
    public static string[] itemSmartPhone = { "スマホ", "現代の必需品" };
    //public static string[] itemRing = { "腕輪", "去年の誕生日に渡したものだ" };
    public static string[][] itemDataAll;


    //----------------------キャラクター関係のデータ----------------------
    public static int charas = 1;

    public static int[] charaDataNum = {1, 0, 0};
    public static string[] charaRino = { "リノ", "charaImageRino1" };//0
    public static string[] charaKurumi = { "クルミ", "charaImageMikoru1" };//1
    public static string[] charaRinoF = { "リノ", null };
    public static string[] charaKarin = { "カリン", "machu" };
    public static string[] charaNull = { null, null };
    public static string[][] charaDataAll;
    public static Sprite charaImage0;
    public static Sprite charaImage1;
    public static Sprite charaImage2;
    //---------------ロード先の座標を保存---------------
    public static float loadPosX;
    public static float loadPosY;
    //---------------敵に追いかけられているとき-----------
    public static float timeWaitEnemy;

    //--------------イベントの進捗に関するデータ---------------
    public static int eventProgressMain;
    public static int eventProgressSub;
    public static bool onEvent;
    //----------------------入力、選択肢-------------------------
    public static string inputString;
    public static string[] choices = new string[4];
    public static int choicesNum;
    public static string choice0;
    public static string choice1;
    public static string choice2;
    public static string choice3;

    //------------------オプション関係のデータ----------------

    public static Difficulty difficulty = Difficulty.Hard;//実施の難易度
    public static Difficulty currentDifficulty = Difficulty.Auto;//設定上の難易度
    public static int playerLevel = 9;//1~12
            //playerLeverlの値はGameManager.GameOver()内で変更
            //GameManager.PlayerLevelUp()/Down()でも
    public static bool dashWhilePush = true;
    //------------------セーブデータ---------------------
    public static int[] saveDataEventProgressMain;
    public static string[] saveDataSceneName;


    //public static GameObject itemButton0;

    static Data()
    {

        itemDataAll = new string[][] {itemNull, itemPhoto, itemRing, itemNewsPaper , itemCord, itemBat, itemKey, itemSmartPhone, itemNull, itemNull};


        charaDataAll = new string[][] {charaNull, charaRino, charaKurumi, charaRinoF, charaKarin };

        //-------------------セーブデータ-----------------------
        saveDataEventProgressMain = new int[] { 0, 0 };
        saveDataSceneName = new string[] { null, null };
    }

    //---------------------アイテム関係の関数--------------------------
    public static void ItemAdd(ItemName itemName)
    {
        //新しく取得したアイテムは配列の先頭に並べたい
        
        if (itemName == ItemName.Null) return;

        for (int i = 0; i < items; ++i)
        {
            if ((int)itemName == itemDataNum[i]) return;//アイテムの重複は許さない
        }
        for (int i = items; i > 0; --i)
        {
            //すべてのアイテムを一つ後ろにずらす
            itemDataNum[i] = itemDataNum[i - 1];
        }
        itemDataNum[0] = (int)itemName;
        items += 1;
    }

    public static void ItemSub(ItemName itemName)
    {
        if (itemName == ItemName.Null) return;
        //並べなおしたい
        for (int i = 0; i < 6; ++i)
        {
            if ((int)itemName == itemDataNum[i])
            {
                //最後は空白に
                if (i == 5) itemDataNum[i] = 0;
                else
                {
                    //順番を一つ前へ
                    for (int j = i; j < 5; ++j)
                    {
                        itemDataNum[j] = itemDataNum[j + 1];
                    }
                    itemDataNum[5] = 0;
                }
                items -= 1;
            }
        }
        Debug.Log(items);
    }

    public static void LoadItem()
    {

    }

    //----------------------キャラクター関係------------------------

    public static void MemberAdd(CharaName charaName)
    {
        if (charaName == CharaName.Null) return;
        //charaDataの最後にcharaを追加
        //Debug.Log(chara);

        for (int i = 0; i < charas; ++i)
        {
            if ((int)charaName == charaDataNum[i]) return;//アイテムの重複は許さない
        }
        
        charaDataNum[charas] = (int)charaName;
        charas += 1;
        LoadMember();
    }
    public static void MemberSub(CharaName charaName)
    {
        if (charaName == CharaName.Null) return;

        for (int i = 0; i < 3; ++i)
        {
            if ((int)charaName == charaDataNum[i])
            {
                //最後は空白に
                if (i == 2) charaDataNum[i] = 0;
                else
                {
                    //順番を一つ前へ
                    for (int j = i; j < 2; ++j)
                    {
                        charaDataNum[j] = charaDataNum[j + 1];
                    }
                    
                    charaDataNum[2] = 0;
                }
                charas -= 1;
            }
        }
        Debug.Log(charas);
        LoadMember();
    }

    public static void LoadMember()
    {
        switch (charas)
        {
            case 1:
                charaImage0 = Resources.Load<Sprite>(charaDataAll[charaDataNum[0]][1]);
                charaImage1 = null;
                charaImage2 = null;
                break;
            case 2:
                charaImage0 = Resources.Load<Sprite>(charaDataAll[charaDataNum[0]][1]);
                charaImage1 = Resources.Load<Sprite>(charaDataAll[charaDataNum[1]][1]);
                charaImage2 = null;
                break;
            case 3:
                charaImage0 = Resources.Load<Sprite>(charaDataAll[charaDataNum[0]][1]);
                charaImage1 = Resources.Load<Sprite>(charaDataAll[charaDataNum[1]][1]);
                charaImage2 = Resources.Load<Sprite>(charaDataAll[charaDataNum[2]][1]);
                break;
        }
    }

   
}
   
