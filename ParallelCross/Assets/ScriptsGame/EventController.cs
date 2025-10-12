using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public bool canRotate;//方向転換できるかどうか
    GameObject player;
    Vector2 playerPosition;
    SpriteRenderer spriteRenderer;
    public Sprite downImage;//下向きの画像
    public Sprite rightImage;//右向きの画像
    public Sprite upImage;//上向きの画像
    public Sprite leftImage;//左向きの画像


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //方向転換できるなら、プレイヤー、SpriteRendererを取得
        if (canRotate)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            spriteRenderer = this.GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------方向転換できるもの------------------------
        if (canRotate)
        {
            //自分から見たプレイヤーの位置
            playerPosition = new Vector2(player.transform.position.x - transform.position.x, 
            player.transform.position.y - transform.position.y);

            if (playerPosition.y * 1.1f <= playerPosition.x && playerPosition.y * 1.1f <= -playerPosition.x)
            {
                spriteRenderer.sprite = downImage;//下向きの画像を代入
            }
            else if (playerPosition.y * 1.1f <= playerPosition.x && playerPosition.y * 1.1f > -playerPosition.x)
            {
                spriteRenderer.sprite = rightImage;//右向きの画像を代入
            }
            else if (playerPosition.y * 1.1f > playerPosition.x && playerPosition.y * 1.1f <= -playerPosition.x)
            {
                spriteRenderer.sprite = leftImage;//左向きの画像を代入
            }
            else 
            {
                spriteRenderer.sprite = upImage;//上向きの画像を代入
            }
        }
    }
}
