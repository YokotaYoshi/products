using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyNormal : Enemy
{
    //float time = 0f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Enemyクラスを継承しているので、Enemyクラスで作ったpublicなものをそのまま使える
    //こっちでStartとUpdateを呼んでしまうと親で無効化される
    //オブジェクトにアタッチするのはこっちのスクリプトだけでOK
    //Update関数とかの中に処理を書かずになるべく別の関数使うのがよさそう
    //ここは変更するだけの場所
    

    public override IEnumerator Attack()//攻撃だけ変更する。overrideをつける
    {
        yield return null;
        base.Attack();
    }
}
