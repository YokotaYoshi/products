using UnityEngine;



public class CollisionCensor : MonoBehaviour
{
    public Direction collisionDirection;
    //public GameObject player;
    PlayerController playerCnt;
    BoxScript boxCnt;
    BoxScript otherBoxCnt;
    EnemyChaseController enemyCnt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCnt = GetComponentInParent<PlayerController>();
        boxCnt = GetComponentInParent<BoxScript>();
        enemyCnt = GetComponentInParent<EnemyChaseController>();
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
                    default:
                        break;
                }
            }
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
                    default:
                        break;
                }
            }
            if (enemyCnt != null)
            {
                switch (collisionDirection)
                {
                    case Direction.Up:
                        enemyCnt.isCollisionUp = true;
                        break;
                    case Direction.Down:
                        enemyCnt.isCollisionDown = true;
                        break;
                    case Direction.Right:
                        enemyCnt.isCollisionRight = true;
                        break;
                    case Direction.Left:
                        enemyCnt.isCollisionLeft = true;
                        break;
                    default:
                        break;
                }
            }
        }
        if (other.gameObject.tag == "Carry")
        {
            otherBoxCnt = other.GetComponent<BoxScript>();
            //接触した箱が動けるかどうか
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
                    default:
                        break;
                }
            }

            if (playerCnt != null)
            {
                switch (collisionDirection)
                {
                    case Direction.Up:
                        if (otherBoxCnt.isCollisionUp)
                        {
                            playerCnt.isCollisionUp = true;
                        }
                        break;
                    case Direction.Down:
                        if (otherBoxCnt.isCollisionDown)
                        {
                            playerCnt.isCollisionDown = true;
                        }
                        break;
                    case Direction.Right:
                        if (otherBoxCnt.isCollisionRight)
                        {
                            playerCnt.isCollisionRight = true;
                        }
                        break;
                    case Direction.Left:
                        if (otherBoxCnt.isCollisionLeft)
                        {
                            playerCnt.isCollisionLeft = true;
                        }
                        break;
                    default:
                        break;
                }
            }

            if (enemyCnt != null)
            {
                switch (collisionDirection)
                {
                    case Direction.Up:
                        enemyCnt.isCollisionUp = true;
                        break;
                    case Direction.Down:
                        enemyCnt.isCollisionDown = true;
                        break;
                    case Direction.Right:
                        enemyCnt.isCollisionRight = true;
                        break;
                    case Direction.Left:
                        enemyCnt.isCollisionLeft = true;
                        break;
                    default:
                        break;
                }
            }
        }
        if (other.gameObject.tag == "Hole")
        {
            //プレイヤーだけ通れない
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
                    default:
                        break;
                }
            }
            if (enemyCnt != null)
            {
                switch (collisionDirection)
                {
                    case Direction.Up:
                        enemyCnt.isCollisionUp = true;
                        break;
                    case Direction.Down:
                        enemyCnt.isCollisionDown = true;
                        break;
                    case Direction.Right:
                        enemyCnt.isCollisionRight = true;
                        break;
                    case Direction.Left:
                        enemyCnt.isCollisionLeft = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Untagged" || other.gameObject.tag == "Block" || other.gameObject.tag == "Carry" || other.gameObject.tag == "Hole")
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
                    default:
                        break;
                }
            }
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
                    default:
                        break;
                }
            }
            if (enemyCnt != null)
            {
                switch (collisionDirection)
                {
                    case Direction.Up:
                        enemyCnt.isCollisionUp = false;
                        break;
                    case Direction.Down:
                        enemyCnt.isCollisionDown = false;
                        break;
                    case Direction.Right:
                        enemyCnt.isCollisionRight = false;
                        break;
                    case Direction.Left:
                        enemyCnt.isCollisionLeft = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
