using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

//独白
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
    public int loop; //0:播一遍停，1:无限循环
    public List<string> lines;
}

//读json
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


//对话
[System.Serializable]
public class OneDialogueLine
{
    public string speaker;
    public string line;
}


//下面的三个类是从开始到结束的一段对话
[System.Serializable]
public class EndingDialoguePassage
{
    //也就是玩家似了以后的黑幕文字
    public string endingStatus;  //玩家达到的形态的名称
    public List<OneDialogueLine> dialogues;
}

[System.Serializable]
public class NpcDialoguePassage
{
    //普通变身时触发的npc对话
    public int npcDialogueID; //唯一对话id
    public string npcName;
    public int triggerPotionID;
    public int rounds; //周目数
    public int[] intervals; //分段的dialogue索引
    public List<OneDialogueLine> dialogues;
}

[System.Serializable]
public class LanDialoguePassage_Drink
{
    //触发诸葛兰的对话：总喝药次数
    public int realDrinkTime;
    public List<OneDialogueLine> dialogues;
}

[System.Serializable]
public class LanDialoguePassage_Give
{
    //诸葛兰给玩家合照道具时发生的对话
    public int itemID;
    public List<OneDialogueLine> dialogues;
}

[System.Serializable]
public class MissBoxDialoguePassage
{
    public string triggerType; 
    //A,B,C三种取值，A每周目第一轮第二次去场景触发，每周目只触发一次，
    //B每周目*第一次*在【某几个形态次数用完后，在场景复活】时触发，不同形态触发台词相同
    //C在【某几个形态次数用完后，在房间复活，后第一次来到场景】时触发，不同形态触发台词不同
    //每个真周目，AB只触发一次，C无此限制

    public string lastEndingStatus; //若triggerType为A或B，则此项缺省
    public List<OneDialogueLine> dialogues;
}


//每一种对话写成一个大的json文件，每一个json文件读为下面的一个类的一个对象
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


    private string[] lanCalls = { "啊，有人来了。", "喔，你看，有人来找你了。", "*转头 有朋友来找你了。", "*转头 好像有新的委托了。", "*转身 哇，好像有人来找你了，那我先走了喔！再见——"};


    #region 单例

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
         * 注：这里的json文件转成csv再转回json时，结构会从
         * 原本的{"l_drink":[], "l_give":[]}
         * 变成[{l_drink":[]},{"l_give":[]}]
         * 后者事实上与LanDialogues的结构不符。需要调整过来。
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
                    call.speaker = "诸葛兰";
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
