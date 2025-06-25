using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarScript : MonoBehaviour
{
    private float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("right"))
        {
            if (transform.position.x>=6.5f)
            {
                return;
            }
            else
            {
                transform.Translate(speed*Time.deltaTime, 0, 0, Space.Self);
            }
            
        }
        if (Input.GetKey("left"))
        {
            if (transform.position.x<=-6.5f)
            {
                return;
            }
            else
            {
                transform.Translate(-1*speed*Time.deltaTime, 0, 0, Space.Self);
            }
        }
    }
}
