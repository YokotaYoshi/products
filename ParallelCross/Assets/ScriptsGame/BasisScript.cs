using UnityEngine;

public class BasisScript : MonoBehaviour
{
    public Vector2 nearestGrid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nearestGrid = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }
}
