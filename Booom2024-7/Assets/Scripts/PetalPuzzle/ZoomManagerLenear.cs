using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoomManagerLinear : MonoBehaviour
{
    public Camera mainCamera;
    private PetalPuzzle PetalPuzzle;
    private bool isPuzzleEnabled = false;

    public List<GameObject> clickTargets;
    private int targetsCount;
    public Button backButton;
    private EventTrigger[] eventTriggers;

    public float fullSize = 5f; // =mainCamera.orthographicSize
    private float[] zoomedSizes;// = new float[clickTargets.Count];
    private float[,] speeds; //[1][i]:in; [0][i]:out
    public float exitSpeed;
    public Vector3 initialCamPos = new Vector3(0, 0, -10);
    private Vector3[] focusPoints;

    private float statePointer = 0f;
    private bool isZoomingIn = false;
    private bool isZoomingOut = false;

    private int witherStatus = 0;
    public bool exited = false;

    // Start is called before the first frame update
    void Start()
    {
        targetsCount = clickTargets.Count;
        //初始化数组变量并从游戏对象组件获取数值
        zoomedSizes = new float[targetsCount + 1];
        speeds = new float[2, targetsCount];
        focusPoints = new Vector3[targetsCount + 1];

        zoomedSizes[0] = fullSize;
        focusPoints[0] = initialCamPos;
        for (int i = 0; i < targetsCount; i++)
        {
            ZoomClickTarget Z = clickTargets[i].GetComponent<ZoomClickTarget>();
            zoomedSizes[i + 1] = Z.zoomedSize;
            speeds[0, i] = Z.outSpeed;
            speeds[1, i] = Z.inSpeed;
            focusPoints[i + 1] = Z.focusPoint;
        }

        //给游戏对象添加事件触发器，并初始化其启停状态
        eventTriggers = new EventTrigger[targetsCount];
        for (int i = 0; i < targetsCount; i++)
        {
            EventTrigger trigger = clickTargets[i].AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerClick
            };
            entry.callback.AddListener((data) =>
            {
                OnTargetClick();
            });
            trigger.triggers.Add(entry);
            trigger.enabled = false;
            eventTriggers[i] = trigger;
        }
        eventTriggers[0].enabled = true;

        //初始化button状态使其隐藏
        backButton.onClick.AddListener(() => { OnBackButtonClick(); });
        backButton.gameObject.SetActive(false);

        //获取花瓣谜题脚本对象用于控制
        PetalPuzzle = GetComponent<PetalPuzzle>();

        //Debug.Log("hello");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(statePointer);

        witherStatus = PetalPuzzle.witherStatus;
        if (witherStatus == 2)
        {
            if (!exited)
            {
                //Debug.Log("zoommanager: withered");
                exitPetalPuzzle();
                PetalPuzzle.enabled = false;
                isPuzzleEnabled = false;
            }
            else
            {
                if (statePointer == 0)
                {
                    this.enabled = false;
                }
            }
        }
        /*
        Debug.Log(statePointer);
        for(int i=0; i<targetsCount; i++)
        {
            if (eventTriggers[i].enabled)
            {
                Debug.Log("the " + i + "th trigger is on");
            }
            else
            {
                Debug.Log("the " + i + "th trigger is off");
            }
        }
        */
        if (witherStatus == 0 && statePointer % 1 == 0 && statePointer < targetsCount)
        {
            eventTriggers[(int)statePointer].enabled = true;
        }

        if (witherStatus == 0 && statePointer > 0)
        {
            backButton.gameObject.SetActive(true);
        }
        else
        {
            //只要witherStatus等于1也就是开始枯萎了，按钮立刻会消失
            backButton.gameObject.SetActive(false);
        }

        if (isZoomingIn || isZoomingOut)
        {
            ZoomInOut();
        }

        //添加谜题脚本启停逻辑：只有i==targetcount时才启动puzzle脚本，否则关闭
        //由开转关时，需要复位所有花瓣
        if (statePointer == targetsCount)
        {
            if (!isPuzzleEnabled)
            {
                //PetalPuzzle.enabled = true;
                PetalPuzzle.PuzzleOn();
                isPuzzleEnabled = true;
            }
        }
        else
        {
            if (isPuzzleEnabled)
            {
                PetalPuzzle.PuzzleOff();
                //PetalPuzzle.enabled = false;
                isPuzzleEnabled = false;
                //StartCoroutine(ResetPetalPuzzleScript());
            }
        }
    }

    private void OnTargetClick()
    {
        eventTriggers[(int)statePointer].enabled = false;  //点击后，禁用本trigger
        isZoomingIn = true;  //此时必有isZoomingOut=false
        statePointer += 0.5f;
    }

    private void OnBackButtonClick()
    {
        /* 分类讨论：状态-已到达/推动中；推动中-in/out
        * 已到达：zooming==false, currstate%1==0 => currstate-=0.5,zoomingout=true,nexttigger off
        * in: zoomingin==t/zoomingout==f, currstate%1!=0 => currstate still,zoomingout=true
        * out: zoomingin==f/zoomingout==t, currstate%1!=0 => currstate still,zoomingout=true (don't do anything
        */

        if (statePointer % 1 == 0)
        {
            if (statePointer < targetsCount)
            {
                eventTriggers[(int)statePointer].enabled = false;
            }
            statePointer -= 0.5f;
        }
        isZoomingIn = false;
        isZoomingOut = true;
    }

    private void ZoomInOut()
    {
        //statePointer必不为整数
        int targetPointer = isZoomingIn ? (int)statePointer + 1 : (int)statePointer;
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomedSizes[targetPointer], Time.deltaTime * speeds[targetPointer - (int)statePointer, (int)statePointer]);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, focusPoints[targetPointer], Time.deltaTime * speeds[targetPointer - (int)statePointer, (int)statePointer]);
        if (Mathf.Abs(mainCamera.orthographicSize - zoomedSizes[targetPointer]) < 0.01f)
        {
            mainCamera.orthographicSize = zoomedSizes[targetPointer];
            isZoomingIn = false;
            isZoomingOut = false;
            statePointer = targetPointer;
            //Debug.Log("zoomed");
        }
    }

    private void exitPetalPuzzle()
    {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, fullSize, Time.deltaTime * exitSpeed);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, initialCamPos, Time.deltaTime * exitSpeed);
        if (Mathf.Abs(mainCamera.orthographicSize - fullSize) < 0.01f)
        {
            mainCamera.orthographicSize = fullSize;
            isZoomingIn = false;
            isZoomingOut = false;
            statePointer = 0;
            exited = true;
            Debug.Log("exited");
        }
        //此时必有所有trigger都是关着的。因为petalPuzzle on的时候，
        //所有的trigger都已经关了
    }

    /*    private IEnumerator ResetPetalPuzzleScript()
        {
            PetalPuzzle.ResetAllThePetals();
            PetalPuzzle.SetTriggers(false);
            yield return new WaitForSeconds(0.1f);

            PetalPuzzle.enabled = false;
            Debug.Log("puzzle disabled");
            yield break;
        }*/
}
