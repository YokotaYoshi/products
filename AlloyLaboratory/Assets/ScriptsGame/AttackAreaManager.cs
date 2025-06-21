using UnityEngine;

public class AttackAreaManager : MonoBehaviour
{
    float time = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.5f)
        {
            Destroy(gameObject);
        }
    }
}
