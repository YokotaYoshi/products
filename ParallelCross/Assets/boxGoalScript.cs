using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fungus;

public class boxGoalScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //倉庫番のゴール
    //特定の箱をそれぞれ特定のゴールに置くもの
    //順番はどうでもよくて、すべてのゴールを埋めればいいもの
    //eventProgressSubを変更
    public Flowchart flowchart;
    public int goalID = 1;
    int boxID;
    int point;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Carry")
        {
            //箱が乗っている
            boxID = other.gameObject.GetComponent<BoxScript>().boxID;
            if (boxID == goalID)
            {
                point = flowchart.GetIntegerVariable("eventProgressSub");
                point += boxID;
                Debug.Log(point);
                flowchart.SetIntegerVariable("eventProgressSub", point);
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Carry" && boxID == goalID)
        {
            //箱が離れた
            point = flowchart.GetIntegerVariable("eventProgressSub");
            point -= boxID;
            Debug.Log(point);
            flowchart.SetIntegerVariable("eventProgressSub", point);
        }
    }
}
