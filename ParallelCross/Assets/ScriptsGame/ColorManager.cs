using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour
{
    public bool willAppear = true;
    SpriteRenderer[] spriteRenderers;
    Animator[] animators;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        //アニメーターを一時的にストップさせる
        if (animators.Length > 0)
        {
            for (int i = 0; i < animators.Length; ++i)
            {
                animators[i].enabled = false;
            }
        }
        if (willAppear)
        {
            for (int i = 0; i < spriteRenderers.Length; ++i)
            {
                spriteRenderers[i].color = new Color(0f, 0f, 0f, 1f);
            }
        }
        else
        {
            for (int i = 0; i < spriteRenderers.Length; ++i)
            {
                spriteRenderers[i].color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Appear()
    {
        float time = 0f;
        Debug.Log(spriteRenderers.Length);

        while (true)
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            if (time >= 1f) break;
            for (int i = 0; i < spriteRenderers.Length; ++i)
            {
                spriteRenderers[i].color = new Color(time, time, time, 1f);
            }
            //Debug.Log(time);
        }
        for (int i = 0; i < spriteRenderers.Length; ++i)
        {
            spriteRenderers[i].color = new Color(1f, 1f, 1f, 1f);
        }
        if (animators.Length > 0)
        {
            for (int i = 0; i < animators.Length; ++i)
            {
                animators[i].enabled = true;
            }
        }
        //Debug.Log("出現");
    }
    
    public IEnumerator DisAppear()
    {
        float time = 0f;
        Debug.Log(spriteRenderers.Length);

        while (true)
        {
            yield return null;
            time += Time.unscaledDeltaTime;
            if (time >= 1f) break;
            for (int i = 0; i < spriteRenderers.Length; ++i)
            {
                spriteRenderers[i].color = new Color(1f - time, 1f - time, 1f - time, 1f);
            }
            //Debug.Log(time);
        }
        for (int i = 0; i < spriteRenderers.Length; ++i)
        {
            spriteRenderers[i].color = new Color(0f, 0f, 0f, 1f);
        }
        if (animators.Length > 0)
        {
            for (int i = 0; i < animators.Length; ++i)
            {
                animators[i].enabled = true;
            }
        }
        //Debug.Log("出現");
    }
}
