using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GridMove : MonoBehaviour
{
    //プレイヤー以外のグリッド移動担当
    //外から移動方向と距離を入れるとその通りに動く

    public Vector2 targetGrid;//外部からいじる
    public float speed;//外部からいじる
    public Direction moveDirection = Direction.N;//外部からいじる
    public bool isCoroutineWorking;
    Vector2 targetDirection;
    Vector2 nearestGrid;
    Rigidbody2D rb2d;
    float gap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        nearestGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Debug.Log(speed);
        //Debug.Log(moveDirection);
    }

    void FixedUpdate()
    {
        
        switch (moveDirection)
        {
            case Direction.Right:
                rb2d.linearVelocity = new Vector2(speed, 0f);
                break;
            case Direction.Left:
                rb2d.linearVelocity = new Vector2(-speed, 0f);
                break;
            case Direction.Up:
                rb2d.linearVelocity = new Vector2(0f, speed);
                break;
            case Direction.Down:
                rb2d.linearVelocity = new Vector2(0f, -speed);
                break;
            case Direction.N:
                //コルーチン開始？
                rb2d.linearVelocity = Vector2.zero;
                break;
        }
        
    }
    public IEnumerator Move(Direction moveDirectionEnum, float distance)
    {
        if (isCoroutineWorking) yield break;
        //目標となる格子点まで移動する

        float time = 0f;

        //動いているフラグ立て
        isCoroutineWorking = true;

        float isGoal = 0.1f;//ゴールまでの距離がこれ以下だったらゴールとする

        moveDirection = moveDirectionEnum;

        switch (moveDirection)
        {
            case Direction.Right:
                targetGrid = nearestGrid + new Vector2(distance, 0f);
                break;
            case Direction.Left:
                targetGrid = nearestGrid + new Vector2(-distance, 0f);
                break;
            case Direction.Up:
                targetGrid = nearestGrid + new Vector2(0f, distance);
                break;
            case Direction.Down:
                targetGrid = nearestGrid + new Vector2(0f, -distance);
                break;
        }


        while (true)
        {
            //ゴールまでの距離を更新
            gap = new Vector2(targetGrid.x - transform.position.x, targetGrid.y - transform.position.y).magnitude;

            isGoal = 0.01f * speed;

            time += Time.deltaTime;

            //ゴールに十分近づいたらおわり
            if (gap < isGoal)
            {
                transform.position = nearestGrid;

                //rb2d.linearVelocity = Vector2.zero;//ここで一瞬0になるせいでアニメーションがくずれる?

                moveDirection = Direction.N;
                break;
            }

            //そのほか時間経過でも終わり

            if (time >= 1f)
            {
                transform.position = nearestGrid;
                rb2d.linearVelocity = Vector2.zero;
                moveDirection = Direction.N;
                break;
            }

            yield return null;
        }

        //動いているフラグおろし
        isCoroutineWorking = false;
    }

    public void Stop()
    {
        //移動終了する
        StopAllCoroutines();
        isCoroutineWorking = false;
        rb2d.linearVelocity = Vector2.zero;
        transform.position = nearestGrid;
    }
}
