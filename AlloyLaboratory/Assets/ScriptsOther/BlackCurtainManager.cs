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

public class BlackCurtainManager : MonoBehaviour
{
    //public bool isActiveOnStart;
    public Brightness brightness = Brightness.Dark;
    public bool isBrightStart = true;//スタート時に明るくするかどうか
    public float fadeInTime = 0.2f;//暗闇が完全に晴れるまでの時間
    
    public float fadeOutTime = 0.3f;//暗転時間
    
    Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        //StartCoroutine(FadeIn());
        if (isBrightStart)
        {
            FadeIn();
            
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (brightness == Brightness.Middle) return;//フェードイン中かアウト中
    }

    public void FadeIn()
    {
        if (brightness == Brightness.Dark)
        {
            StartCoroutine(FadeInCoroutine());
        }
    }

    IEnumerator FadeInCoroutine()
    {
        float time = 0f;
        brightness = Brightness.Middle;
        //明るくなる

        while (true)
        {
            time += Time.deltaTime;
            image.color = new Color(0f, 0f, 0f, 1f - time / fadeInTime);

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

    public void FadeOut()
    {
        if (brightness == Brightness.Bright)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    public IEnumerator FadeOutCoroutine()
    {
        float time = 0f;
        brightness = Brightness.Middle;
        //暗くなる

        while (true)
        {
            time += Time.deltaTime;
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
}
