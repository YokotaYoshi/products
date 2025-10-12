using UnityEngine;
//Mirrorとの違いは反射映像を移すこと

public class ReflectionManager : MonoBehaviour
{
    GameObject reflectObject;
    SpriteRenderer thisSpriteRenderer;
    SpriteRenderer targetSpriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisSpriteRenderer = this.GetComponent<SpriteRenderer>();
        reflectObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetSpriteRenderer != null)
        {
            thisSpriteRenderer.sprite = targetSpriteRenderer.sprite;
        }
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        reflectObject = other.gameObject;

        if (targetSpriteRenderer != null)
        {
            targetSpriteRenderer = reflectObject.GetComponent<SpriteRenderer>();
            Debug.Log("反射");
        }
    }
}
