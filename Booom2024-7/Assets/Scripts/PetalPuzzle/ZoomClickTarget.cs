using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomClickTarget : MonoBehaviour
{
    //1、点击此对象后，所进入的画面大小   
    public float zoomedSize;
    //2、缩放速度
    public float inSpeed;  //点击此对象后，画面推进的速度
    public float outSpeed;   //从【此对象可被点击画面】返回到上一层画面的速度
    //3、缩放焦点
    public Vector3 focusPoint;   //点击此对象推进镜头时的焦点


}
