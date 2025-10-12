using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaitController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //ゲームオーバー画面で自機から逃げるように動く
    public float speed;
    Direction moveDirection;
    GridMove gridMove;
    
    void Start()
    {
        gridMove = GetComponent<GridMove>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Random.value);
        Debug.Log(moveDirection);
        if (Random.value <= 0.25f)
        {
            moveDirection = Direction.Right;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Load());
        }
    }

    IEnumerator Load()
    {
        float time = 0.0f;
        while (true)
        {
            yield return null;
            time += Time.deltaTime;
            if (time >= 1.0f) break;
        }
        SceneManager.LoadScene("Home0");//バッドエンドを見せる
    }
}
