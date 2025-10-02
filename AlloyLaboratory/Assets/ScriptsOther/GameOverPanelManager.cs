using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverPanelManager : MonoBehaviour
{
    public GameObject blackCurtain;
    float fadeOutTime = 1.0f;
    Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (blackCurtain != null)
        {
            image = blackCurtain.GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.inputType == InputType.Action || InputManager.inputType == InputType.Back)
        {
            StartCoroutine(LoadTitleScene());
        }
    }

    IEnumerator LoadTitleScene()
    {
        float time = 0.0f;
        while (true)
        {
            yield return null;
            time += Time.deltaTime;
            image.color = new Color(0f, 0f, 0f, time / fadeOutTime);
            if (time >= fadeOutTime) break;
        }
        SceneManager.LoadScene("TitleScene");
    }
}
