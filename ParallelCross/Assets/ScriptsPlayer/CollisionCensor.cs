using UnityEngine;



public class CollisionCensor : MonoBehaviour
{
    public Direction collisionDirection;
    //public GameObject player;
    PlayerController playerCnt;
    BoxScript boxCnt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCnt = GetComponentInParent<PlayerController>();
        boxCnt = GetComponentInParent<BoxScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Untagged" || other.gameObject.tag == "Block")
        {
            if (playerCnt != null)
            {
                switch (collisionDirection)
                {
                    case Direction.Up:
                        playerCnt.isCollisionUp = true;
                        break;
                    case Direction.Down:
                        playerCnt.isCollisionDown = true;
                        break;
                    case Direction.Right:
                        playerCnt.isCollisionRight = true;
                        break;
                    case Direction.Left:
                        playerCnt.isCollisionLeft = true;
                        break;
                }
            }
        }
        if (other.gameObject.tag == "Untagged" || other.gameObject.tag == "Block" || other.gameObject.tag == "Carry")
        {
            if (boxCnt != null)
            {
                switch (collisionDirection)
                {
                    case Direction.Up:
                        boxCnt.isCollisionUp = true;
                        break;
                    case Direction.Down:
                        boxCnt.isCollisionDown = true;
                        break;
                    case Direction.Right:
                        boxCnt.isCollisionRight = true;
                        break;
                    case Direction.Left:
                        boxCnt.isCollisionLeft = true;
                        break;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Untagged" || other.gameObject.tag == "Block")
        {
            if (playerCnt != null)
            {
                switch (collisionDirection)
                {
                    case Direction.Up:
                        playerCnt.isCollisionUp = false;
                        break;
                    case Direction.Down:
                        playerCnt.isCollisionDown = false;
                        break;
                    case Direction.Right:
                        playerCnt.isCollisionRight = false;
                        break;
                    case Direction.Left:
                        playerCnt.isCollisionLeft = false;
                        break;
                }
            }
        }
        if (other.gameObject.tag == "Untagged" || other.gameObject.tag == "Block" || other.gameObject.tag == "Carry")
        {
            if (boxCnt != null)
            {
                switch (collisionDirection)
                {
                    case Direction.Up:
                        boxCnt.isCollisionUp = false;
                        break;
                    case Direction.Down:
                        boxCnt.isCollisionDown = false;
                        break;
                    case Direction.Right:
                        boxCnt.isCollisionRight = false;
                        break;
                    case Direction.Left:
                        boxCnt.isCollisionLeft = false;
                        break;
                }
            }
        }
    }
}
