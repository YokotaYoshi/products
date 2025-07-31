using UnityEngine;
using UnityEngine.UI;

public class ChoicePanelManager : MonoBehaviour
{
    //TextPanelManagerから命令されて、選択肢を表示する
    public GameObject choice1;
    public GameObject choice2;
    public int choiceNum;
    public string[] choices;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (choiceNum == 2)
        {
            choice1.SetActive(true);
            choice2.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
