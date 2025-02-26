using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameWindow : MonoBehaviour
{
    /// <summary>
    /// 几个背包格的y轴坐标
    /// </summary>
    /// <value>pos_y</value>
    [SerializeField]
    int[] pos_y=new int[5] {175,45,-90,-225,0};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool setPosition(int num){
        if(num>=0&&num<5){
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector3 pos = rectTransform.localPosition;
            pos.y = pos_y[num];
            rectTransform.localPosition=pos;
            return true;
        }
        
        return false;
    }

    public bool setText(string name){
        if(name.Length<10){
            Transform text= transform.Find("ItemName");
            text.gameObject.GetComponent<TextMeshPro>().text = "<margin=1em>"+name;
            return true;
        }
        return false;
    }

    public bool setAvilable(bool act){
        if(act){
            
        }
        return false;
    }


}
