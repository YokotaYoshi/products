using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextBreakAnimator : MonoBehaviour
{
    private TMP_Text textComponent;

    
    
    [SerializeField]
    private bool isLoop = true;

    [SerializeField]
    private float maxTime = 1f;
    private float time = 0f;//アニメーションの開始と終了を0f~1fの間で制御したい。シンプルだから
    private bool isPlaying = false;

    [SerializeField]
    private Gradient gradientColor;
    private RangeInt charaIndexRange = new RangeInt(0, 0);
    Vector3 mousePosition;

    void Start()
    {
        this.textComponent = GetComponent<TMP_Text>();
        StartCoroutine(PopUpAnimation());
    }


    // Update is called once per frame
    void Update()
    {
        
        //時間制御
        if (this.isPlaying || this.isLoop)
        {
            this.time += Time.deltaTime;//時間計測

            if (this.isLoop)
            {
                //ループするなら、maxTimeの次0
                if (this.time >= this.maxTime) this.time -= this.maxTime;
            }
            else
            {
                //ループしないなら、maxTimeでストップ
                if (this.time >= this.maxTime)
                {
                    this.time = this.maxTime;
                    this.isPlaying = false;

                    SceneManager.LoadScene("TitleScene");
                }
            }

            UpdateAnimation(this.time / this.maxTime);//再生中は毎フレーム実行
        }
    }

    private IEnumerator PopUpAnimation()
    {
        //ゲームオーバー直後にバンっと出てくるアニメーション


        float popUpTime = 0.3f;

        //ジオメトリ情報を初期化
        this.textComponent.ForceMeshUpdate(true);//テキスト頂点情報を最新のものに更新
        var textInfo = this.textComponent.textInfo;//textInfoから文字の情報などにアクセスできるように

        //文字が画面中央にあるときの各頂点の座標を保存
        //1メッシュ4頂点とは限らない
        //全頂点数を合計してから配列を作る
        int totalVertexCount = 0;
        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            totalVertexCount += textInfo.meshInfo[i].vertices.Length;//各文字の頂点数を足し上げていく
        }

        Vector3[] vertexPos = new Vector3[totalVertexCount];//配列を初期化

        int index = 0;

        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            
            for (int j = 0; j < meshInfo.vertices.Length; ++j)
            {
                vertexPos[index++] = meshInfo.vertices[j];
            }       
            //i++:iを使った後に+1する。++i:iを使う前に+1する  
        }

        //初期座標を設定
        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];

            for (int j = 0; j < meshInfo.vertices.Length; ++j)
            {
                meshInfo.vertices[j] += new Vector3(300f, 300f, 0f) ;
            }            
        }

        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);

        }

        //メッシュの初期座標を取得
        //varは右側の型を勝手に解釈してくれる。intとかstringとか

        while (true)
        {
            popUpTime -= Time.deltaTime;

            //最初の動きは大きく
            if (popUpTime >= 0.2f)
            {
                for (int i = 0; i < textInfo.meshInfo.Length; ++i)
                {
                    var meshInfo = textInfo.meshInfo[i];

                    for (int j = 0; j < meshInfo.vertices.Length; ++j)
                    {
                        meshInfo.vertices[j] += 
                        new Vector3(-6000f * Time.deltaTime, -6000f * Time.deltaTime, 0f * Time.deltaTime) ;
                    }            
                }
            }
            else if (popUpTime >= 0.1f)
            {
                //少しずつ動きを落ち着かせる
                for (int i = 0; i < textInfo.meshInfo.Length; ++i)
                {
                    var meshInfo = textInfo.meshInfo[i];

                    for (int j = 0; j < meshInfo.vertices.Length; ++j)
                    {
                        meshInfo.vertices[j] += 
                        new Vector3(1000f * Time.deltaTime, 6000f * Time.deltaTime, 0f * Time.deltaTime) ;
                    }            
                }
            }
            else if (popUpTime >= 0f)
            {
                //少しずつ動きを落ち着かせる
                for (int i = 0; i < textInfo.meshInfo.Length; ++i)
                {
                    var meshInfo = textInfo.meshInfo[i];

                    for (int j = 0; j < meshInfo.vertices.Length; ++j)
                    {
                        meshInfo.vertices[j] += 
                        new Vector3(2000f * Time.deltaTime, -3000f * Time.deltaTime, 0f * Time.deltaTime) ;
                    }            
                }
            }
            

            //ジオメトリ情報の更新
            for (int i = 0; i < textInfo.meshInfo.Length; ++i)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                //textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
                textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);

            }

            yield return null;

            if (popUpTime <= 0f)
            {
                //最初の位置に戻す
                index = 0;
                for (int i = 0; i < textInfo.meshInfo.Length; ++i)
                {
                    var meshInfo = textInfo.meshInfo[i];
                    
                    for (int j = 0; j < meshInfo.vertices.Length; ++j)
                    {
                        meshInfo.vertices[j] = vertexPos[index++];
                    }           
                }

                //ジオメトリ情報の更新
                for (int i = 0; i < textInfo.meshInfo.Length; ++i)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    //textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
                    textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);

                }
                
                yield break;
            }
        }
    }

    private void UpdateAnimation(float time)
    {
        //ジオメトリ情報を初期化
        this.textComponent.ForceMeshUpdate(true);//テキスト頂点情報を最新のものに更新
        var textInfo = this.textComponent.textInfo;//textInfoから文字の情報などにアクセスできるように

        //簡易的なテキスト上昇アニメーション
        /*
        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];

            for (int j = 0; j < meshInfo.vertices.Length; ++j)
            {
                meshInfo.vertices[j] += new Vector3(0f, time * 50f, 0f);
            }            
        }
        */

        //テキストに含まれている全文字を順番に処理
        //textInfo.~.Lengthは<link=break>みたいなのもすべてひっくるめた文字の数
        for (int i = 0; i < textInfo.characterInfo.Length; ++i)
        {
            /*
            //文字情報インデックスの範囲指定
            if (this.charaIndexRange.start <= i && i <= this.charaIndexRange.end){以下をこの中に入れる}
            */

            //文字情報・メッシュ情報の取得
            var charaInfo = textInfo.characterInfo[i];//入力文字のi番目の文字について
            if (!charaInfo.isVisible) continue;//空白なら処理をスキップ

            //文字があればそれについて頂点データを操作する準備をする

            //各文字は四角形のポリゴンでできていて、4頂点を持つ
            int materialIndex = charaInfo.materialReferenceIndex;//メッシュ情報配列の要素番号
            //複数のマテリアルを使っているとこの値が変化する。bible.p113
            int vertexIndex = charaInfo.vertexIndex;//頂点配列の要素番号。vertexIndex~vetexIndex+3で1文字の4頂点
            var meshInfo = textInfo.meshInfo[materialIndex];

            /*
            //頂点情報の編集
            float t = 100f * Mathf.Clamp01(time * 4.0f - i * 0.2f);//文字の配列の要素番号iごとにタイムをずらす
            //メッシュの4頂点をそれぞれt上昇させる
            meshInfo.vertices[vertexIndex + 0] += new Vector3(0f, t, 0f);
            meshInfo.vertices[vertexIndex + 1] += new Vector3(0f, t, 0f);
            meshInfo.vertices[vertexIndex + 2] += new Vector3(0f, t, 0f);
            meshInfo.vertices[vertexIndex + 3] += new Vector3(0f, t, 0f);
            */

            //Debug.Log(vertexIndex);
            //頂点情報の編集2
            Vector3 vertex0 = meshInfo.vertices[vertexIndex + 0];
            Vector3 vertex1 = meshInfo.vertices[vertexIndex + 1];
            Vector3 vertex2 = meshInfo.vertices[vertexIndex + 2];
            Vector3 vertex3 = meshInfo.vertices[vertexIndex + 3];

            //パーリンノイズ生成
            //引数にiをいれて文字毎のノイズを生成
            //2倍して1引くことで0~1になる戻り値を-1~1になるよう調整
            Vector3 rotationNoise = new Vector3(
                Mathf.PerlinNoise(i * 0.1f, 0.4f) * 2.0f - 1.0f,
                Mathf.PerlinNoise(i * 0.2f, 0.5f) * 2.0f - 1.0f,
                Mathf.PerlinNoise(i * 0.3f, 0.6f) * 2.0f - 1.0f
            );

            Vector3 positionNoise = new Vector3(
                Mathf.PerlinNoise(i * 0.7f, 0.1f) * 2.0f - 1.0f,
                Mathf.PerlinNoise(i * 0.8f, 0.2f) * 2.0f - 1.0f,
                Mathf.PerlinNoise(i * 0.9f, 0.3f) * 2.0f - 1.0f
            );

            //回転
            //回転行列を用いて頂点の回転を計算
            //中心座標を求める。原点は画面中央
            var center = Vector3.Scale(vertex2 - vertex0, Vector3.one * 0.5f) + vertex0;
            //ノイズ生成したオイラー角を徐々に360度回転させる形で生成
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(rotationNoise * 360f * time));
            //各頂点をMultiplyPoint関数で行列ベクトル積を行う
            //積計算をする前にポリゴンを中心とした頂点座標を算出し、積計算後に元の座標に戻している
            vertex0 = matrix.MultiplyPoint(vertex0 - center) + center;
            vertex1 = matrix.MultiplyPoint(vertex1 - center) + center;
            vertex2 = matrix.MultiplyPoint(vertex2 - center) + center;
            vertex3 = matrix.MultiplyPoint(vertex3 - center) + center;

            //移動
            
            
            //おおまかにマウスの入力ポジションからみた文字の方向に飛んでいくようにしたい
            Vector3 direction = new Vector3(center.x - mousePosition.x, center.y - mousePosition.y, 0f).normalized;
            //Debug.Log(i);
            //Debug.Log(direction);
            //移動量調整
            direction = direction * time * 500f;
            

            //ノイズ生成した移動方向に徐々に移動するpositionNoiseを計算
            positionNoise = positionNoise * 300f * time;

            //すべての頂点にノイズを加算
            vertex0 += positionNoise + direction;
            vertex1 += positionNoise + direction;
            vertex2 += positionNoise + direction;
            vertex3 += positionNoise + direction;

            //代入
            meshInfo.vertices[vertexIndex + 0] = vertex0; 
            meshInfo.vertices[vertexIndex + 1] = vertex1;
            meshInfo.vertices[vertexIndex + 2] = vertex2;
            meshInfo.vertices[vertexIndex + 3] = vertex3;

            //色・終盤のα値が0になるように設定
            Color color = this.gradientColor.Evaluate(time);
            meshInfo.colors32[vertexIndex + 0] *= color;
            meshInfo.colors32[vertexIndex + 1] *= color;
            meshInfo.colors32[vertexIndex + 2] *= color;
            meshInfo.colors32[vertexIndex + 3] *= color;
            //Debug.Log(color);
        }
        //文字情報インデックスの範囲指定
        
        
        //ジオメトリ情報の更新
        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
            textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);

        }
    }

    //アニメーション再生開始
    public void StartAnimation(float positionX, float positionY)
    {
        //画面クリックでアニメーション再生
        this.isPlaying = true;
        this.time = 0f;
        //mousePositionを、左下原点になっているのを画面中央原点に修正
        mousePosition.x = positionX - 640f;
        mousePosition.y = positionY - 480f;
    }

    /*
    //アニメーション再生開始
    public void OnTouchLink(float positionX, float positionY)
    {
        //画面クリックでアニメーション再生
        this.isPlaying = true;
        this.time = 0f;
        //mousePositionを、左下原点になっているのを画面中央原点に修正
        mousePosition.x = positionX - 640f;
        mousePosition.y = positionY - 480f;
    }
    
    public void OnTouchLink(string linkID, string linkText, 
    int linkTextFirstCharaIndex, int linkIndex)
    {
        if (linkID == "break")
        {
            //<link=break></link>で囲われたテキストをクリック7するとアニメーション再生
            this.isPlaying = true;
            this.time = 0f;
            this.charaIndexRange.start = linkTextFirstCharaIndex;
            this.charaIndexRange.length = linkText.Length;
        }
    }
    */
}
