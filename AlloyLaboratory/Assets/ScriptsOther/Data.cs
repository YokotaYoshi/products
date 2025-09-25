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
    public static string[][] itemData;

    public static string[] itemSmaho = { "スマホ", "命の次に大事" };
    public static string[] itemWatch = { "腕時計", "最近はスマホで時間を確認するから使う機会がない" };
    public static string[] itemExtinguisher = { "消火器", "Fire Extinguisher" };
    public static string[] itemCamera = { "カメラ", "これでタイムトラベルの証拠を撮るのだ" };
    public static string[][] itemDataAll;


    //----------------------キャラクター関係のデータ----------------------
    public static int charas = 1;

    public static string[][] charaData;
    public static string[] charaRino = { "リノ", "charaImageRino1" };
    public static string[] charaKurumi = { "クルミ", "charaImageMikoru1" };
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
    public static bool dashWhilePush;

    //public static GameObject itemButton0;

    static Data()
    {
        itemData = new string[][] { null, null, null, null, null, null };

        itemDataAll = new string[][] { itemSmaho, itemWatch, itemExtinguisher, itemCamera };

        charaData = new string[][] { charaRino, charaNull, charaNull };

        charaDataAll = new string[][] { charaRino, charaKurumi };

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
        charaImage0 = Resources.Load<Sprite>(charaData[0][1]);
        charaImage1 = Resources.Load<Sprite>(charaData[1][1]);
        charaImage2 = Resources.Load<Sprite>(charaData[2][1]);
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
   
