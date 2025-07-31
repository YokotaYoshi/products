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
    public SpotLightController spotLightCnt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHomeScene && !spotLightCnt.isGameOver && !isGoal)
        {
            //操作できる
            isMovable = true;
        }
        else
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

        axisX = Input.GetAxis("Horizontal");
        axisY = rb.linearVelocity.y;
        //真ん中に近づきすぎるとそれ以上前にいけないように
        if (distance < 0.9f)
        {
            axisZ = (Input.GetAxis("Vertical") - 1.0f) / 2f;

        }
        else if (distance > 19f)
        {
            axisZ = (Input.GetAxis("Vertical") + 1.0f) / 2f;
        }
        else
        {
            axisZ = Input.GetAxis("Vertical");
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
        if (other.gameObject.tag == "Goal")
        {
            Debug.Log("Goal");
            if (!spotLightCnt.isGameOver)
            {
                isGoal = true;
            }
            
        }
    }
}
