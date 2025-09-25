using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BlackCurtainManager : MonoBehaviour
{
    //public bool isActiveOnStart;
    public bool isBrightStart = true;//スタート時に明るくするかどうか
    public float fadeInTime = 0.2f;//暗闇が完全に晴れるまでの時間
    bool isBright = false;
    public float fadeOutTime = 0.3f;//暗転時間
    bool isDark = true;
    Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        //StartCoroutine(FadeIn());
        if (isBrightStart) FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBright && !isDark) return;//フェードイン中かアウト中
    }

    public void FadeIn()
    {
        if (isDark)
        {
            StartCoroutine(FadeInCoroutine());
        }
    }

    IEnumerator FadeInCoroutine()
    {
        float time = 0f;
        isDark = false;
        isBright = false;
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
        isBright = true;
    }

    public void FadeOut()
    {
        if (isBright)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    public IEnumerator FadeOutCoroutine()
    {
        float time = 0f;
        isBright = false;
        isDark = false;
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
        isDark = true;
    }
}
