using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUp : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float speed = 10f;
    public LaserGunScript laserGunScript;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (laserGunScript != null)
        {
            rb2d.linearVelocity = new Vector2(0,speed); 
        }
              
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Lasergun"&& other.gameObject.tag != "Goal"&&other.gameObject.tag != "LaserThrough"&&other.gameObject.tag!="Damage"&&other.gameObject.tag!="OnewayRight"&&other.gameObject.tag!="OnewayLeft"&&other.gameObject.tag!="OnewayUp"&&other.gameObject.tag!="OnewayDown")
        {
            Destroy(gameObject);
        }
    }
}
