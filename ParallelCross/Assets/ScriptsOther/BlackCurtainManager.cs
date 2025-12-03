using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Brightness
{
    Dark,
    Middle,
    Bright
}

public enum StartColor
{
    Black,
    White,
    Custom,
}

public class BlackCurtainManager : MonoBehaviour
{
    //public bool isActiveOnStart;
    public Brightness brightness = Brightness.Dark;
    public bool isBrightStart = true;//スタート時に明るくするかどうか
    public StartColor startColor = StartColor.Black;
    public float fadeInTime = 0.2f;//暗闇が完全に晴れるまでの時間

    public float fadeOutTime = 0.3f;//暗転時間
    public float goalBrightness = 0f;//フェードイン時に最終的にこの明るさになる
    public float flashTime = 0.5f;//フラッシュ時間
    public GameObject flash;

    Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        image = GetComponent<Image>();
        switch (startColor)
        {
            case (StartColor.Black):
                image.color = new Color(0f, 0f, 0f, 1f);
                break;
            case (StartColor.White):
                image.color = new Color(1f, 1f, 1f, 1f);
                break;
            case (StartColor.Custom):
                break;
        }
    }
    void Start()
    {


        //StartCoroutine(FadeIn());
        if (isBrightStart)
        {
            switch (startColor)
            {
                case (StartColor.Black):
                    StartCoroutine(FadeIn());
                    break;
                case (StartColor.White):
                    StartCoroutine(WhiteIn());
                    break;
            }
        }

        if (flash != null)
        {
            flash.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (brightness == Brightness.Middle) return;//フェードイン中かアウト中

        if (GameManager.gameState == GameState.GameOver)
        {
            StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeIn()
    {
        if (brightness != Brightness.Dark) yield break;
        image.color = new Color(0f, 0f, 0f, 1f);
        
        float time = 0f;
        brightness = Brightness.Middle;
        yield return new WaitForSeconds(0.1f);
        //明るくなる

        while (true)
        {
            time += Time.deltaTime;
            image.color = new Color(0f, 0f, 0f, 1f - (1f - goalBrightness) * time / fadeInTime);

            if (time >= fadeInTime)
            {
                image.color = new Color(0, 0, 0, goalBrightness);
                break;
            }

            yield return null;
        }
        image.color = new Color(0, 0, 0, goalBrightness);
        brightness = Brightness.Bright;
    }

    public IEnumerator WhiteIn()
    {
        float time = 0f;
        brightness = Brightness.Middle;
        //明るくなる

        while (true)
        {
            time += Time.deltaTime;
            image.color = new Color(1f, 1f, 1f, 1f - time / fadeInTime);

            if (time >= fadeInTime)
            {
                image.color = new Color(0, 0, 0, 0f);
                break;
            }

            yield return null;
        }
        image.color = new Color(0, 0, 0, 0f);
        brightness = Brightness.Bright;
    }


    public IEnumerator FadeOut()
    {
        if (brightness != Brightness.Bright) yield break;
        float time = 0f;
        brightness = Brightness.Middle;
        //暗くなる

        while (true)
        {
            time += Time.unscaledDeltaTime;
            image.color = new Color(0, 0, 0, time / fadeOutTime);
            yield return null;

            if (time >= fadeOutTime)
            {
                image.color = new Color(0f, 0f, 0f, 1f);
                break;
            }
        }
        brightness = Brightness.Dark;
    }

    public IEnumerator WhiteOut()
    {
        float time = 0f;
        brightness = Brightness.Middle;
        //白くなる

        while (true)
        {
            time += Time.deltaTime;
            image.color = new Color(1, 1, 1, time / fadeOutTime);
            yield return null;

            if (time >= fadeOutTime)
            {
                image.color = new Color(1f, 1f, 1f, 1f);
                break;
            }
        }
        brightness = Brightness.Dark;
    }

    public IEnumerator BlackFlash()
    {
        //明るいタイミングで一瞬暗くする
        brightness = Brightness.Middle;
        image.color = new Color(0f, 0f, 0f, 1f);
        yield return new WaitForSeconds(flashTime);
        brightness = Brightness.Bright;
        image.color = new Color(0f, 0f, 0f, 0f);
    }


    public IEnumerator WhiteFlash()
    {
        //明るいタイミングで一瞬白くする
        brightness = Brightness.Middle;
        image.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(flashTime);
        brightness = Brightness.Dark;
        image.color = new Color(0f, 0f, 0f, 0f);
    }

    public IEnumerator BrightFlash()
    {
        //暗いタイミングで一瞬明るくする
        brightness = Brightness.Middle;
        image.color = new Color(0f, 0f, 0f, 0f);
        yield return new WaitForSeconds(flashTime);
        brightness = Brightness.Dark;
        image.color = new Color(0f, 0f, 0f, 1f);
    }

    public void Flash()
    {
        flash.SetActive(true);
    }
}