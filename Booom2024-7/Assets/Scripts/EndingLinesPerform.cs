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
    //�������������ܻ�Ҫ������Ļ����ȥ�Ժ��ٿ�����Ļ
    public bool isFading = false;
    private bool isFaded = false;
    private SceneChange sceneController;

    //�Ի�����
    private List<OneDialogueLine> endingText;
    private int linesCount;
    private int textIndex;

    //textMeshPro���
    public TextMeshProUGUI textPrefabDialogue;
    public TextMeshProUGUI textPrefabNarratage;
    private List<TextMeshProUGUI> thisGroupLine;
    private TextMeshProUGUI nextSign;
    private TextMeshProUGUI returnCue;

    //��ʽ����
    private float speakerWidth;
    private float colonWidth;
    private float generalIndent;
    private float thisLineHeight;
    private float accHeight=0; //content���ۻ��ĸ߶ȣ���content���ϵ��������еĶ���
    private float hangingCap=0; //��ǰ���ӷ�Χ�ڣ��������еײ������ӷ�Χ�ײ��ľ���
    public float minCap = 160; //�Զ�����ʱ���ֵײ���scroll view�ײ��Ŀռ�
    public float[] space= {14.2f,34,21};
    private int lastLineType; //0:̨�ʣ�1:�԰�

    //����
    public ScrollRect scrollRect;
    public Transform content;
    private RectTransform contentRect;
    private bool isScrolling = false;
    private bool isScrolled = false;
    private bool canScroll = false;
    public float scrollSpeed = 6f;
    public float overMidOffset = 66; //420+��������ж��Ƿ�Խ�ߣ����ǣ����Զ�����

    //��β��ʾ
    private float[] endingTimer = { 0, 0 }; //[0]:���һ��̨�ʽ�����ʼ��ʱ,
          //[1]:�����ʾ���һ���ʱ���������棬Ҫ����ȥ�������ƹ���ȥ�Ժ�һ��ʱ�������ʾ
    private bool returnCueShown = false;
    private string[] returnCueTexts= {"�����ǽ���", "��������"};
    private int returnCueType; //�ӱ�get���������źţ�0����;�����þ�������1�������������

    //"��"������
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

        returnCueType = 0; //������Ҫ�ӱ�get
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //���滻����������
        { 
            isFading = true; 
        }

        if (Input.GetKeyDown(KeyCode.L) && textIndex<linesCount && !isScrolling && !isScrolled) //�������Ի�������
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
                //����ĳ���ߣ���������������content�߶��ӳ�����ʼ�����������Ժ���ʾ��
                //��������content�ĸ߶Ⱥ͹��������������ı��ĸ߶ȣ�������Ҫget����߶�
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

        //�Զ���
        if (isScrolling)
        {
            scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(
                scrollRect.verticalNormalizedPosition, 0, scrollSpeed * Time.deltaTime);

            // ����Ƿ񵽴�Ŀ��λ��
            if (Mathf.Abs(scrollRect.verticalNormalizedPosition - 0) < 0.01f)
            {
                isScrolling = false; // ֹͣ����
                isScrolled = true;
            }
        }
    
        //�����˰�����ʾ����
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
        
        //content�ĸ߶����Թ����Ժ��ټ���������
        if (canScroll)
        {
            HandleScrollInput();
        }

        if (textIndex==linesCount && !returnCueShown)
        {
            endingTimer[0] += Time.deltaTime;
            Debug.Log("ending timer:" + endingTimer);

            if ((Input.GetKeyDown(KeyCode.L) && endingTimer[0] >0.1f) || endingTimer[1] >= 10)  //��������������
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
                //���س�ʼ������߼�
                Debug.Log("���س�ʼҳ��");
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
        if (endingText[index].speaker != "�԰�")
        {
            TextMeshProUGUI tmp_s = Instantiate(textPrefabDialogue, content);
            TextMeshProUGUI tmp_l = Instantiate(textPrefabDialogue, content);
            TextMeshProUGUI tmp_c = Instantiate(textPrefabDialogue, content); //ð��

            thisGroupLine.Add(tmp_s);
            thisGroupLine.Add(tmp_l);
            thisGroupLine.Add(tmp_c);

            //λ��
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

            //����
            //tmp_s.alignment = TextAlignmentOptions.Flush;
            tmp_s.alignment = TextAlignmentOptions.TopFlush;

            //��ɫ
            if (endingText[index].speaker == "�����")
            {
                tmp_s.color = new Color(161f/255f, 203f/255f, 247f/255f, 1f);
                Debug.Log("lan color:"+tmp_s.color);
            }

            //����
            tmp_s.text = endingText[index].speaker;
            tmp_l.text = endingText[index].line;
            tmp_c.text = "��";

            thisLineHeight = tmp_l.preferredHeight;
            //Debug.Log("thisLineHeight��" + thisLineHeight);
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
            
            //λ��
            RectTransform r_m = tmp_n.GetComponent<RectTransform>();

            float currHeight = index == 0 ? 0 : -(accHeight + space[1^lastLineType]);

            r_m.offsetMin = new Vector2(0, r_m.offsetMin.y);
            r_m.offsetMax = new Vector2(0, currHeight);

            //����
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

    //�����·���С������
    private void InitAndShowNextSign()
    {
        nextSign = Instantiate(textPrefabDialogue,content);
        RectTransform r_n = nextSign.GetComponent<RectTransform>();
        r_n.offsetMin = new Vector2(0, r_n.offsetMin.y);
        r_n.offsetMax = new Vector2(0, -(accHeight + space[2]));
        //nextSign.alignment=TextAlignmentOptions.Center;
        nextSign.alignment = TextAlignmentOptions.Top;
        nextSign.text = "��";
    }

    //������ʾ
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

    //��ȡ���԰������ֺżƵ�������
    private float GetIndent()
    {
        TextMeshProUGUI tempTMP = Instantiate(textPrefabNarratage);
        tempTMP.text = "����";
        float res = tempTMP.preferredWidth;
        Destroy(tempTMP.gameObject);
        tempTMP = null;
        Debug.Log("indent:"+res);
        return res;
    }

    //��ȡ��ǰ�����ֺ��£����������
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
        tempTMP.text = "��";
        float colonWidth = tempTMP.preferredWidth;
        Destroy(tempTMP.gameObject);
        //Debug.Log("Temp GameObject destroyed");
        tempTMP = null;
        float[] res = { maxSpeakerWidth, colonWidth};
        return res;
    }

    private void HandleScrollInput()
    {
        // ��ȡ�����ֵ�����
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            // ���� verticalNormalizedPosition
            scrollRect.verticalNormalizedPosition += scrollInput * scrollSpeed/4.4f;
        }

        // ������¼�ͷ��������
        if (Input.GetKey(KeyCode.UpArrow))
        {
            scrollRect.verticalNormalizedPosition += scrollSpeed * Time.deltaTime/4.4f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime/4.4f;
        }

        // ���� verticalNormalizedPosition �ķ�Χ�� 0 �� 1 ֮��
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
    }

    //������
    private IEnumerator SignBreath(TextMeshProUGUI sign)
    {
        Color signColor = sign.color;

        while (true) 
        {
            // �䰵
            while (signColor.a > 0.01f)
            {
                signColor.a = Mathf.Lerp(signColor.a, 0, signFadeOutSpeed * Time.deltaTime);
                sign.color = signColor;
                yield return null;
            }

            // ȷ��͸����Ϊ0
            signColor.a = 0f;
            sign.color = signColor;

            // ����
            while (signColor.a < 0.88f)
            {
                signColor.a = Mathf.Lerp(signColor.a, 1, signFadeInSpeed * Time.deltaTime);
                sign.color = signColor;
                yield return null;
            }

            // ȷ��͸����Ϊ1
            signColor.a = 1f;
            sign.color = signColor;
        }        
    }
}
