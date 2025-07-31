using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeleteZoneManager : MonoBehaviour
{
    public bool drop = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("a");
        if (other.gameObject.tag == "Player")
        {
            drop = true;
            Destroy(gameObject);
        }
    }
}
