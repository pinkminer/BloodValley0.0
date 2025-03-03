using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

//����
[System.Serializable]
public class ZhangMonologuePassage
{
    public string playerStatus;
    public List<string> lines;
}

[System.Serializable]
public class MissBoxMonologuePassage
{
    public string triggerPlayerStatus;
    public int loop; //0:��һ��ͣ��1:����ѭ��
    public List<string> lines;
}

//��json
[System.Serializable]
public class ZhangMonologues
{
    public List<ZhangMonologuePassage> zhangMonologues;
}

[System.Serializable]
public class MissBoxMonologues
{
    public List<MissBoxMonologuePassage> missBoxMonologues;
}


//�Ի�
[System.Serializable]
public class OneDialogueLine
{
    public string speaker;
    public string line;
}


//������������Ǵӿ�ʼ��������һ�ζԻ�
[System.Serializable]
public class EndingDialoguePassage
{
    //Ҳ������������Ժ�ĺ�Ļ����
    public string endingStatus;  //��Ҵﵽ����̬������
    public List<OneDialogueLine> dialogues;
}

[System.Serializable]
public class NpcDialoguePassage
{
    //��ͨ����ʱ������npc�Ի�
    public int npcDialogueID; //Ψһ�Ի�id
    public string npcName;
    public int triggerPotionID;
    public int rounds; //��Ŀ��
    public int[] intervals; //�ֶε�dialogue����
    public List<OneDialogueLine> dialogues;
}

[System.Serializable]
public class LanDialoguePassage_Drink
{
    //����������ĶԻ����ܺ�ҩ����
    public int realDrinkTime;
    public List<OneDialogueLine> dialogues;
}

[System.Serializable]
public class LanDialoguePassage_Give
{
    //���������Һ��յ���ʱ�����ĶԻ�
    public int itemID;
    public List<OneDialogueLine> dialogues;
}

[System.Serializable]
public class MissBoxDialoguePassage
{
    public string triggerType; 
    //A,B,C����ȡֵ��Aÿ��Ŀ��һ�ֵڶ���ȥ����������ÿ��Ŀֻ����һ�Σ�
    //Bÿ��Ŀ*��һ��*�ڡ�ĳ������̬����������ڳ������ʱ��������ͬ��̬����̨����ͬ
    //C�ڡ�ĳ������̬����������ڷ��临����һ������������ʱ��������ͬ��̬����̨�ʲ�ͬ
    //ÿ������Ŀ��ABֻ����һ�Σ�C�޴�����

    public string lastEndingStatus; //��triggerTypeΪA��B�������ȱʡ
    public List<OneDialogueLine> dialogues;
}


//ÿһ�ֶԻ�д��һ�����json�ļ���ÿһ��json�ļ���Ϊ�����һ�����һ������
[System.Serializable]
public class EndingDialogues
{
    public List<EndingDialoguePassage> endingDialogues;
}

[System.Serializable]
public class NpcDialogues
{
    public List<NpcDialoguePassage> npcDialogues;
}

[System.Serializable]
public class LanDialogues
{
    public List<LanDialoguePassage_Drink> lanDialogues_drink;
    public List<LanDialoguePassage_Give> lanDialogues_give;
}

[System.Serializable]
public class MissBoxDialogues
{
    public List<MissBoxDialoguePassage> missBoxDialogues;
}



public class PlotDialogues : MonoBehaviour
{
    ZhangMonologues zhangMonologues;

    MissBoxMonologues missBoxMonologues;
    
    EndingDialogues endingDialogues;
    EndingDialogues endingDialogues_test;

    NpcDialogues npcDialogues;
    NpcDialogues npcDialogues_test;

    LanDialogues lanDialogues;
    LanDialogues lanDialogues_test;

    MissBoxDialogues missBoxDialogues;


    private string[] lanCalls = { "�����������ˡ�", "ร��㿴�������������ˡ�", "*תͷ �������������ˡ�", "*תͷ �������µ�ί���ˡ�", "*ת�� �ۣ����������������ˣ�����������ร��ټ�����"};


    #region ����

    private static PlotDialogues instance;
    public static PlotDialogues getInstance()
    {
        return instance;
    }

    #endregion


    private void Awake()
    {
        instance = this;
        
        //endingDialogues = LoadJson<EndingDialogues>.LoadJsonFromFile("EndingDialogues");
        endingDialogues_test = LoadJson<EndingDialogues>.LoadJsonFromFile("DialogueAndMonologueTexts\\endingTest_1");
        npcDialogues_test = LoadJson<NpcDialogues>.LoadJsonFromFile("DialogueAndMonologueTexts\\NpcDialogues_raw");
        
        /* 
         * ע�������json�ļ�ת��csv��ת��jsonʱ���ṹ���
         * ԭ����{"l_drink":[], "l_give":[]}
         * ���[{l_drink":[]},{"l_give":[]}]
         * ������ʵ����LanDialogues�Ľṹ��������Ҫ����������
         */
        lanDialogues_test = LoadJson<LanDialogues>.LoadJsonFromFile("DialogueAndMonologueTexts\\LanDialogues_raw");
        /*if (lanDialogues_test == null)
        {
            Debug.Log("dialogue null");
        }
        else
        {
            Debug.Log("dialogue exists");
        }
        Debug.Log(JsonUtility.ToJson(lanDialogues_test));*/
    }

    // Start is called before the first frame update
    void Start()
    {
        //PrintEndingLines("testStatus1");

        //PrintNpcLines("npcName1");
        PrintTest(GetLanDialogues_Drink(2,true));
    }


    private void PrintTest(List<OneDialogueLine> dialogueLines)
    {
        foreach (var pair in dialogueLines)
        {
            Debug.Log(pair.speaker + ": " + pair.line);
        }
    }


    public List<string> GetZhangMonologues(string playerStatus)
    {
        foreach(var passage in zhangMonologues.zhangMonologues)
        {
            if (passage.playerStatus == playerStatus)
            {
                return passage.lines;
            }
        }
        return null;
    }

    public List<string> GetMissBoxMonologues(string triggerPlayerStatus)
    {
        foreach (var passage in missBoxMonologues.missBoxMonologues)
        {
            if (passage.triggerPlayerStatus == triggerPlayerStatus)
            {
                return passage.lines;
            }
        }
        return null;
    }

    public List<OneDialogueLine> GetEndingDialogues(string endingStatus)
    {
        foreach (var passage in endingDialogues_test.endingDialogues)
        {
            if (passage.endingStatus == endingStatus)
            {
                return passage.dialogues;
            }
        }
        return null;
    }

    public List<List<OneDialogueLine>> GetNpcDialogues(int npcDialogueID)
    {
        foreach (var passage in npcDialogues_test.npcDialogues)
        {
            if (passage.npcDialogueID==npcDialogueID)
            {
                int linesCount = passage.dialogues.Count;
                int intervalCount = passage.intervals.Length;
                if (linesCount <= passage.intervals[intervalCount - 1])
                {
                    return null;
                }
                List<List<OneDialogueLine>> npcLines = new List<List<OneDialogueLine>>();
                List<OneDialogueLine> onePartLines = new List<OneDialogueLine>();
                int j = 0;
                for (int i=0; i<intervalCount+1; i++)
                {
                    int jj = i < intervalCount ? passage.intervals[i] : linesCount;
                    while (j < jj)
                    {
                        onePartLines.Add(passage.dialogues[j]);
                        j++;
                    }
                    npcLines.Add(onePartLines);
                    onePartLines.Clear();
                }
                return npcLines;
            }
        }

        return null;
    }

    public List<OneDialogueLine> GetLanDialogues_Drink(int realDrinkTime, bool callingOthers)
    {
        foreach (var passage in lanDialogues_test.lanDialogues_drink)
        {
            if (passage.realDrinkTime == realDrinkTime)
            {
                if (!callingOthers)
                {
                    return passage.dialogues;
                }
                else
                {
                    OneDialogueLine call = new OneDialogueLine();
                    call.speaker = "�����";
                    call.line = lanCalls[Random.Range(0,lanCalls.Length)];
                    List<OneDialogueLine> passage_Drink = passage.dialogues;
                    passage_Drink.Add(call);
                    return passage_Drink;
                }
                
            }
        }
        return null;
    }

    public List<OneDialogueLine> GetLanDialogues_Give(int itemID)
    {
        foreach (var passage in lanDialogues_test.lanDialogues_give)
        {
            if (passage.itemID == itemID)
            {
                return passage.dialogues;
            }
        }
        return null;
    }

    public List<OneDialogueLine> GetMissBoxDialogues(string triggerType)
    {
        foreach (var passage in missBoxDialogues.missBoxDialogues)
        {
            if (passage.triggerType == triggerType)
            {
                return passage.dialogues;
            }
        }
        return null;
    }


    /*public void PrintNpcLines(string npcName)
    {
        foreach (var passage in npcDialogues_test.npcDialogues)
        {
            Debug.Log(passage.rounds);
            if (passage.npcName == npcName)
            {
                foreach (var line in passage.dialogues)
                {
                    Debug.Log(line.line);
                    Debug.Log("interval");
                }
            }
        }
    }*/
}
