using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float axisX;
    float axisY;
    float axisZ;
    float speedX;
    float speedY;
    float speedZ;
    float speed;
    float theta;
    float distance;
    bool isDiving = false;
    Rigidbody rb;
    float maxSpeed = 15.0f;
    public bool isGoal = false;
    public bool isHomeScene = false;
    bool isMovable = true;
    GameObject[] SpotLights;
    SpotLightController[] spotLightCnt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SpotLights = GameObject.FindGameObjectsWithTag("SpotLight");
        spotLightCnt = new SpotLightController[SpotLights.Length];
        for (int i = 0; i < SpotLights.Length; ++i)
        {
            spotLightCnt[i] = SpotLights[i].GetComponent<SpotLightController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < SpotLights.Length; ++i)
        {
            if (spotLightCnt[i].isGameOver)
            {
                isMovable = false;
            }
        }
        if (isHomeScene || isGoal)
        {
            //操作できない
            isMovable = false;
        }
        
        //スペースキーで急降下
        if (isMovable && Input.GetKeyDown(KeyCode.Space))
        {
            isDiving = true;
        }
        if (isMovable && Input.GetKeyUp(KeyCode.Space))
        {
            isDiving = false;
        }

        distance = Mathf.Sqrt(transform.position.x * transform.position.x +
        transform.position.z * transform.position.z);

        //axisX = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.D))
        {
            axisX = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            axisX = -1f;
        }
        else
        {
            axisX = 0f;
        }
        
        axisY = rb.linearVelocity.y;
        
        if (Input.GetKey(KeyCode.W))
        {
            if (distance < 0.9f)
            {
                //真ん中に近づきすぎるとそれ以上前にいけないように
                axisZ = 0f;
            }
            else
            {
                axisZ = 1f;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (distance > 19f)
            {
                //円盤ステージから落ちないように
                axisZ = 0f;
            }
            else
            {
                axisZ = -1f;
            }
        }
        else
        {
            axisZ = 0f;
        }
        
        
        theta = Mathf.Atan2(transform.position.z, transform.position.x);
        
    }

    void FixedUpdate()
    {
        //行列計算。Θ+π/2の回転かける(axisX, axisV)
        speedX = Mathf.Cos(theta + Mathf.PI / 2) * axisX - Mathf.Sin(theta + Mathf.PI / 2) * axisZ;
        speedZ = Mathf.Sin(theta + Mathf.PI / 2) * axisX + Mathf.Cos(theta + Mathf.PI / 2) * axisZ;
        
        if (isDiving)
        {
            speedY = axisY - 0.3f;
        }
        else
        {
            speedY = axisY;
        }
        if (speedY < -maxSpeed)
        {
            //Debug.Log("終端");
            speedY = -maxSpeed;
        }
        if (speedY > maxSpeed)
        {
            speedY = maxSpeed;
        }
        //中央から遠いほど速く動ける。
        speed = 3f + distance / 6f;

        if (isMovable)
        {
            rb.linearVelocity = new Vector3(speed * speedX, speedY, speed * speedZ);
        }
        else
        {
            rb.linearVelocity = new Vector3(0f, speedY, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < SpotLights.Length; ++i)
        {
            if (spotLightCnt[i].isGameOver)
            {
                return;
            }
        }
        if (other.gameObject.tag == "Goal")
        {
            Debug.Log("Goal");
            
            isGoal = true;
        }
    }
}
