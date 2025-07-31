using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //ゴールと球を2:1に外分する点
    //視点は常に中央向き
    float positionX;
    float positionY;
    float positionZ;
    Vector2 playerPositionXZ;
    float directionX;
    float directionY;
    float directionZ;
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            SetCameraPosition();//カメラの位置調整
            SetCameraDirection();//カメラの方向調整
             //カメラの位置と方向を更新
            transform.position = new Vector3(positionX, positionY, positionZ);
            transform.rotation = Quaternion.Euler(directionX, directionY, directionZ);
        }
    }

    void SetCameraPosition()
    {
        playerPositionXZ = new Vector2(player.transform.position.x, player.transform.position.z);
        //カメラの位置
        positionX = playerPositionXZ.x + playerPositionXZ.normalized.x * 10.0f;
        if (player.transform.position.y > 3.5f)
        {
            positionY = player.transform.position.y + 1.0f;
        }
        else
        {
            positionY = 4.5f;
        }
        positionZ = playerPositionXZ.y + playerPositionXZ.normalized.y * 10.0f;
    }
    void SetCameraDirection()
    {
        //カメラの方向

        
        if (Mathf.Atan2(positionY, Mathf.Sqrt(positionX * positionX + positionZ * positionZ)) * Mathf.Rad2Deg < 45f)
        {
            //下を向きすぎないように調整
            directionX = Mathf.Atan2(positionY, Mathf.Sqrt(positionX * positionX + positionZ * positionZ)) * Mathf.Rad2Deg;
        }
        else
        {
            directionX = 45f;
        }
        directionY = -90f - Mathf.Atan2(positionZ, positionX) * Mathf.Rad2Deg;
        
        directionZ = 0f;
    }
}
