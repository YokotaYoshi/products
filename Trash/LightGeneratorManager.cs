using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightGeneratorManager : MonoBehaviour
{
    public string direction = "right";
    public GameObject lightObj;
    LightController lightCnt;

    float time = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightCnt = lightObj.GetComponent<LightController>();
    }

    

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1f)
        {
            time = 0f;
            lightCnt.direction = direction;
            Instantiate(lightObj, transform.position, Quaternion.identity);
        }
    }
}
