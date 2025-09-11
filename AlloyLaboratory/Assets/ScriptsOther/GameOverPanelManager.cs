using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.inputType == InputType.Action || InputManager.inputType == InputType.Back)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("TitleScene");
        }
    }
}
