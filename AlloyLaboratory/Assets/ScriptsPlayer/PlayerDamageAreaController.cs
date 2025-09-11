using UnityEngine;

public class PlayerDamageAreaController : MonoBehaviour
{
    //プレイヤーのダメージ処理はこっちで
    public GameObject player;
    PlayerController playerCnt;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player != null)
        {
            playerCnt = player.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("ヒット");
        //---------------------------------------------敵と接触----------------------------------------------------
        
        if (!playerCnt.isInvincible)
        {
            if (other.gameObject.tag == "Damage1")
            {
                //敵に接触した時の処理
                PlayerController.hp -= 1;
                playerCnt.isAttacked = true;
                playerCnt.blownDirection = new Vector2(transform.position.x - other.transform.position.x, transform.position.y - other.transform.position.y).normalized;
            }
            if (other.gameObject.tag == "Damage2")
            {
                //敵に接触した時の処理
                PlayerController.hp -= 2;
                playerCnt.isAttacked = true;
                playerCnt.blownDirection = new Vector2(transform.position.x - other.transform.position.x, transform.position.y - other.transform.position.y).normalized;
            }
            if (other.gameObject.tag == "Damage3")
            {
                //即死
                PlayerController.hp -= 3;
            }
        }
    }
}
