using UnityEngine;

public class SearchPlayer : MonoBehaviour
{
    GameObject player;
    Vector2 playerDirection;
    public float distance;
    //レイキャストでプレイヤーを探して敵に伝える
    public int point = 10;
    //レイキャスト
    int Mask;
    //int MaskSP;
    RaycastHit2D hitPlayer;
    RaycastHit2D[] hitSP;
    //Ray[] raySP;
    GameObject[] searchPoints;
    Vector3[] searchPointsDirection;
    int myNumber;
    //見た目→後で消す
    SpriteRenderer spriteRenderer;
    CircleCollider2D cCollider;
    bool isActive = true;
    float inactiveTime = 0f;
    GameObject enemy;
    Vector2 enemyDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        Mask = LayerMask.GetMask("Default");//Defaultレイヤーのみ
        searchPoints = GameObject.FindGameObjectsWithTag("SearchPoint");
        //自分も含まれる
        hitSP = new RaycastHit2D[searchPoints.Length];
        searchPointsDirection = new Vector3[searchPoints.Length];
        for (int i = 0; i < searchPoints.Length; ++i)
        {
            searchPointsDirection[i] = new Vector3(searchPoints[i].transform.position.x - transform.position.x, searchPoints[i].transform.position.y - transform.position.y, 0f);
            hitSP[i] = Physics2D.Raycast(transform.position + searchPointsDirection[i].normalized, searchPointsDirection[i], searchPointsDirection[i].magnitude);
            //Debug.Log(searchPointsDirection[i]);
            if (searchPointsDirection[i].magnitude < 0.5f)
            {
                myNumber = i;//自分の番号
            }
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        cCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDirection = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);//ここから見たプレイヤーの位置

        if (enemy != null)
        {
            enemyDirection = new Vector2(enemy.transform.position.x - transform.position.x, enemy.transform.position.y - transform.position.y);

            if(enemyDirection.magnitude <= 0.1f)
            {
                isActive = false;
            }
        }
        

        distance = Mathf.Abs(playerDirection.x) + Mathf.Abs(playerDirection.y);//斜めの位置にあるものの評価低めにする

        //Ray ray = new Ray(transform.position, playerDirection);

        //Debug.DrawRay(ray.origin, ray.direction * playerDirection.magnitude, Color.red, 0.5f);
        
        hitPlayer = Physics2D.Raycast(transform.position, playerDirection, playerDirection.magnitude, Mask);
        


        if (hitPlayer.collider != null)
        {
            if (hitPlayer.collider.gameObject.name == "Player")
            {
                //プレイヤーが見える位置
                point = 1;
            }
            else
            {
                int minPoint = 10;
                for (int i = 0; i < searchPoints.Length; ++i)
                {
                    //Debug.Log(i);
                    if (i != myNumber) 
                    {
                        
                        //Debug.Log(gameObject.name);
                        //Debug.Log(hitSP[i].collider.gameObject.name);
                        if (hitSP[i].collider != null && hitSP[i].collider.gameObject.tag == "SearchPoint")
                        {
                            minPoint = Mathf.Min(minPoint, hitSP[i].collider.gameObject.GetComponent<SearchPlayer>().point);
                        }
                        point = minPoint + 1;
                    }
                    
                }
                //point = 2;
            }
            //1が見えるいちなら2、2が見える位置なら3、という風にしたい
        }


        if (point == 2)
        {
            //Debug.Log(hitPlayer.collider.gameObject.name);
        }
        //Debug.Log(point);
        DebugMethod();

        //コライダーOnOff
        if (isActive)
        {
            //敵はこの点から離れた
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
            //敵はこの点の近くにいる
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
                default:
                    spriteRenderer.color = new Color(0f, 0f, 0f, 1f);
                    break;
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.gameObject.tag == "Enemy")
        {
            isActive = false;
        }
        */
    }
    void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Enemy")
        {
            isActive = true;
        }
        
    }
}
