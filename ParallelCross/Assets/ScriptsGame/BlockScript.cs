using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    //オブジェクトの見た目とかをコントロールする
    //以下このブロック群をよけるのに必要な移動量
    public float up = 1.0f;
    public float down = 1.0f;
    public float right = 1.0f;
    public float left = 1.0f;

    //スプライト切り替えや生成破壊も担当  

    //eventProgressの値に応じてスプライトを切り替える

    SpriteRenderer spriteRenderer;
    public Sprite sprite0;

    public Sprite sprite1;
    public Sprite sprite1Sub;
    //eventProgressMainSubがこれ以上ならスプライト切り替えるか削除
    //両方ゼロなら切りかえないオブジェクト
    public int eventProgressMainBase = 0;
    public int eventProgressSubBase = 0;
    public bool willDestroy = false;//イベント進行でオブジェクト削除
    public GameObject createObject;
    public float animateTime = 0.3f;

    GameObject player;
    Vector2 playerPosition;
    AnimationManager animManager;

    //eventProgressがBase以上だったら固定
    //それ以下の場合は一時的に変更することもありうる

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        //Debug.Log("あ");
        player = GameObject.FindGameObjectWithTag("Player");
        animManager = GetComponent<AnimationManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (eventProgressMainBase != 0)
        {
            if (Data.eventProgressMain >= eventProgressMainBase)
            {
                if (willDestroy) 
                {
                    if (createObject != null)
                    {
                        Instantiate(createObject, transform.position, Quaternion.identity);
                        Data.loadPosX = transform.position.x;//仲間を生成する場合必要になる
                        Data.loadPosY = transform.position.y;
                    }

                    Destroy(gameObject);
                }
                else if (sprite1 != null) spriteRenderer.sprite = sprite1;
            }
        }
        
        if (eventProgressSubBase != 0)
        {
            if (Data.eventProgressSub >= eventProgressSubBase)
            {
                if (willDestroy) 
                {
                    if (createObject != null)
                    {
                        Instantiate(createObject, transform.position, Quaternion.identity);
                        Data.loadPosX = transform.position.x;
                        Data.loadPosY = transform.position.y;
                    }
                    Destroy(gameObject);
                }
                else if (sprite1Sub != null) spriteRenderer.sprite = sprite1Sub;
            }
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
    }

    IEnumerator ChangeTemporarily()
    {
        //一時的にスプライト変更
        float time = 0.0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite1;
        while (true)
        {
            yield return null;
            time += Time.deltaTime;
            if (time >= animateTime)
            {
                spriteRenderer.sprite = sprite0;
                yield break;
            }
        }
    }

    public void ChangePermanently()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //変えたら戻さない
        if (willDestroy)
        {
            Destroy(gameObject);
        }
        else
        {
            spriteRenderer.sprite = sprite1;
        }
    }

    
}
