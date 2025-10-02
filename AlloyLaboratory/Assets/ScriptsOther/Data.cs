using UnityEngine;

public enum DataType
{
    Member,
    Item,
    EventProgress,
}

public enum ItemName
{
    Smaho,
    Watch,
    Extinguisher,
    Camera,
    Null,
}

public enum CharaName
{
    Rino,
    Kurumi,
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

    public static int items = 0;
    public static int[] itemDataNum = new int[6];

    public static string[] itemSmaho = { "スマホ", "命の次に大事", "0" };
    public static string[] itemWatch = { "腕時計", "最近はスマホで時間を確認するから使う機会がない" };
    public static string[] itemExtinguisher = { "消火器", "Fire Extinguisher" };
    public static string[] itemCamera = { "カメラ", "これでタイムトラベルの証拠を撮るのだ" };
    public static string[] itemRing = { "腕輪", "去年の誕生日に渡したものだ" };
    public static string[][] itemDataAll;


    //----------------------キャラクター関係のデータ----------------------
    public static int charas = 1;

    public static string[][] charaData;
    public static int[] charaDataNum = new int[3];
    public static string[] charaRino = { "リノ", "charaImageRino1" };//0
    public static string[] charaKurumi = { "クルミ", "charaImageMikoru1" };//1
    public static string[] charaRinoF = { "リノ", null };
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
    //----------------------入力-------------------------
    public static string inputString;
    //------------------オプション関係のデータ----------------

    public static Difficulty difficulty = Difficulty.Hard;//実施の難易度
    public static Difficulty currentDifficulty = Difficulty.Auto;//設定上の難易度
    public static int playerLevel = 0;
    public static bool dashWhilePush = true;
    //------------------セーブデータ---------------------
    public static int[] saveDataEventProgressMain;
    public static string[] saveDataSceneName;


    //public static GameObject itemButton0;

    static Data()
    {

        itemDataAll = new string[][] { itemSmaho, itemWatch, itemExtinguisher, itemCamera };

        charaData = new string[][] { charaRino, charaNull, charaNull };

        charaDataAll = new string[][] { charaRino, charaKurumi, charaRinoF, charaNull};

        //-------------------セーブデータ-----------------------
        saveDataEventProgressMain = new int[] { 0, 0 };
        saveDataSceneName = new string[] { null, null };
    }

    //---------------------アイテム関係の関数--------------------------
    public static void ItemAdd(ItemName itemName)
    {
        //新しく取得したアイテムは配列の先頭に並べたい
        /*
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
        */

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
            if ((int)charaName == charaDataNum[i]) return;//アイテムの重複は許さない
        }
        
        charaDataNum[charas] = (int)charaName;
        charas += 1;
        LoadMember();
    }
    public static void MemberSub(CharaName charaName)
    {
        for (int i = 0; i < 3; ++i)
        {
            if (charaDataAll[(int)charaName] == charaData[i])
            {
                //最後は空白に
                if (i == 2) charaData[i] = charaNull;
                else
                {
                    charaData[i] = charaData[i + 1];
                    charaData[2] = charaNull;
                }
                charas -= 1;
            }
        }
        LoadMember();
    }

    public static void LoadMember()
    {
        switch (charas)
        {
            case 1:
                charaImage0 = Resources.Load<Sprite>(charaDataAll[charaDataNum[0]][1]);
                break;
            case 2:
                charaImage0 = Resources.Load<Sprite>(charaDataAll[charaDataNum[0]][1]);
                charaImage1 = Resources.Load<Sprite>(charaDataAll[charaDataNum[1]][1]);
                break;
            case 3:
                charaImage0 = Resources.Load<Sprite>(charaDataAll[charaDataNum[0]][1]);
                charaImage1 = Resources.Load<Sprite>(charaDataAll[charaDataNum[1]][1]);
                charaImage2 = Resources.Load<Sprite>(charaDataAll[charaDataNum[2]][1]);
                break;
        }
    }

    //-----------------難易度関係------------------------
    public static void LoadDifficulty()
    {
        if (currentDifficulty == Difficulty.Auto)
        {
            //自動で難易度変更
            if (playerLevel == 0) difficulty = Difficulty.Hard;
            else if (playerLevel <= 3) difficulty = Difficulty.Normal;
            else difficulty = Difficulty.Easy;
        }
    }
}
   
