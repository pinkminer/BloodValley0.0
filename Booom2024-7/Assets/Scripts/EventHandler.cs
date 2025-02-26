using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent(){
        BeforeSceneUnloadEvent.Invoke();
    }

    public static event Action AfterSceneUnloadEvent;
    public static void CallAfterSceneUnloadEvent(){
        AfterSceneUnloadEvent.Invoke();
    }

    public static event Action<string,int> UpdateUIEvent;
    public static void CallUpdateUIEvent(string itemName,int arg2){
        UpdateUIEvent.Invoke(itemName,arg2);
    }

}
