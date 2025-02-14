using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{   

    [SerializeField]private float fadeSpeed = 1.5f;
	[SerializeField]private float duration = 0.5f;
    private bool sceneStarting = true;
    private RawImage backImage;
    Transform button;
    String name;
    int num;

    void Start() {
        Transform obj = transform.Find("RawImage");
        backImage = obj.GetComponent<RawImage>();
        backImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        //backImage.GetComponent<RectTransform>().position = new Vector3(0,0,0);

        name = SceneManager.GetActiveScene().name;
        // Debug.Log(name);
        name = name.Substring(name.Length-1,1);
        num = int.Parse(name);
        // Debug.Log(num);
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

    private void FadeToClear()
	{
		backImage.color = Color.Lerp(backImage.color, Color.clear, fadeSpeed*Time.deltaTime);
	}
	// 渐隐
	private void FadeToBlack()
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
        
        num = num-1;
        name = num.ToString();
        // Debug.Log(name);


        SceneManager.LoadScene("BloodScabValley_"+name);
    }
    public void RightScene(){
        num = num + 1;
        name = num.ToString();
        // Debug.Log(name);

        backImage.enabled = true;
        backImage.color =Color.black;
        Invoke(nameof(MethodName), duration);
        // backImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
		// FadeToBlack();
		// if(backImage.color.a >= 0.95f){
        //     Invoke(nameof(MethodName), duration);
        //     SceneManager.LoadScene("BloodScabValley_"+name);
		// }
		
        SceneManager.LoadScene("BloodScabValley_"+name);
    }

    private void MethodName()
    {
		return;
    }

    public void HomeScene(){
        Debug.Log("Get Home.");
        // SceneManager.LoadScene("HomeScene");
    }
}
