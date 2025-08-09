using UnityEngine;

public class SwitchImageScript : MonoBehaviour
{
    //特定の条件のもとで画像を切り替える

    public Sprite downImage;
    public Sprite upImage;
    public Sprite rightImage;
    public Sprite leftImage;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb2d.linearVelocity.y < 0f)
        {
            //下向き画像が最優先
            spriteRenderer.sprite = downImage;
        }
        else if (rb2d.linearVelocity.x > 0f)
        {
            //次右向き
            spriteRenderer.sprite = rightImage;
        }
        else if (rb2d.linearVelocity.x < 0f)
        {
            //次左向き
            spriteRenderer.sprite = leftImage;
        }
        else if (rb2d.linearVelocity.y > 0f)
        {
            //次上向き
            spriteRenderer.sprite = upImage;
        }
        else
        {
            //一切動いていないなら下向き
            //spriteRenderer.sprite = downImage;
        }
    }
}
