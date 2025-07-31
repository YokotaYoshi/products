using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunScript : MonoBehaviour
{
    public bool isRight = false;
    public bool isLeft = false;
    public bool isUp = false;
    public bool isDown = false;
    public float interval = 3.0f;
    public float elapsedTime = 1.0f;
    public GameObject laserRight;
    public GameObject laserLeft;
    public GameObject laserUp;
    public GameObject laserDown;
    public GameManager gameManager;
    Vector3 launchPosition;
    private Rigidbody2D rb2d;
    float Movedistance = 1.0f;
    float Moveduration = 0.2f;
    float Movechecktime = 0.1f;
    float InputStay = 1.0f;
    float Modifytime = 0.19f;
    float speed = 5.0f;
    float merge = 0.1f;
    bool canMove = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;          
        if (elapsedTime > interval)
        {
            elapsedTime = 0f;
            if (isRight)
            {
                Instantiate (laserRight, transform.position, Quaternion.identity);
            }
            else if (isLeft)
            {
                Instantiate (laserLeft, transform.position, Quaternion.identity);
            }
            else if (isUp)
            {
                Instantiate (laserUp, transform.position, Quaternion.identity);
            }
            else if (isDown)
            {
                Instantiate (laserDown, transform.position, Quaternion.identity);
            } 
        }  
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 Distance = transform.position - player.transform.position;
        Vector3 up = Distance - Vector3.up;
        Vector3 down = Distance - Vector3.down;
        Vector3 left = Distance - Vector3.left;
        Vector3 right = Distance - Vector3.right;
        InputStay += Time.deltaTime;
        if (Distance.magnitude < 1.1f||canMove)
        {
            rb2d.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            rb2d.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            rb2d.constraints |= RigidbodyConstraints2D.FreezePositionX;
            rb2d.constraints |= RigidbodyConstraints2D.FreezePositionY;
        }
        if (InputStay > Moveduration && gameManager.isClear == false)
        {
            if (up.magnitude < merge && Input.GetKeyDown(KeyCode.UpArrow))
            {
                StartCoroutine(Move(Vector3.up));
                InputStay = 0f;
            }
            if (down.magnitude < merge && Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine(Move(Vector3.down));
                InputStay = 0f;
            }
            if (right.magnitude < merge && Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine(Move(Vector3.right));
                InputStay = 0f;
            }
            if (left.magnitude < merge && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine(Move(Vector3.left));
                InputStay = 0f;
            }
        }  
        if (InputStay >= Modifytime)
        {
            transform.position = new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),transform.position.z);      
        }         
    }

    private IEnumerator Move(Vector3 direction)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction * Movedistance;
        float elapsedTime = 0.01f;
        rb2d.linearVelocity = direction*speed;
        while (elapsedTime < Moveduration)
        {
            
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= Movechecktime)
            {
                Vector3 location = transform.position - startPosition;
                if (location.magnitude >= 0.3f)
                {
                    rb2d.linearVelocity = direction*speed;
                }
                else
                {
                    transform.position = new Vector3(Mathf.Round(startPosition.x),Mathf.Round(startPosition.y),transform.position.z);
                    rb2d.linearVelocity = new Vector3 (0,0,0);
                    yield break;
                }
            }
            yield return null;            
        }
        rb2d.linearVelocity = new Vector3 (0,0,0);
        transform.position = new Vector3(Mathf.Round(targetPosition.x),Mathf.Round(targetPosition.y),transform.position.z);      
    }    
    private IEnumerator ForcedMove(Vector3 direction)
    {
        yield return new WaitForSeconds(0.12f);
        yield return Move(direction);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("OnewayRight")||other.CompareTag("OnewayLeft")||other.CompareTag("OnewayUp")||other.CompareTag("OnewayDown"))
        {
            canMove = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("OnewayRight")||other.CompareTag("OnewayLeft")||other.CompareTag("OnewayUp")||other.CompareTag("OnewayDown"))
        {
            canMove = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {    
        
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
