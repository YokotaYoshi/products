using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour
{
    private float speed = 10.0f;
    private Rigidbody2D rbball;
    private bool start = false;
    private GameObject paddle;
    private GameObject aim;
    private GameObject block;
    private GameObject deadzone;
    public int score=0;
    public Text startText;

    // Start is called before the first frame update
    void Start()
    {
        rbball = GetComponent<Rigidbody2D>();
        paddle = GameObject.FindWithTag("Paddle");
        aim = GameObject.FindWithTag("Aim");
        block = GameObject.FindWithTag("Block");
        deadzone = GameObject.FindWithTag("DeadZone");
        
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Paddle"))
        {
            Vector2 hitPoint = collision.contacts[0].point;
            Vector2 paddleCenter = collision.collider.bounds.center;

            float difference = hitPoint.x - paddleCenter.x;
            float width = collision.collider.bounds.size.x;
            float bounceAngle = difference / width * 2; // 調整された反射角度
            rbball.velocity = new Vector2(bounceAngle*speed, Mathf.Abs(rbball.velocity.y)).normalized * speed;
        }
        if (collision.gameObject.CompareTag("Block"))
        {
            Destroy(collision.gameObject);
            score +=1;

            GameManagerScript gameManager = FindObjectOfType<GameManagerScript>();
            if (gameManager != null)
            {
                gameManager.UpdateScore(score);
            }
        }
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            start = false;
            Destroy(gameObject);
            GameManagerScript gameManager = FindObjectOfType<GameManagerScript>();
            if (gameManager != null)
            {
                gameManager.Dead();
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(aim.transform.position.x-transform.position.x,aim.transform.position.y-transform.position.y);
        if (start == false)
        {
            transform.position = new Vector2(paddle.transform.position.x, paddle.transform.position.y+1f);
            if (Input.GetKey(KeyCode.Space))
            {
                start = true;
                rbball.velocity = direction*speed;
                startText.text = "";
            }
        }
        if (score>=10)
        {
            start = false;
            Destroy(gameObject);
        }
    }
        
}
