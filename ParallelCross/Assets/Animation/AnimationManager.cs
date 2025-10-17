using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AnimationManager : MonoBehaviour
{

    //------------------アニメーション関係------------------------
    //public CharaName charaName;
    public bool isPlayer;
    public Direction moveDirection = Direction.Down;
    
    
    Rigidbody2D rb2d;
    Animator[] animators;
    
    string currentAnime = "";
    string preAnime = "";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
        animators = GetComponentsInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.gameState == GameState.Pause) return;

        ChangeAnimation();

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
        for (int i = 0; i < animators.Length; i++)
        {
            switch (moveDirection)
            {
                case Direction.Down:
                    animators[i].Play("animWalkDown");
                    break;
                case Direction.Up:
                    animators[i].Play("animWalkUp");
                    break;
                case Direction.Right:
                    animators[i].Play("animWalkRight");
                    break;
                case Direction.Left:
                    animators[i].Play("animWalkLeft");
                    break;
            }
        }
        yield return new WaitForSeconds(1f);
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
