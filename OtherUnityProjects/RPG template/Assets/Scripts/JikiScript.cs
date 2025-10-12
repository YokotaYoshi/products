using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JikiScript : MonoBehaviour
{
    private float speed = 5f;
    private GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("right"))
        {
            transform.Translate(speed*Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("left"))
        {            
            transform.Translate(-1*speed*Time.deltaTime, 0, 0);            
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0,speed*Time.deltaTime, 0);           
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {      
            transform.Translate(0,-speed*Time.deltaTime, 0);          
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("1toB1"))
        {
            transform.position=new Vector3(45,3,0);
        }
        if (collision.gameObject.CompareTag("B1to1"))
        {
            transform.position=new Vector3(5,3,0);
        }
    }
}
