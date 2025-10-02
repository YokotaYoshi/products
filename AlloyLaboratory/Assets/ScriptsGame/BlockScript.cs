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
    //eventProgressMainSubがこれ以上ならスプライト切り替えるか削除
    //両方ゼロなら切りかえないオブジェクト
    public int eventProgressMainBase = 0;
    public int eventProgressSubBase = 0;
    public bool willDestroy = false;//イベント進行でオブジェクト削除
    public float animateTime = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (eventProgressMainBase == 0 && eventProgressSubBase == 0) return;

        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("あ");

        if (Data.eventProgressMain < eventProgressMainBase)
        {
            //イベント進行していない
            if (willDestroy) return;
            else spriteRenderer.sprite = sprite0;
        }
        else
        {
            if (willDestroy) Destroy(gameObject);
            else spriteRenderer.sprite = sprite1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (eventProgressMainBase == 0 && eventProgressSubBase == 0) return;
        if (Data.eventProgressMain < eventProgressMainBase)
        {
            //イベント進行していない
            if (willDestroy) return;

            else if (sprite0 != null) spriteRenderer.sprite = sprite0;
        }
        else
        {
            if (willDestroy) Destroy(gameObject);
            else if (sprite1 != null) spriteRenderer.sprite = sprite1;
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
