using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AnimationManager : MonoBehaviour
{

    //------------------アニメーション関係------------------------
    //public CharaName charaName;
    public bool isPlayer;
    public bool auto = true;//自動切り替え
    public Direction moveDirection = Direction.Down;


    Rigidbody2D rb2d;
    Animator animator;
    Animator[] animators;
    
    string currentAnime = "";
    string preAnime = "";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        animators = GetComponentsInChildren<Animator>();

        Debug.Log(moveDirection);
        for (int i = 0; i < animators.Length; i++)
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

    // Update is called once per frame
    void Update()
    {

        if (auto && GameManager.gameState == GameState.Pause) return;

        if (auto)
        {
            ChangeAnimation();
        }
        else
        {
            ManualAnimation();
        }

        if (currentAnime != preAnime)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].Play(currentAnime);
                preAnime = currentAnime;
            }
        }
        
    }
    
    void ChangeAnimation()
    {
        if (isPlayer)
        {
            if (Input.GetAxisRaw("Vertical") == -1.0f)
            {
                moveDirection = Direction.Down;
            }
            else if (Input.GetAxisRaw("Vertical") == 1.0f)
            {
                moveDirection = Direction.Up;
            }
            else if (Input.GetAxisRaw("Horizontal") == 1.0f)
            {
                moveDirection = Direction.Right;
            }
            else if (Input.GetAxisRaw("Vertical") == -1.0f)
            {
                moveDirection = Direction.Down;
            }


            if (rb2d.linearVelocity.y < -0.1f)
            {
                currentAnime = "animWalkDown";
                moveDirection = Direction.Down;
            }
            else if (rb2d.linearVelocity.y > 0.1f)
            {
                currentAnime = "animWalkUp";
                moveDirection = Direction.Up;
            }
            else if (rb2d.linearVelocity.x > 0.1f)
            {
                currentAnime = "animWalkRight";
                moveDirection = Direction.Right;
            }
            else if (rb2d.linearVelocity.x < -0.1f)
            {
                currentAnime = "animWalkLeft";
                moveDirection = Direction.Left;
            }
            else //動いていないとき
            {
                switch (moveDirection)
                {
                    case Direction.Up:
                        currentAnime = "animStayUp";
                        break;
                    case Direction.Down:
                        currentAnime = "animStayDown";
                        break;
                    case Direction.Right:
                        currentAnime = "animStayRight";
                        break;
                    case Direction.Left:
                        currentAnime = "animStayLeft";
                        break;
                }
            }
        }
        else
        {
            if (rb2d.linearVelocity.y < -0.1f)
            {
                currentAnime = "animWalkDown";
                moveDirection = Direction.Down;
            }
            else if (rb2d.linearVelocity.y > 0.1f)
            {
                currentAnime = "animWalkUp";
                moveDirection = Direction.Up;
            }
            else if (rb2d.linearVelocity.x > 0.1f)
            {
                currentAnime = "animWalkRight";
                moveDirection = Direction.Right;
            }
            else if (rb2d.linearVelocity.x < -0.1f)
            {
                currentAnime = "animWalkLeft";
                moveDirection = Direction.Left;
            }
            else //動いていないとき
            {
                switch (moveDirection)
                {
                    case Direction.Up:
                        currentAnime = "animStayUp";
                        break;
                    case Direction.Down:
                        currentAnime = "animStayDown";
                        break;
                    case Direction.Right:
                        currentAnime = "animStayRight";
                        break;
                    case Direction.Left:
                        currentAnime = "animStayLeft";
                        break;
                }
            }
        }
        
    }


    public IEnumerator ManualAnimation()
    {
        currentAnime = "";
        yield return null;
    }
    public void AnimDown()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].Play("animStayDown");
        }
    }
    public void AnimUp()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].Play("animStayUp");
        }
    }
    public void AnimRight()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].Play("animStayRight");
        }
    }
    public void AnimLeft()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].Play("animStayLeft");
        }
    }

    public void AnimStop()
    {
        animator.enabled = false;
    }
    public void AnimRestart()
    {
        animator.enabled = true;
    }

    public void AnimStopAll()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = false;
        }
    }
    public void AnimRestartAll()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = true;
        }
    }
    public IEnumerator WalkBack()
    {
        //1秒かけて1マス後ずさりするアニメーション
        //timeScaleに依存しないようVentor2.Lerpを使用
        Vector2 startPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 goalPosition = startPosition + Vector2.down;

        float time = 0f;

        while (true)
        {
            time += Time.unscaledDeltaTime;
            transform.position = Vector2.Lerp(startPosition, goalPosition, time);
            yield return null;
            if (time >= 1f) break;
        }

        transform.position = goalPosition;
    }
}
