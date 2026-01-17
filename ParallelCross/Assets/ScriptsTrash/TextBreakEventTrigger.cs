using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextBreakEventTrigger : MonoBehaviour
{
    private TMP_Text textComponent;
    private Camera targetCamera;//テキスト上でクリック判定などをするときに使うカメラ

    [SerializeField]
    //private UnityEvent<float, float> onClickLink;//リンクがクリックされたときに外部から設定できるイベント]

    bool isClickable = false;//入力を受け付けるかどうか
    bool isAlreadyClicked = false;//入力されたかどうか
    float waitTime = 0.3f;//入力を受け付けない時間
    TextBreakAnimator textBreakAnimator;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.textComponent = GetComponent<TMP_Text>();//テキストコンポーネント取得

        //カメラを取得する
        //オブジェクトを親方向にたどっていき、最初に見つかるCanvasを取得
        //ようはどのCanvasに属しているかを知りたい
        var rootCanvas = this.GetComponentInParent<Canvas>();
        if (rootCanvas != null)
        {
            //モードにおうじてカメラを設定
            switch (rootCanvas.renderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                    this.targetCamera = null; break;
                    //Overlayモードならカメラ不要なのでnull
                case RenderMode.ScreenSpaceCamera:
                    this.targetCamera = rootCanvas.worldCamera; break;
                case RenderMode.WorldSpace:
                    this.targetCamera = rootCanvas.worldCamera; break;
                    //Cameraモード、WorldSpaceモードならCanvasの持っているカメラ＝worldCameraを使う
            }
        }
        else
        {
            this.targetCamera = Camera.main;
        }

        textBreakAnimator = GetComponent<TextBreakAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (waitTime > 0f)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            //waitTime経過したらタイマーストップして入力を受け付けるように
            if (!isAlreadyClicked)
            {
                isClickable = true;
            }
        }

        //クリック座標を取得
        Vector3 touchPosition = Input.mousePosition;//マウス座標。毎フレーム取得
        bool touchDown = Input.GetMouseButtonDown(0);//クリックした一瞬だけtrue

        //スマホ対応
        /*
        if (0 < Input.touchCount)
        {
            //タップされている指の数が0より大きければ
            //シングルタップを前提としている
            Touch touchInfo = Input.GetTouch(0);
            touchPosition = touchInfo.position;//タップ座標
            touchDown = touchInfo.phase == TouchPhase.Began;
        }
        */

        //クリック判定
        if (touchDown && isClickable)
        {
            //Debug.Log(touchPosition);

            //アニメーションを再生
            textBreakAnimator.StartAnimation(touchPosition.x, touchPosition.y);
            //onClickLink.Invoke(touchPosition.x, touchPosition.y);

            isClickable = false;//アニメーション開始したらもうクリックを受け付けない
            isAlreadyClicked = true;

            /*
            //クリックした位置にある文字の情報
            //文字がない→-1、一塊目→0、二塊目→1
            //ようは文字をクリックしたかどうか
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(
                this.textComponent, touchPosition, this.targetCamera);

            //Debug.Log(linkIndex);

            if (0 <= linkIndex)
            {
                //より正確に、文字をクリックしたかどうか判定する
                int charIndex = TMP_TextUtilities.FindIntersectingCharacter(
                    this.textComponent, touchPosition, this.targetCamera, true
                );

                if (0 <= charIndex)
                {
                    TMP_LinkInfo linkInfo = this.textComponent.textInfo.linkInfo[linkIndex];

                    /*
                    Debug.Log($"Link Index: {linkIndex} with ID [{linkInfo.GetLinkID()}]" +
                        $"and Text \"{linkInfo.GetLinkText()}\" has been selected.");
                    */

                    /*
                    //アニメーションを再生
                    onClickLink.Invoke(linkInfo.GetLinkID(),
                    linkInfo.GetLinkText(),
                    linkInfo.linkTextfirstCharacterIndex,
                    linkIndex);
                    

                    //linkInfo.GetLinkID()→文字をクリックしたときにbreakの文字列を返す
                    //linkInfo.GetLinkText()→クリックした文字の塊
                    //linkInfo.linkTextfirstCharacterIndex→クリックした文字塊の最初の文字が、全体で何番目の文字か
                }
            }
            */
        }
        
    }
}
