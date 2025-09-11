using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BlackCurtainManager : MonoBehaviour
{
    //public bool isActiveOnStart;
    float fadeInTime = 0.2f;//暗闇が完全に晴れるまでの時間
    bool fadeIn = false;
    float fadeOutTime = 0.3f;
    bool fadeOut = false;
    Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        //StartCoroutine(FadeIn());
        StartFadeIn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartFadeIn()
    {
        if (!fadeIn)
        {
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float time = 0f;
        fadeIn = true;
        //明るくなる

        while (true)
        {
            time += Time.deltaTime;
            image.color -= new Color(0f, 0f, 0f, 5f * Time.deltaTime);

            if (time >= fadeInTime)
            {
                image.color = new Color(0, 0, 0, 0f);
                break;
            }

            yield return null;
        }
        fadeOut = false;
    }

    public void StartFadeOut()
    {
        if (!fadeOut)
        {
            StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeOut()
    {
        float time = 0f;
        fadeOut = true;
        //暗くなる

        while (true)
        {
            time += Time.deltaTime;
            image.color += new Color(0, 0, 0, 4f * Time.deltaTime);
            yield return null;

            if (time >= fadeOutTime)
            {
                image.color = new Color(0f, 0f, 0f, 1f);
                break;
            }
        }
        fadeIn = true;
    }
}
