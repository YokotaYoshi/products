using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float Movedistance = 1.0f;
    float Moveduration = 0.2f;
    float Movechecktime = 0.1f;
    private Rigidbody2D rb2d;
    float speed = 5.0f;
    float InputStay = 1.0f;
    float Modifytime = 0.19f;
    public GameManager gameManager;
    Animator animator;
    int direction = 0;
    public bool isGameover = false;
    public int hasKeys = 0;
    public OnewayboardScript oneway;
    //bool rightMove = false;
    

    void Start()
    {
        hasKeys = 0;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Debug.Log(hasKeys);
        InputStay += Time.deltaTime;
        if (InputStay > Moveduration&& gameManager.isClear == false)
        {
            //0.2秒間隔で入力出来る
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {

                StartCoroutine(Move(Vector3.up));
                InputStay = 0f;
                direction = 2;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine(Move(Vector3.down));
                InputStay = 0f;
                direction = 0;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine(Move(Vector3.right));
                InputStay = 0f;
                direction = 3;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine(Move(Vector3.left));
                InputStay = 0f;
                direction = 1;
            }
        }
        //移動開始から0.19秒で位置を修正
        if (InputStay >= Modifytime)
        {
            transform.position = new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),transform.position.z);      
        } 
        animator.SetInteger("Direction", direction);        
    }
    /*上手く動かなかった
    void FixedUpdate()
    {
        if (rightMove)
        {
            StartCoroutine(Move(Vector3.right));
        }
    }
    */

    private IEnumerator Move(Vector3 playerdirection)
    {
        //0.19秒で移動が完了する
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + playerdirection * Movedistance;
        float elapsedTime = 0.01f;
        rb2d.linearVelocity = playerdirection*speed;
        
        while (elapsedTime < Moveduration)
        {
            //Debug.Log(elapsedTime);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= Movechecktime)
            {
                Vector3 location = transform.position - startPosition;
                //動いてなかったら元の位置に戻す。箱との差はColliderの大きさの差
                if (location.magnitude >= 0.2f)
                {
                    rb2d.linearVelocity = playerdirection*speed;
                }
                else
                {
                    //Debug.Log("修正");
                    transform.position = new Vector3(Mathf.Round(startPosition.x),Mathf.Round(startPosition.y),transform.position.z);
                    rb2d.linearVelocity = new Vector3 (0,0,0);
                    yield break;
                }
            }
            yield return null;            
        }
        rb2d.linearVelocity = Vector2.zero;
        transform.position = new Vector3(Mathf.Round(targetPosition.x),Mathf.Round(targetPosition.y),transform.position.z);        
    }

    private IEnumerator ForcedMove(Vector3 playerdirection)
    {
        yield return new WaitForSeconds(0.12f);
        if (playerdirection.x == 1)
        {
            direction = 3;
        }
        else if (playerdirection.x == -1)
        {
            direction = 1;
        }
        else if (playerdirection.x == 0)
        {
            if (playerdirection.y == 1)
            {
                direction = 2;
            }
            else if (playerdirection.y == -1)
            {
                direction = 0;
            }
        }
        yield return Move(playerdirection);
    }

    private IEnumerator Gameover()
    {
        Destroy(gameObject);
        isGameover = true;
        yield return null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("衝突");
    }

    void OnTriggerEnter2D(Collider2D other)
    {    
        if (other.CompareTag("Damage"))
        {
            if (gameManager.isClear == true)
            {
                return;
            }
            else
            {                
                StartCoroutine(Gameover());
            }
        }
        if (other.CompareTag("OnewayRight"))
        {
            InputStay = -0.14f;
            StartCoroutine(ForcedMove(Vector3.right));         
        }
        if (other.CompareTag("OnewayLeft"))
        {
            InputStay = -0.14f;
            StartCoroutine(ForcedMove(Vector3.left));         
        }
        if (other.CompareTag("OnewayUp"))
        {
            InputStay = -0.14f;
            StartCoroutine(ForcedMove(Vector3.up));         
        }
        if (other.CompareTag("OnewayDown"))
        {
            InputStay = -0.14f;
            StartCoroutine(ForcedMove(Vector3.down));         
        }
    }
    
}

