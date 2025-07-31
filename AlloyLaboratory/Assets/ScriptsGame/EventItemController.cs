using UnityEngine;

public class EventItemController : MonoBehaviour
{
    //GameManagerに渡す情報
    public string person;
    [Multiline(2)]
    public string text;
    public bool willEquip;//すぐ装備するかどうか
    //ItemDataに渡す情報
    public string itemName;
    [Multiline(2)]
    public string itemData;
    public Sprite itemImage;
    //取得できる状態かどうか
    bool available = false;
    public int eventProgress;//このアイテムを取得したことでGameManagerのeventProgressに加算する値

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (available)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Destroy(gameObject);
                GameManager.eventProgress += eventProgress;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerFocus")
        {
            available = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerFocus")
        {
            available = false;
        }
    }
}
