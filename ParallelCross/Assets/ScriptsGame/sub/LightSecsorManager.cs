using UnityEngine;

public class LightSecsorManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Light")
        {
            Destroy(other.gameObject);
            Debug.Log("光感知");
        }
    }
}
