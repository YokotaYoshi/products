using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallManager : MonoBehaviour
{
    public DeleteZoneManager deleteZoneManager;
    bool delete = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (delete == false)
        {
            delete = deleteZoneManager.drop;
        }
        if (delete == true)
        {
            StartCoroutine(DeleteWall());
        }
    }

    IEnumerator DeleteWall()
    {
        float time = 0f;
        delete = false;
        transform.position -= new Vector3(0f, 7f * Time.deltaTime, 0f);
        while (true)
        {
            time += Time.deltaTime;
            yield return null;
            if (time >= 3.0f)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
