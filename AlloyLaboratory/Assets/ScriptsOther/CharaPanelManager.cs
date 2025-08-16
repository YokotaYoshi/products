using UnityEngine;
using UnityEngine.UI;

public class CharaPanelManager : MonoBehaviour
{
    public Image charaImage0;
    public Image charaImage1;
    public Image charaImage2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Data.LoadMember();
    }

    // Update is called once per frame
    void Update()
    {
        charaImage0.sprite = Data.charaImage0;
        charaImage1.sprite = Data.charaImage1;
        charaImage2.sprite = Data.charaImage2;

        //DataのcharaDataに応じて表示するキャラを設定
    }
}
