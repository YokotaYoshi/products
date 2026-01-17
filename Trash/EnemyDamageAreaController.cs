using UnityEngine;

public class EnemyDamageAreaController : MonoBehaviour
{
    EnemyChaseController enemyChaseCnt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyChaseCnt = GetComponentInParent<EnemyChaseController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
