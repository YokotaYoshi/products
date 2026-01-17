using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    //オブジェクトの見た目とかをコントロールする
    
    //スプライト切り替えや生成破壊も担当  

    //eventProgressの値に応じてスプライトを切り替える

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    public bool collisionStart = true;//最初判定あり
    public bool collisionEnd = true;//最後判定あり
    public Sprite sprite0;

    public Sprite sprite1;
    //eventProgressMainSubがこれ以上ならスプライト切り替えるか削除
    //両方ゼロなら切りかえないオブジェクト
    public int eventProgressMainBase = 0;
    public int eventProgressSubBase = 0;
    public bool willDestroy = false;//イベント進行でオブジェクト削除
    public bool willCreate = false;//イベント進行でオブジェクト生成
    public GameObject createObject;
    public float animateTime = 0.3f;

    GameObject player;
    Vector2 playerPosition;
    AnimationManager animManager;
    public ItemName keyItem = ItemName.Null;//このオブジェクトになにかしら影響を与えるアイテム
    public ItemName itemSelf = ItemName.Null;//このオブジェクト自体の要素

    //eventProgressがBase以上だったら固定
    //それ以下の場合は一時的に変更することもありうる

    //Inspectorから指定して動かせるように
    public bool customMove = false;//動かすかどうか
    public bool loop = false;//終端で消すかどうか
    float posX;
    float posY;
    public float speedX = 3f;
    public float limitRight = 5f;
    public float limitLeft = -5f;

    public float speedY = 0f;
    public float limitUp = 5f;
    public float limitDown = -5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Debug.Log("あ");
        player = GameObject.FindGameObjectWithTag("Player");
        animManager = GetComponent<AnimationManager>();

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);//レイヤーが同じなら下にあるほど手前に見える

        if (willCreate)
        {
            if (Data.eventProgressMain < eventProgressMainBase)
            {
                Destroy(gameObject);
            }
        }
        if (boxCollider != null)
        {
            if (!collisionStart) boxCollider.enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //アイテムなら取得した瞬間に削除
        for (int i = 0; i < Data.items; ++i)
        {
            if (Data.itemDataNum[i] == (int)itemSelf)
            {
                Destroy(gameObject);
            }
        }
        
        if (eventProgressMainBase != 0 && eventProgressSubBase != 0)
        {
            if (Data.eventProgressMain >= eventProgressMainBase && Data.eventProgressSub >= eventProgressSubBase)
            {
                ChangePermanently();
            }
        }
        else
        {
            if (willCreate) return;
            if (eventProgressMainBase != 0)
            {
                if (Data.eventProgressMain >= eventProgressMainBase)
                {
                    ChangePermanently();
                }
            }

            if (eventProgressSubBase != 0)
            {
                //こっちは一致した場合だけ変更
                if (Data.eventProgressSub == eventProgressSubBase)
                {
                    ChangePermanently();
                }
                else 
                {
                    ChangeBackward();
                }
            }
        }
        if (eventProgressMainBase != 0 && Data.eventProgressMain > eventProgressMainBase)
        {
            ChangePermanently();//イベントがかなり先に進んでいる場合も考慮
        }
        

        if (animManager != null)
        {
            //プレイヤーの方向にmoveDirectionを一致させる
            playerPosition = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

            if (playerPosition.y > playerPosition.x && playerPosition.y > -playerPosition.x)
            {
                animManager.moveDirection = Direction.Up;
            }
            else if (playerPosition.y <= playerPosition.x && playerPosition.y > -playerPosition.x)
            {
                animManager.moveDirection = Direction.Right;
            }
            else if (playerPosition.y > playerPosition.x && playerPosition.y <= -playerPosition.x)
            {
                animManager.moveDirection = Direction.Left;
            }
            else
            {
                animManager.moveDirection = Direction.Down;
            }
        }

        if (customMove)
        {
            posX = transform.position.x;
            posY = transform.position.y;
            posX += speedX * Time.deltaTime;
            posY += speedY * Time.deltaTime;

            if (posX > limitRight)
            {
                Debug.Log("左端へ");
                posX = limitLeft;
                if (!loop)
                {
                    Destroy(gameObject);
                }
            }
            else if (posX < limitLeft)
            {
                Debug.Log("右端へ");
                posX = limitRight;
                if (!loop)
                {
                    Destroy(gameObject);
                }
            }
            if (posY > limitUp)
            {
                posY = limitDown;
                if (!loop)
                {
                    Destroy(gameObject);
                }
            }
            else if (posY < limitDown)
            {
                posY = limitUp;
                if (!loop)
                {
                    Destroy(gameObject);
                }
            }

            transform.position = new Vector2(posX, posY);
        }
    }

    public IEnumerator ChangeTemporarily()
    {
        //一時的にスプライト変更
        float time = 0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite1;

        while (true)
        {
            yield return null;
            time += Time.deltaTime;
            if (time >= animateTime)
            {
                //元の画像に戻す
                spriteRenderer.sprite = sprite0;
                break;
            }
        }

    }


    public void ChangePermanently()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //変えたら戻さない
        if (willDestroy)
        {
            if (createObject != null)
            {
                //代わりとなるオブジェクトを生成する
                Instantiate(createObject, transform.position, Quaternion.identity);

            }
            Destroy(gameObject);
        }
        else //if (sprite1 != null)
        {
            //画像を差し替え
            spriteRenderer.sprite = sprite1;

        }
        if (boxCollider != null)
        {
            if (collisionEnd) boxCollider.enabled = true;
            else boxCollider.enabled = false;
        }
        
    }
    
    public void ChangeBackward()
    {
        //もとの状態に戻す
        spriteRenderer.sprite = sprite0;
        if (boxCollider != null)
        {
            if (collisionStart) boxCollider.enabled = true;
            else boxCollider.enabled = false;
        }
        
    }

    public void Solve()
    {
        //キーアイテムを持っていたらオブジェクトを消去
        for (int i = 0; i < Data.items; ++i)
        {
            if (Data.itemDataNum[i] == (int)keyItem)
            {
                Destroy(gameObject);
            }
        }
    }

    public void StopX()
    {
        speedX = 0f;
    }

    public void StopY()
    {
        speedY = 0f;
    }
}
