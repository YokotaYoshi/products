using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpotLightController : MonoBehaviour
{
    float rotationY = 0f;
    public float rotateSpeed = 60f;
    GameObject player;
    Vector3 rayCastDirection;
    Vector3 playerDirection;
    public bool isGameOver = false;
    public PlayerController playerCnt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        rotationY += rotateSpeed * Time.deltaTime;
        if (rotationY >= 360f)
        {
            rotationY = 0f;
        }
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        //ここからRayCast飛ばす。回転速度を一致させる。上下の角度はボールに合わせる
        playerDirection = new Vector3(PlayerDistance(), player.transform.position.y - transform.position.y, 0f);
        //光の方向に合わせる
        rayCastDirection = Quaternion.Euler(0f, rotationY - 93f, 0f) * playerDirection;
        Ray ray = new Ray(transform.position, rayCastDirection);
        Debug.DrawRay(transform.position, rayCastDirection);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            string name = hit.collider.gameObject.name;//衝突した相手の名前を取得
            if (name != "Ground")
            {
                //Debug.Log(name);
            }

            if (!playerCnt.isGoal && name == "Player")
            {
                isGameOver = true;//プレイヤーにヒットしたらゲームオーバー
                //Time.timeScale = 0;
            }
        }
    }

    float PlayerDistance()
    {
        return Mathf.Sqrt(player.transform.position.x * player.transform.position.x +
        player.transform.position.z * player.transform.position.z);
    }

}
