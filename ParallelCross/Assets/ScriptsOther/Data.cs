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
    Camera,
    Cord,
    Bat,
    
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

    public static int items = 0;
    public static int[] itemDataNum = new int[6];
    public static string[] itemNull = { null, null };

    public static string[] itemPhoto = { "家族写真", "3年前に撮った。この帰りの事故で両親は亡くなった"};
    public static string[] itemRing = { "指輪", "美しい。誰のだろう" };
    public static string[] itemCamera = { "カメラ", "これでタイムトラベルの証拠を撮るのだ" };
    public static string[] itemCord = { "ひも", "丈夫そう。簡単には切れないだろう" };
    public static string[] itemBat = { "バット", "放てホームラン" };
    //public static string[] itemRing = { "腕輪", "去年の誕生日に渡したものだ" };
    public static string[][] itemDataAll;


    //----------------------キャラクター関係のデータ----------------------
    public static int charas = 1;

    public static int[] charaDataNum = {1, 0, 0};
    public static string[] charaRino = { "リノ", "charaImageRino1" };//0
    public static string[] charaKurumi = { "クルミ", "charaImageMikoru1" };//1
    public static string[] charaRinoF = { "リノ", null };
    public static string[] charaKarin = { "カリン", null };
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

        itemDataAll = new string[][] {itemNull, itemPhoto, itemRing, itemCamera , itemCord, itemBat, itemNull, itemNull, itemNull, itemNull};


        charaDataAll = new string[][] {charaNull, charaRino, charaKurumi, charaRinoF, charaKarin };

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
                    charaDataNum[i] = charaDataNum[i + 1];
                    charaDataNum[i + 1] = 0;
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
            if ((int)charaName == charaDataNum[i])
            {
                //最後は空白に
                if (i == 2) charaDataNum[i] = 0;
                else
                {
                    //順番を一つ前へ
                    charaDataNum[i] = charaDataNum[i + 1];
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

    //-----------------難易度関係------------------------
    public static void LoadDifficulty()
    {
        if (currentDifficulty == Difficulty.Auto)
        {
            //自動で難易度変更
            //playerLeverlの値はGameManager.GameOver()内で変更
            //GameManager.EditPlayerLevel()でも
            if (playerLevel <= -3) difficulty = Difficulty.VeryHard;
            else if (playerLevel <= -1) difficulty = Difficulty.Hard;
            else if (playerLevel <= 3) difficulty = Difficulty.Normal;
            else difficulty = Difficulty.Easy;
        }
    }
}
   
