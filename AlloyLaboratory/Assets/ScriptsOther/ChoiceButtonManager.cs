using UnityEngine;
using UnityEngine.UI;

public class ChoiceButtonManager : MonoBehaviour
{
    TextPanelManager textPanelManager;
    public GameObject choicePanel;
    ChoicePanelManager choicePanelManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        choicePanelManager = choicePanel.GetComponent<ChoicePanelManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Close()
    {
        choicePanelManager.gameObject.SetActive(false);
    }
}
