using UnityEngine;

public enum PlayerCollisionDirection
{
    Up,
    Down,
    Right,
    Left,
}

public class PlayerCollisionCensor : MonoBehaviour
{
    public PlayerCollisionDirection playerCollisionDirection;
    public GameObject player;
    PlayerController playerCnt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCnt = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Untagged" || other.gameObject.tag == "Block")
        {
            switch (playerCollisionDirection)
            {
                case PlayerCollisionDirection.Up:
                    playerCnt.isCollisionUp = true;
                    break;
                case PlayerCollisionDirection.Down:
                    playerCnt.isCollisionDown = true;
                    break;
                case PlayerCollisionDirection.Right:
                    playerCnt.isCollisionRight = true;
                    break;
                case PlayerCollisionDirection.Left:
                    playerCnt.isCollisionLeft = true;
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Untagged" || other.gameObject.tag == "Block")
        {
            switch (playerCollisionDirection)
            {
                case PlayerCollisionDirection.Up:
                    playerCnt.isCollisionUp = false;
                    break;
                case PlayerCollisionDirection.Down:
                    playerCnt.isCollisionDown = false;
                    break;
                case PlayerCollisionDirection.Right:
                    playerCnt.isCollisionRight = false;
                    break;
                case PlayerCollisionDirection.Left:
                    playerCnt.isCollisionLeft = false;
                    break;
            }
        }
    }
}
