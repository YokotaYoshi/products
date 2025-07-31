using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MirrorManager : MonoBehaviour
{
    //光が当たったらそれを削除して、新たに光を生成して特定の方向に打ち出す

    public string direction = "up";
    public GameObject lightObj;
    LightController lightCnt;
    public bool isPermeable = false;//透過するかどうか
    int lightDensity = 0;
    float time = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightCnt = lightObj.GetComponent<LightController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isPermeable)
        {
            //一定時間に一定以上の光が流れ込んだらオブジェクトを破壊
            time += Time.deltaTime;
            if (time >= 0.1f && lightDensity >= 1)
            {
                lightDensity -= 1;
                time = 0f;
            }
            if (lightDensity >= 100)
            {
                Destroy(gameObject);
            }
        }

        //プレイヤーが回転させられるように
        //右向きに置かれているなら　right
        //そのままにする、上に向ける up 、下に向けるdown、倒すleft
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Light")
        {
            if (isPermeable)
            {
                lightDensity += 1;
            }
            //光が当たったら
                //鏡を倒す→透過
                //正面→反射
                //傾ける→曲げる
                Destroy(other.gameObject);//削除
            if (direction == "up")
            {
                //上向きに光を射出
                lightCnt.direction = "up";
                Instantiate(lightObj, new Vector2(transform.position.x, transform.position.y + 0.7f), Quaternion.identity);
            }
            else if (direction == "down")
            {
                //下向きに光を射出
                lightCnt.direction = "down";
                Instantiate(lightObj, new Vector2(transform.position.x, transform.position.y - 0.7f), Quaternion.identity);
            }
            else if (direction == "right")
            {
                //右向きに光を射出
                lightCnt.direction = "right";
                Instantiate(lightObj, new Vector2(transform.position.x + 0.7f, transform.position.y), Quaternion.identity);
            }
            else if (direction == "left")
            {
                //左向きに光を射出
                lightCnt.direction = "left";
                Instantiate(lightObj, new Vector2(transform.position.x - 0.7f, transform.position.y), Quaternion.identity);
            }
        }
    }
}
