using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AnimationManager : MonoBehaviour
{

    //------------------アニメーション関係------------------------
    public CharaName charaName;
    public MoveDirection moveDirection = MoveDirection.Down;
    Rigidbody2D rb2d;
    Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == GameState.Pause) return;
        Animation();
    }

    void Animation()
    {
        if (charaName == CharaName.Rino)
        {
            if (rb2d.linearVelocity.y < 0f)
            {
                animator.Play("animRinoWalkDown");
                moveDirection = MoveDirection.Down;
            }
            else if (rb2d.linearVelocity.y > 0f)
            {
                animator.Play("animRinoWalkUp");
                moveDirection = MoveDirection.Up;
            }
            else if (rb2d.linearVelocity.x > 0f)
            {
                animator.Play("animRinoWalkRight");
                moveDirection = MoveDirection.Right;
            }
            else if (rb2d.linearVelocity.x < 0f)
            {
                animator.Play("animRinoWalkLeft");
                moveDirection = MoveDirection.Left;
            }
            else //動いていないとき
            {
                if (Input.GetAxisRaw("Vertical") == -1.0f)
                    moveDirection = MoveDirection.Down;
                else if (Input.GetAxisRaw("Vertical") == 1.0f)
                    moveDirection = MoveDirection.Up;
                else if (Input.GetAxisRaw("Horizontal") == 1.0f)
                    moveDirection = MoveDirection.Right;
                else if (Input.GetAxisRaw("Horizontal") == -1.0f)
                    moveDirection = MoveDirection.Left;

                if (moveDirection == MoveDirection.Down)
                    animator.Play("animRinoStayDown");
                else if (moveDirection == MoveDirection.Up)
                    animator.Play("animRinoStayUp");
                else if (moveDirection == MoveDirection.Right)
                    animator.Play("animRinoStayRight");
                else if (moveDirection == MoveDirection.Left)
                    animator.Play("animRinoStayLeft");
                
            }
        
        }
    }
}
