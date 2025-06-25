using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private float speed = 3f;
    private float time;
    private int num=0;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time>=1f)
        {
            time=0f;
            num = Random.Range(0,4);
        }
        if (num==0)
        {
            
            transform.Translate(speed*Time.deltaTime,0,0);
        }
        if (num==1)
        {
            transform.Translate(-speed*Time.deltaTime,0,0);
        }
        if (num==2)
        {
            if (transform.position.y>=4f)
            {
                return;
            }
            else
            {
                transform.Translate(0,speed*Time.deltaTime,0);
            }            
        }
        if (num==3)
        {
            if (transform.position.y<=-4f)
            {
                return;
            }
            else
            {
                transform.Translate(0,-speed*Time.deltaTime,0);
            } 
        }
    }
    
}
