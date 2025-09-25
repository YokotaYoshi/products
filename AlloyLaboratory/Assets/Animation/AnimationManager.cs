using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AnimationManager : MonoBehaviour
{

    //------------------アニメーション関係------------------------
    //public CharaName charaName;
    public bool isPlayer;
    public Direction moveDirection = Direction.Down;
    public Direction directionReference = Direction.N;
    bool isMoving;
    
    Rigidbody2D rb2d;
    Animator[] animators;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
        animators = GetComponentsInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(animators.Length);
        if (rb2d.linearVelocity.magnitude > 0f) isMoving = true;
        else isMoving = false;

        Debug.Log(isMoving);

        if (GameManager.gameState == GameState.Pause) return;

        //移動方向に応じてmoveDirection変更
        MoveDirection();
        //moveDirection != directionReferenceならアニメーション変更
        if (isPlayer)
        {
            Animation();
        }
        else if (moveDirection != directionReference)
        {
            Animation();
        }

        directionReference = moveDirection;
    }

    void MoveDirection()
    {
        if (isPlayer)
        {
            //プレイヤーの場合
            
            if (rb2d.linearVelocity.y < 0f)
            {
                moveDirection = Direction.Down;
            }
            else if (rb2d.linearVelocity.y > 0f)
            {
                moveDirection = Direction.Up;
            }
            else if (rb2d.linearVelocity.x > 0f)
            {
                moveDirection = Direction.Right;
            }
            else if (rb2d.linearVelocity.x < 0f)
            {
                moveDirection = Direction.Left;
            }
            else //動いていないとき
            {
                if (Input.GetAxisRaw("Vertical") == -1.0f)
                    moveDirection = Direction.Down;
                else if (Input.GetAxisRaw("Vertical") == 1.0f)
                    moveDirection = Direction.Up;
                else if (Input.GetAxisRaw("Horizontal") == 1.0f)
                    moveDirection = Direction.Right;
                else if (Input.GetAxisRaw("Horizontal") == -1.0f)
                    moveDirection = Direction.Left;
            }
        }
        else
        {
            //Debug.Log(rb2d.linearVelocity);
            
            if (rb2d.linearVelocity.y < 0f)
            {
                moveDirection = Direction.Down;
            }
            else if (rb2d.linearVelocity.y > 0f)
            {
                moveDirection = Direction.Up;
            }
            else if (rb2d.linearVelocity.x > 0f)
            {
                moveDirection = Direction.Right;
            }
            else if (rb2d.linearVelocity.x < 0f)
            {
                moveDirection = Direction.Left;
            }
        }
    }

    void Animation()
    {
        //プレイヤーのアニメーション
        for (int i = 0; i < animators.Length; i++)
        {
            if (isMoving)
            {
                switch (moveDirection)
                {
                    case (Direction.Down):
                        animators[i].Play("animWalkDown");
                        break;
                    case (Direction.Up):
                        animators[i].Play("animWalkUp");
                        break;
                    case (Direction.Right):
                        animators[i].Play("animWalkRight");
                        break;
                    case (Direction.Left):
                        animators[i].Play("animWalkLeft");
                        break;
                }
            }
            else
            {
                switch (moveDirection)
                {
                    case (Direction.Down):
                        animators[i].Play("animStayDown");
                        break;
                    case (Direction.Up):
                        animators[i].Play("animStayUp");
                        break;
                    case (Direction.Right):
                        animators[i].Play("animStayRight");
                        break;
                    case (Direction.Left):
                        animators[i].Play("animStayLeft");
                        break;
                }
            }
        }
    }
}
