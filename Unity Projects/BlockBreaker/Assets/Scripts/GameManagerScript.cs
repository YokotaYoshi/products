using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public Text gameoverText;
    public Text clearText;
    public Text restartText;
    
    public bool gameover = false;

    // Start is called before the first frame update
    void Start()
    {
        clearText.text = "";
        gameoverText.text = "";
        restartText.text = "";
        
        UpdateScore(0);
    }

    public void UpdateScore(int score)
    {
        if (score >= 10)
        {
            clearText.text = "CLEAR";
            restartText.text = "press SPACE to restart";
            gameover = true;
            
        }
    }

    public void Dead()
    {
        gameoverText.text ="GAMEOVER";
        restartText.text = "press SPACE to restart";
        gameover = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameover == true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                gameover = false;        
            }
        }
    }
}
