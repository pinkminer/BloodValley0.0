using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // 存储场景中物品的状态
    public Dictionary<string,bool> itemAvailableDict = new Dictionary<string, bool>();


    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
    }

  

    private void OnDisable() {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
    }

  

    private void OnBeforeSceneUnloadEvent()
    {
        //找到场景中带有Item tag的物体
        foreach(var item in GameObject.FindGameObjectsWithTag("Item")){
            // 如果不包含此物体
            if(!itemAvailableDict.ContainsKey(item.name)){
                // 字典中加入
                itemAvailableDict.Add(item.name,true);
                Debug.Log("字典加入"+item.name);
            }
        }

    }
    
    private void OnAfterSceneUnloadEvent()
    {
        //找到场景中带有Item tag的物体
        foreach(var item in GameObject.FindGameObjectsWithTag("Item")){
            // 如果不包含此物体
            if(!itemAvailableDict.ContainsKey(item.name)){
                // 字典中加入
                itemAvailableDict.Add(item.name,true);
                Debug.Log("字典加入"+item.name);
            }else{
                item.gameObject.SetActive(itemAvailableDict[item.name]);
                Debug.Log("设置active:"+item.name+ ","+itemAvailableDict[item.name]);
            }
        }
    }

    private void OnUpdateUIEvent(string itemName,int arg2){
        if(itemName != null){
            itemAvailableDict[itemName] = false;
            Debug.Log("设置不可见"+itemName);
        }
    }
}
