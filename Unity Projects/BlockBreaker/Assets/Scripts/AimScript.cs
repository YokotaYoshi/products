using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
    private Vector2 startPosition;   
    private float pi = Mathf.PI;
    int a =1;
    private float dx =0.0f;
    private GameObject paddle;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        paddle = GameObject.FindWithTag("Paddle");
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time;
        float T = t-(float)(1.0f*(int)(t/a));
        float y = Mathf.Abs(Mathf.Sin(t*pi));
        float x = Mathf.Abs(Mathf.Cos(t*pi));
        
        if (0<=T&&T<0.25)
        {
            dx = y;
        }
        else if (0.25<=T&&T<0.5)
        {
            dx = x;
        }
        else if (0.5<=T&&T<0.75)
        {
            dx = -x;
        }
        else if (0.75<=T&&T<1.0)
        {
            dx = -y;
        }
        float dy = Mathf.Max(x,y);
        transform.position = new Vector2(paddle.transform.position.x + startPosition.x+dx, startPosition.y+dy-1f);
        if (Input.GetKey(KeyCode.Space))
        {
            gameObject.SetActive(false);
        }
    }
}
