using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EndingLinesPerform : MonoBehaviour
{
    //黑屏，后续可能还要设置屏幕黑下去以后再开启字幕
    public bool isFading = false;
    private bool isFaded = false;
    private SceneChange sceneController;

    //对话控制
    private List<OneDialogueLine> endingText;
    private int linesCount;
    private int textIndex;

    //textMeshPro组件
    public TextMeshProUGUI textPrefabDialogue;
    public TextMeshProUGUI textPrefabNarratage;
    private List<TextMeshProUGUI> thisGroupLine;
    private TextMeshProUGUI nextSign;
    private TextMeshProUGUI returnCue;

    //格式对齐
    private float speakerWidth;
    private float colonWidth;
    private float generalIndent;
    private float thisLineHeight;
    private float accHeight=0; //content内累积的高度，从content顶上到现在这行的顶部
    private float hangingCap=0; //当前可视范围内，现在这行底部到可视范围底部的距离
    public float minCap = 160; //自动滚动时文字底部到scroll view底部的空间
    public float[] space= {14.2f,34,21};
    private int lastLineType; //0:台词，1:旁白

    //滚动
    public ScrollRect scrollRect;
    public Transform content;
    private RectTransform contentRect;
    private bool isScrolling = false;
    private bool isScrolled = false;
    private bool canScroll = false;
    public float scrollSpeed = 6f;
    public float overMidOffset = 66; //420+这个变量判断是否越线，如是，则自动滚动

    //结尾提示
    private float[] endingTimer = { 0, 0 }; //[0]:最后一句台词结束后开始计时,
          //[1]:如果显示最后一句的时候不在最下面，要滚下去，用来计滚下去以后一段时间后再显示
    private bool returnCueShown = false;
    private string[] returnCueTexts= {"还不是结束", "如如如如"};
    private int returnCueType; //从别处get结局种类的信号，0：中途次数用尽死亡，1：到达非人真结局

    //"▼"呼吸灯
    private Coroutine signBreathCo;
    private float signFadeOutSpeed = 3.6f;
    private float signFadeInSpeed = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        endingText = PlotDialogues.getInstance().GetEndingDialogues("Yuanjun");
        linesCount = endingText.Count;
        Debug.Log("lines count:" + linesCount);
        
        sceneController = GetComponent<SceneChange>();
        //content = transform.Find("Scroll View/Viewport/Content");
        if (content == null) { Debug.Log("content is null"); }
        contentRect = content.GetComponent<RectTransform>();

        textIndex = 0;

        generalIndent = GetIndent();
        float[] widths = MaxSpeakerNameWidths();
        speakerWidth = widths[0];
        colonWidth = widths[1];

        thisGroupLine = new List<TextMeshProUGUI>();

        returnCueType = 0; //还是需要从别处get
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //后面换成其它条件
        { 
            isFading = true; 
        }

        if (Input.GetKeyDown(KeyCode.L) && textIndex<linesCount && !isScrolling && !isScrolled) //按键可以换成其它
        {
            if (thisGroupLine != null)
            {
                thisGroupLine.Clear();
            }
            if (nextSign != null) 
            { 
                StopCoroutine(signBreathCo);
                signBreathCo = null;
                Destroy(nextSign.gameObject);
                nextSign = null;
            }

            if(accHeight < 420 + overMidOffset)
            {
                AddNewLine(textIndex++);
                hangingCap = 840 - accHeight - thisLineHeight;
                if (textIndex < linesCount)
                {
                    InitAndShowNextSign();
                    IEnumerator e = SignBreath(nextSign);
                    signBreathCo = StartCoroutine(e);
                }
/*                else
                {
                    linesEnd = true;
                }*/
            }
            else
            {
                //过了某个线：把字隐藏起来，content高度延长，开始滚动，结束以后显示字
                //其中增加content的高度和滚动的量都是新文本的高度，所以需要get这个高度
                AddNewLine(textIndex);
                foreach (var tmp in thisGroupLine)
                {
                    tmp.enabled = false;
                }
                contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentRect.sizeDelta.y + thisLineHeight + space[2]);
                if (hangingCap < minCap)
                {
                    contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentRect.sizeDelta.y + minCap - hangingCap);
                    hangingCap = minCap;
                }
                isScrolling = true;
                canScroll = true;
            }

            //Debug.Log("HaningCap:" + hangingCap);
            
        }

        //自动滚
        if (isScrolling)
        {
            scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(
                scrollRect.verticalNormalizedPosition, 0, scrollSpeed * Time.deltaTime);

            // 检查是否到达目标位置
            if (Mathf.Abs(scrollRect.verticalNormalizedPosition - 0) < 0.01f)
            {
                isScrolling = false; // 停止滚动
                isScrolled = true;
            }
        }
    
        //滚完了把字显示出来
        if (isScrolled && textIndex<linesCount)
        {
            foreach (var tmp in thisGroupLine)
            {
                tmp.enabled = true;
            }
            textIndex++;
            if (textIndex < linesCount)
            {
                InitAndShowNextSign();
                IEnumerator e = SignBreath(nextSign);
                signBreathCo = StartCoroutine(e);
            }
/*            else
            {
                linesEnd = true;
            }*/
            isScrolled = false;
        }
        
        //content的高度足以滚动以后再检测滚动输入
        if (canScroll)
        {
            HandleScrollInput();
        }

        if (textIndex==linesCount && !returnCueShown)
        {
            endingTimer[0] += Time.deltaTime;
            Debug.Log("ending timer:" + endingTimer);

            if ((Input.GetKeyDown(KeyCode.L) && endingTimer[0] >0.1f) || endingTimer[1] >= 10)  //按键换成其它的
            {
                if (scrollRect.verticalNormalizedPosition <= 0.01f)
                {
                    InitAndShowReturnCue(returnCueType);
                    returnCueShown = true;
                }
                else
                {
                    isScrolling = true;
                }               
            }
        }

        if(isScrolled && textIndex == linesCount)
        {
            endingTimer[1] += Time.deltaTime;
            if (endingTimer[1] > 0.44f)
            {
                InitAndShowReturnCue(returnCueType);
                returnCueShown = true;
                isScrolled = false;
            }           
        }

        if (returnCueShown)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                //返回初始界面的逻辑
                Debug.Log("返回初始页面");
            }
        }
        
        if (isFading)
        {
            EndingFade();
        }

        Debug.Log("text index:" + textIndex);
    }

    public void EndingFade()
    {
        sceneController.backImage.enabled = true;
        sceneController.FadeToBlack();
        if (sceneController.backImage.color.a >= 9.95f)
        {
            sceneController.backImage.color = Color.black;
            isFading = false;
            isFaded = true;
        }
    }

    private void AddNewLine(int index)
    {
        if (endingText[index].speaker != "旁白")
        {
            TextMeshProUGUI tmp_s = Instantiate(textPrefabDialogue, content);
            TextMeshProUGUI tmp_l = Instantiate(textPrefabDialogue, content);
            TextMeshProUGUI tmp_c = Instantiate(textPrefabDialogue, content); //冒号

            thisGroupLine.Add(tmp_s);
            thisGroupLine.Add(tmp_l);
            thisGroupLine.Add(tmp_c);

            //位置
            RectTransform r_s = tmp_s.GetComponent<RectTransform>();
            RectTransform r_l = tmp_l.GetComponent<RectTransform>();
            RectTransform r_c = tmp_c.GetComponent<RectTransform>();

            float currHeight = index == 0 ? 0 : -(accHeight + space[0^lastLineType]);

            r_s.offsetMin = new Vector2(generalIndent, r_s.offsetMin.y);
            r_s.offsetMax = new Vector2(-(1320 - generalIndent - speakerWidth), currHeight);
            r_l.offsetMin = new Vector2(speakerWidth + colonWidth + generalIndent, r_l.offsetMin.y);
            r_l.offsetMax = new Vector2(0, currHeight);
            r_c.offsetMin = new Vector2(speakerWidth + generalIndent, r_c.offsetMin.y);
            r_c.offsetMax = new Vector2(0,currHeight);

            //对齐
            //tmp_s.alignment = TextAlignmentOptions.Flush;
            tmp_s.alignment = TextAlignmentOptions.TopFlush;

            //颜色
            if (endingText[index].speaker == "诸葛兰")
            {
                tmp_s.color = new Color(161f/255f, 203f/255f, 247f/255f, 1f);
                Debug.Log("lan color:"+tmp_s.color);
            }

            //内容
            tmp_s.text = endingText[index].speaker;
            tmp_l.text = endingText[index].line;
            tmp_c.text = "：";

            thisLineHeight = tmp_l.preferredHeight;
            //Debug.Log("thisLineHeight：" + thisLineHeight);
            accHeight += index==0 ? thisLineHeight: thisLineHeight + space[0^lastLineType];
            /*if (index > 0)
            {
                Debug.Log("space:" + space[0 ^ lastLineType]);
            }*/

            lastLineType = 0;
        }
        else
        {
            TextMeshProUGUI tmp_n = Instantiate(textPrefabNarratage, content);

            thisGroupLine.Add(tmp_n);
            
            //位置
            RectTransform r_m = tmp_n.GetComponent<RectTransform>();

            float currHeight = index == 0 ? 0 : -(accHeight + space[1^lastLineType]);

            r_m.offsetMin = new Vector2(0, r_m.offsetMin.y);
            r_m.offsetMax = new Vector2(0, currHeight);

            //内容
            tmp_n.text = "\u3000\u3000"+ endingText[index].line;

            thisLineHeight = tmp_n.preferredHeight;
            accHeight += index==0 ? thisLineHeight : thisLineHeight + space[1 ^ lastLineType];

           /* if (index > 0)
            {
                Debug.Log("space:" + space[0 ^ lastLineType]);
            }*/

            lastLineType = 1;
        }
        //Debug.Log(accHeight);
    }

    //文字下方的小三角形
    private void InitAndShowNextSign()
    {
        nextSign = Instantiate(textPrefabDialogue,content);
        RectTransform r_n = nextSign.GetComponent<RectTransform>();
        r_n.offsetMin = new Vector2(0, r_n.offsetMin.y);
        r_n.offsetMax = new Vector2(0, -(accHeight + space[2]));
        //nextSign.alignment=TextAlignmentOptions.Center;
        nextSign.alignment = TextAlignmentOptions.Top;
        nextSign.text = "▼";
    }

    //最后的提示
    private void InitAndShowReturnCue(int cueType)
    {
        if (cueType != 0 && cueType != 1) 
        {
            Debug.Log("cue type error!");
            return;
        }
        
        returnCue = Instantiate(textPrefabNarratage,content);
        RectTransform r = returnCue.GetComponent<RectTransform>();
        r.offsetMin = new Vector2(0,r.offsetMin.y);
        r.offsetMax = new Vector2(0, -99);
        returnCue.alignment = TextAlignmentOptions.Bottom;
        returnCue.text = returnCueTexts[cueType];
    }

    //获取按旁白字体字号计的缩进量
    private float GetIndent()
    {
        TextMeshProUGUI tempTMP = Instantiate(textPrefabNarratage);
        tempTMP.text = "缩进";
        float res = tempTMP.preferredWidth;
        Destroy(tempTMP.gameObject);
        tempTMP = null;
        Debug.Log("indent:"+res);
        return res;
    }

    //获取当前字体字号下，人名最大宽度
    private float[] MaxSpeakerNameWidths()
    {
        float maxSpeakerWidth = 0f;

        int maxWrdNum = 0;
        HashSet<string> speakers = new HashSet<string>();
        foreach (var pair in endingText)
        {
            //Debug.Log("listing speakers:"+speaker+"; length:"+speaker.Length);
            if (pair.speaker.Length > maxWrdNum)
            {
                maxWrdNum = pair.speaker.Length;
                speakers.Clear();
                speakers.Add(pair.speaker);
            }
            else if (pair.speaker.Length == maxWrdNum)
            {
                speakers.Add(pair.speaker);
            }
        }

        //Debug.Log("maxWrdNum:" + maxWrdNum);
        //GameObject temp = new GameObject("TempText");
        TextMeshProUGUI tempTMP = Instantiate(textPrefabDialogue);
        //tempTMP.font = textPrefab.font;
        //tempTMP.fontSize = textPrefab.fontSize;
        foreach (string speaker in speakers)
        {
            //Debug.Log("longest speaker: "+speaker);
            tempTMP.text = speaker;
            maxSpeakerWidth = Math.Max(maxSpeakerWidth, tempTMP.preferredWidth);
        }
        tempTMP.text = "：";
        float colonWidth = tempTMP.preferredWidth;
        Destroy(tempTMP.gameObject);
        //Debug.Log("Temp GameObject destroyed");
        tempTMP = null;
        float[] res = { maxSpeakerWidth, colonWidth};
        return res;
    }

    private void HandleScrollInput()
    {
        // 获取鼠标滚轮的输入
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            // 调整 verticalNormalizedPosition
            scrollRect.verticalNormalizedPosition += scrollInput * scrollSpeed/4.4f;
        }

        // 检测上下箭头键的输入
        if (Input.GetKey(KeyCode.UpArrow))
        {
            scrollRect.verticalNormalizedPosition += scrollSpeed * Time.deltaTime/4.4f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime/4.4f;
        }

        // 限制 verticalNormalizedPosition 的范围在 0 到 1 之间
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
    }

    //呼吸灯
    private IEnumerator SignBreath(TextMeshProUGUI sign)
    {
        Color signColor = sign.color;

        while (true) 
        {
            // 变暗
            while (signColor.a > 0.01f)
            {
                signColor.a = Mathf.Lerp(signColor.a, 0, signFadeOutSpeed * Time.deltaTime);
                sign.color = signColor;
                yield return null;
            }

            // 确保透明度为0
            signColor.a = 0f;
            sign.color = signColor;

            // 变亮
            while (signColor.a < 0.88f)
            {
                signColor.a = Mathf.Lerp(signColor.a, 1, signFadeInSpeed * Time.deltaTime);
                sign.color = signColor;
                yield return null;
            }

            // 确保透明度为1
            signColor.a = 1f;
            sign.color = signColor;
        }        
    }
}
