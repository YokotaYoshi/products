using UnityEngine;

public class ObjectController : MonoBehaviour
{
    float posX;
    float posY;
    public float speedX = 3.0f;
    public float limitRight = 5.0f;
    public float limitLeft = -5.0f;

    public float speedY = 0f;
    public float limitUp = 5.0f;
    public float limitDown = -5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        posX += speedX * Time.deltaTime;
        posY += speedY * Time.deltaTime;

        if (posX > limitRight)
        {
            Debug.Log("左端へ");
            posX = limitLeft;
        }
        else if (posX < limitLeft)
        {
            Debug.Log("右端へ");
            posX = limitRight;
        }
        if (posY > limitUp)
        {
            posY = limitDown;
        }
        else if (posY < limitDown)
        {
            posY = limitUp;
        }

        transform.position = new Vector2(posX, posY);
    }

    public void StopX()
    {
        speedX = 0.0f;
    }

    public void StopY()
    {
        speedY = 0.0f;
    }
}
