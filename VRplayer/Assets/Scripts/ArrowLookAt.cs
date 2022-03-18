using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLookAt : MonoBehaviour
{
    private GameObject target;    //目标  
    public Transform self;  //自己  

    public float direction; //箭头旋转的方向，或者说角度，只有正的值  
    public Vector3 u;   //叉乘结果，用于判断上述角度是正是负  

    float devValue = 10f;   //离屏边缘距离  
    float showWidth;    //由devValue计算出从中心到边缘的距离（宽和高）  
    float showHeight;

    Quaternion originRot;   //箭头原角度  

    // 初始化  
    void Start()
    {
        
        //showWidth = Screen.width / 2 - devValue;  
        //showHeight = Screen.height / 2 - devValue;  
    }

    void Update()
    {
        try
        {
            target = GameObject.FindGameObjectsWithTag("Player")[0];
            if (target != null)
            {
                transform.LookAt(target.transform);
            }
        }
        catch
        {
            throw;
        }
    }
}
