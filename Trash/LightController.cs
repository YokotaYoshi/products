using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightController : MonoBehaviour
{
    public string direction = "right";
    float speed = 20f;
    Vector2 position;
    float time = 0f;
    float lifeTime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= lifeTime)
        {
            //一定時間経過で削除
            Destroy(gameObject);
        }
        position = transform.position;
        if (direction == "right")
        {
            position += new Vector2(speed * Time.deltaTime, 0f);
        }
        else if (direction == "left")
        {
            position += new Vector2(-speed * Time.deltaTime, 0f);
        }
        else if (direction == "up")
        {
            position += new Vector2(0f, speed * Time.deltaTime);
        }
        else if (direction == "down")
        {
            position += new Vector2(0f, -speed * Time.deltaTime);
        }
        transform.position = position;
    }
}
