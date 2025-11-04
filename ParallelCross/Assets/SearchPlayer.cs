using UnityEngine;

public class SearchPlayer : MonoBehaviour
{
    GameObject player;
    Vector2 playerDirection;
    float distance;
    //レイキャストでプレイヤーを探して敵に伝える
    public int point;
    //レイキャスト
    RaycastHit2D hitPlayer;
    RaycastHit2D[] hitOthers;
    GameObject[] searchPoints;
    Vector2[] searchPointsDirection;
    //見た目→後で消す
    SpriteRenderer spriteRenderer;
    CircleCollider2D cCollider;
    bool isActive = true;
    float inactiveTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        searchPoints = GameObject.FindGameObjectsWithTag("SearchPoint");
        //自分も含まれる
        searchPointsDirection = new Vector2[searchPoints.Length];
        for (int i = 0; i < searchPoints.Length; ++i)
        {
            searchPointsDirection[i] = new Vector2(searchPoints[i].transform.position.x - transform.position.x, searchPoints[i].transform.position.y - transform.position.y);
            //Debug.Log(searchPointsDirection[i]);
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        cCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);//ここから見たプレイヤーの位置

        distance = Mathf.Abs(playerDirection.x) + Mathf.Abs(playerDirection.y);//斜めの位置にあるものの評価低めにする

        Ray ray = new Ray(transform.position, playerDirection);
        //Debug.DrawRay(ray.origin, ray.direction * playerDirection.magnitude, Color.red, 0.5f);
        /*
        int excludedLayer = LayerMask.NameToLayer("Enemy");
        int excludedMask = 1 << excludedLayer;
        int invertedMask = ~excludedMask;//~:ビット反転で除外レイヤー以外を対象にする
        */
        int Mask = LayerMask.GetMask("Default");//"Default"レイヤーのみ

        hitPlayer = Physics2D.Raycast(transform.position, playerDirection, playerDirection.magnitude, Mask);

        if (hitPlayer.collider != null)
        {
            if (hitPlayer.collider.gameObject.name == "Player")
            {
                point = 1;//プレイヤーが見える位置
            }
            else
            {
                point = 2;
            }
        }

        //見える位置でも遠すぎる場合は論外
        if (distance > 8f) point = 3;
        if (distance > 11f) point = 4;
        if (distance > 15f) point = 5;
        //Debug.Log(point);
        DebugMethod();

        //コライダーOnOff
        if (isActive)
        {
            if (inactiveTime > 0f)
            {
                inactiveTime -= Time.deltaTime;
            }
            else
            {
                cCollider.enabled = true;
            }
        }
        else
        {
            cCollider.enabled = false;
            inactiveTime = 1f;
        }
    }

    void DebugMethod()
    {
        if (spriteRenderer != null)
        {
            switch (point)
            {
                case 1:
                    spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
                    break;
                case 2:
                    spriteRenderer.color = new Color(0f, 1f, 0f, 1f);
                    break;
                case 3:
                    spriteRenderer.color = new Color(0f, 0f, 1f, 1f);
                    break;
                case 4:
                    spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case 5:
                    spriteRenderer.color = new Color(0f, 0f, 0f, 1f);
                    break;
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isActive = false;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isActive = true;
        }
    }
}
