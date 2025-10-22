using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuardianController : MonoBehaviour
{
    //RayCastでプレイヤーが視界に入ったら追いかけ始める
    //プレイヤーの座標を取得してそこまで行く。
    //そこでもう一度探索して発見したらまた追跡
    //見つからなかったら初期位置にもどる
    //またはプレイヤーが視界に入った段階で遠距離攻撃してくる

    //レイヤーに高いのと低いのを用意して低いやつはRaycastが貫通するとか
    Vector2 basePosition;
    Vector2 currentPosition;
    Vector2 homeDirection;
    GameObject player;
    Vector2 playerDirection;
    
    float time = 1f;
    Rigidbody2D rb2d;
    RaycastHit2D hit;

    public bool willChase;//プレイヤーを見つけて追いかけるかどうか
    public bool isChasing = false;//追いかけフラグ
    public GameObject bullet;//弾丸


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //基準となる座標を取得
        basePosition = transform.position;

        rb2d = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = new Vector2(transform.position.x, transform.position.y);

        homeDirection = new Vector2(basePosition.x - currentPosition.x,
        basePosition.y - currentPosition.y);

        playerDirection = new Vector2(player.transform.position.x - transform.position.x,
        player.transform.position.y - transform.position.y);

        float distance = Vector2.Distance(currentPosition, player.transform.position);

        Ray ray = new Ray(currentPosition, playerDirection);
        Debug.DrawRay(ray.origin, ray.direction * playerDirection.magnitude, Color.red, 0.5f);

        /*
        int excludedLayer = LayerMask.NameToLayer("Enemy");
        int excludedMask = 1 << excludedLayer;
        int invertedMask = ~excludedMask;//~:ビット反転で除外レイヤー以外を対象にする
        */
        int invertedMask = LayerMask.GetMask("Default");

        time += Time.deltaTime;

        //追いかける場合
        if (willChase)
        {
            if (time >= 1f)
            {
                time = 0f;
                //1秒ごとに位置チェック
                hit = Physics2D.Raycast(currentPosition, playerDirection, distance, invertedMask);
            }
            if (hit.collider != null)
            {
                string name = hit.collider.gameObject.name;
                if (name == "Player")
                {
                    Debug.Log(name);
                    isChasing = true;
                }
                else
                {
                    isChasing = false;
                }
            }
        }
        else
        {
            hit = Physics2D.Raycast(currentPosition, playerDirection, distance, invertedMask);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    if (time >= 1f)
                    {
                        Instantiate(bullet, transform.position, Quaternion.identity);
                        time = 0f;
                    }
                }
            }
        }
        
    }

    void FixedUpdate()
    {
        //rb2d.linearVelocity = new Vector2(3f, 0f);
        /*
        if (isChasing)
        {
            rb2d.linearVelocity = new Vector2(playerDirection.normalized.x * 3f,
            playerDirection.normalized.y * 3f);
        }
        else
        {
            if (homeDirection.magnitude < 0.3f)
            {
                transform.position = basePosition;
                rb2d.linearVelocity = Vector2.zero;
            }
            else
            {
                rb2d.linearVelocity = new Vector2(homeDirection.normalized.x * 3f,
                homeDirection.normalized.y * 3f);
            }

        }
        */
    }
}
