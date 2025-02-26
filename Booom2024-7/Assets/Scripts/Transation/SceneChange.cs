using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//在scrennchange这个prefab上面挂载了，主要用于场景切换，包含了一个渐变特效
// 场景切换逻辑是按照1234的顺序进行切换，没有场景可切换的那一边按钮会消失
public class SceneChange : MonoBehaviour
{   
    // 这两个变量存储渐变特效参数
    [SerializeField]private float fadeSpeed = 1.5f;
	[SerializeField]private float duration = 0.5f;
    private bool sceneStarting = true;
    private RawImage backImage;
    Transform button;
    String name;
    //存储室内or室外
    String SceneType;
    int num;

    void Start() {
        Transform obj = transform.Find("RawImage");
        backImage = obj.GetComponent<RawImage>();
        backImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        //backImage.GetComponent<RectTransform>().position = new Vector3(0,0,0);

        SceneType = SceneManager.GetActiveScene().name.Substring(0,SceneManager.GetActiveScene().name.Length-1);
        name = SceneManager.GetActiveScene().name;
        // Debug.Log(name);
        name = name.Substring(name.Length-1,1);
        num = int.Parse(name);
        // Debug.Log(num);
        if(SceneType=="BloodScabValley_"){
            transform.Find("Ring").GetComponent<Image>().enabled = false;
            button = transform.Find("LeftScene");
            button.transform.position=new Vector3(360,240,0);
            button = transform.Find("RightScene");
            button.transform.position=new Vector3(1410,240,0);
            button = transform.Find("HomeScene");
            button.transform.position=new Vector3(885,190,0);
            if(num == 1){
                button = transform.Find("LeftScene");
                button.GetComponent<Image>().enabled = false;
            }else if(num == 4){
                button = transform.Find("RightScene");
                button.GetComponent<Image>().enabled = false;
            }else if(num == 2){
                button= transform.Find("HomeScene");
                button.GetComponent<Image>().enabled = true;;
            }
        }else if(SceneType=="BloodScabValleyIndoor_"){
            transform.Find("Ring").GetComponent<Image>().enabled = true;
            button = transform.Find("LeftScene");
            button.transform.position=new Vector3(360,240,0);
            button = transform.Find("RightScene");
            button.transform.position=new Vector3(1560,240,0);
            if(num == 1){
                button = transform.Find("LeftScene");
                button.GetComponent<Image>().enabled = false;
            }
        }
        else if(SceneType=="TestScene_"){
            transform.Find("Ring").GetComponent<Image>().enabled = false;
            button = transform.Find("LeftScene");
            button.transform.position=new Vector3(360,240,0);
            button = transform.Find("RightScene");
            button.transform.position=new Vector3(1410,240,0);
            button = transform.Find("HomeScene");
            button.transform.position=new Vector3(885,190,0);
            if(num == 1){
                button = transform.Find("LeftScene");
                button.GetComponent<Image>().enabled = false;
            }else if(num == 4){
                button = transform.Find("RightScene");
                button.GetComponent<Image>().enabled = false;
            }else if(num == 2){
                button= transform.Find("HomeScene");
                button.GetComponent<Image>().enabled = true;;
            }
        }
        
    }

    public void FadeToClear()
	{
		backImage.color = Color.Lerp(backImage.color, Color.clear, fadeSpeed*Time.deltaTime);
	}
	// 渐隐
	public void FadeToBlack()
	{
        
		backImage.color = Color.Lerp(backImage.color, Color.black, fadeSpeed*Time.deltaTime);
	}
     void Update(){
        if(sceneStarting){
            StartScene();
        }
    }

     private void StartScene()
	{
		backImage.enabled = true;
		FadeToClear();
		if(backImage.color.a <= 0.15f)
		{
			backImage.color = Color.clear;
			backImage.enabled = false;
			sceneStarting = false;
		}
		
	}
    public void LeftScene() {
        if(num==3 && SceneType=="BloodScabValleyIndoor_"){
            // 室内场景向左滑动代码
            return;
        }
        string from = num.ToString();
        num = num-1;
        
        string to = num.ToString();
        // Debug.Log(name);

        TransitionManager.Instance.Transition(SceneType+from,SceneType+to);
        // SceneManager.LoadScene(SceneType+name);
        // SceneManager.LoadScene("BloodScabValley_"+name);
    }
    public void RightScene(){
        if(num==3 && SceneType=="BloodScabValleyIndoor_"){
            // 室内场景向右滑动代码
            return;
        }
        string from = num.ToString();
        num = num+1;
        
        string to = num.ToString();

        // backImage.enabled = true;
        // backImage.color =Color.black;
        // Invoke(nameof(MethodName), duration);
        // backImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
		// FadeToBlack();
		// if(backImage.color.a >= 0.95f){
        //     Invoke(nameof(MethodName), duration);
        //     SceneManager.LoadScene("BloodScabValley_"+name);
		// }
        TransitionManager.Instance.Transition(SceneType+from,SceneType+to);
		// SceneManager.LoadScene(SceneType+to);
        // SceneManager.LoadScene("BloodScabValley_"+name);
    }

    private void MethodName()
    {
		return;
    }

    public void HomeScene(){
        // Debug.Log("Get Home.");
        string from = num.ToString();
        TransitionManager.Instance.Transition(SceneType+from,"BloodScabValleyIndoor_3");
        // SceneManager.LoadScene("BloodScabValleyIndoor_3");
    }
}
