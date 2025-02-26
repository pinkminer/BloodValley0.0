using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionManager : Singleton<TransitionManager>
{
    // public string startScene = "BloodScabValleyIndoor_3";
    public string startScene = "TestScene_1";
    private void Start()
    {
        StartCoroutine(TransitionToScene(string.Empty,startScene));
    }
    public void Transition(string from, string to){
        StartCoroutine(TransitionToScene(from,to));
    }

    private IEnumerator TransitionToScene(string from, string to){
        
        
        if(from!=string.Empty){
            EventHandler.CallBeforeSceneUnloadEvent();
            // 卸载不需要的场景
            yield return SceneManager.UnloadSceneAsync(from);
        }
        // load需要的场景 
        yield return SceneManager.LoadSceneAsync(to,LoadSceneMode.Additive);
        // 根据序号找到新加载的场景，并将其设置为激活场景
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount-1);
        SceneManager.SetActiveScene(newScene);
        EventHandler.CallAfterSceneUnloadEvent();
    }
}
