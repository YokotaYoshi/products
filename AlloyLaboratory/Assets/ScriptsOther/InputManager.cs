using UnityEngine;

public enum InputType
{
    Null,
    Action,
    Back,
}
public class InputManager : MonoBehaviour
{
    public static InputType inputType = InputType.Null;
    public GameObject InputPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (InputPanel != null)
        {
            InputPanel.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //入力があれば状態変化
        InputTypeManagement();
        if (inputType != InputType.Null) Debug.Log(inputType);
    }

    //------------------------入力切替メソッド-----------------------
    public void InputTypeManagement()
    {
        //入力を変数に変更
        //決定:左クリック、Z、Enter
        //戻る:右クリック、X、Space
        //ダッシュ:左右Shift
        //最初に入力をニュートラルに戻す
        inputType = InputType.Null;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        {
            inputType = InputType.Action;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(1))
        {
            inputType = InputType.Back;
        }
    }

    //----------------謎解きの答えを入力したりする入力パネル------------
    public void OpenInputPanel()
    {
        GameManager.gameState = GameState.Pause;
        InputPanel.SetActive(true);
    }
    public void CloseInputPanel()
    {
        InputPanel.SetActive(false);
    }
}
