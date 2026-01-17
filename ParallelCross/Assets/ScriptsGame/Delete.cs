using UnityEngine;

public class Delete : MonoBehaviour
{
    float time = 0f;
    public float deleteTime = 0.33f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= deleteTime)
        {
            Destroy(gameObject);
        }
    }
}
