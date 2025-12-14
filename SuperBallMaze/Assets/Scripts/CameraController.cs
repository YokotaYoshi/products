using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Front,
    Back,
    Right,
    Left,
}

public class CameraController : MonoBehaviour
{
    //ゴールと球を2:1に外分する点
    //視点は常に中央向き→十字で切り替えられるように
    Direction direction = Direction.Front;
    float positionX;
    float positionY;
    float positionZ;
    Vector2 playerPositionXZ;
    float cameraAngle;
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Direction.Front;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Direction.Back;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Direction.Right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Direction.Left;
        }

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
        //カメラの位置について
        playerPositionXZ = new Vector2(player.transform.position.x, player.transform.position.z);//上から見た2次元座標
        
        
        switch (direction)
        {
            case Direction.Front://X,Zはプレイヤー後方一定の位置
                positionX = playerPositionXZ.x + playerPositionXZ.normalized.x * 10.0f;
                positionZ = playerPositionXZ.y + playerPositionXZ.normalized.y * 10.0f;
                break;
            case Direction.Back:
                positionX = playerPositionXZ.x - playerPositionXZ.normalized.x * 10.0f;
                positionZ = playerPositionXZ.y - playerPositionXZ.normalized.y * 10.0f;
                break;
            case Direction.Right:
                positionX = playerPositionXZ.x - playerPositionXZ.normalized.y * 10.0f;
                positionZ = playerPositionXZ.y + playerPositionXZ.normalized.x * 10.0f;
                break;
            case Direction.Left:
                positionX = playerPositionXZ.x + playerPositionXZ.normalized.y * 10.0f;
                positionZ = playerPositionXZ.y - playerPositionXZ.normalized.x * 10.0f;
                break;
        }
        
        //Yはプレイヤーが高く飛んだ時、見下ろせる位置に
        if (player.transform.position.y > 5f)
        {
            positionY = player.transform.position.y + 1.0f;
        }
        else
        {
            positionY = 6f;
        }
        
    }
    void SetCameraDirection()
    {
        //カメラの方向
        //原点からカメラに引いた直線が円盤ステージとなす角
        cameraAngle = Mathf.Atan2(positionY, Mathf.Sqrt(positionX * positionX + positionZ * positionZ)) * Mathf.Rad2Deg;

        //上下
        if (cameraAngle < 45f)
        {
            //下を向きすぎないように調整
            directionX = cameraAngle;
        }
        else
        {
            directionX = 45f;
        }

        switch (direction)
        {
            case Direction.Front:
                directionY = -90f - Mathf.Atan2(playerPositionXZ.y, playerPositionXZ.x) * Mathf.Rad2Deg;//横回転、中心を見るように移動
                break;
            case Direction.Back:
                directionY = 90f - Mathf.Atan2(playerPositionXZ.y, playerPositionXZ.x) * Mathf.Rad2Deg;//横回転、中心から見るように移動
                break;
            case Direction.Right:
                directionY = 180f - Mathf.Atan2(playerPositionXZ.y, playerPositionXZ.x) * Mathf.Rad2Deg;//横回転、右から見るように移動
                break;
            case Direction.Left:
                directionY = - Mathf.Atan2(playerPositionXZ.y, playerPositionXZ.x) * Mathf.Rad2Deg;//横回転、左から見るように移動
                break;
        }
        
        
        directionZ = 0f;//斜めになることはない。ギミックで斜めにする=みづらいペナルティを与えたっていいかも
    }
}
