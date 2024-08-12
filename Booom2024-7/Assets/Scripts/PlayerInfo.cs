using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    #region 单例
    public static PlayerInfo Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public string scensName; //保存时所在场景名

    public const int itemNum = 20; //物品总数
    public const int potionNum = 5; //药水总数
    public bool[] hasGotItems = new bool[itemNum];
    public bool[] hasGotPotions = new bool[potionNum];
    public bool[] hasUsedPotions = new bool[potionNum];

    //剧情进度读取，应该要等具体的剧情流程出来再改
    public bool isChattingWithNPC;
    public int npcStatus;
    /*   npcStatus   意义
     *   0           与诸葛兰对话中
     *   1           与npc1对话中
     *   2           与npc2对话中
     *   3           与npc3对话中
     */
    public int dialogueProgress; //存档时所读台词的行数

    public class SaveData
    {
        public string scensName;

        public bool[] hasGotItems = new bool[itemNum];
        public bool[] hasGotPotions = new bool[potionNum];
        public bool[] hasUsedPotions = new bool[potionNum];

        public bool isChattingWithNPC;
        public int npcStatus;
        public int dialogueProgress;
    }

    SaveData ForSave()
    {
        var savedata = new SaveData();
        savedata.scensName = scensName;
        savedata.hasGotItems = hasGotItems;
        savedata.hasGotPotions = hasGotPotions;
        savedata.hasUsedPotions = hasUsedPotions;
        savedata.isChattingWithNPC = isChattingWithNPC;
        savedata.npcStatus = npcStatus;
        savedata.dialogueProgress = dialogueProgress;
        return savedata;
    }

    void ForLoad(SaveData savedata)
    {
        scensName = savedata.scensName;
        hasGotItems = savedata.hasGotItems;
        hasGotPotions = savedata.hasGotPotions;
        hasUsedPotions = savedata.hasUsedPotions;
        isChattingWithNPC = savedata.isChattingWithNPC;
        npcStatus = savedata.npcStatus;
        dialogueProgress = savedata.dialogueProgress;
    }

    public void Save(int id)
    {
        SAVE.JsonSave(RecordData.Instance.recordName[id], 
            ForSave());
    }

    public void Load(int id)
    {
        var saveData = SAVE.JsonLoad<SaveData>
            (RecordData.Instance.recordName[id]);
        ForLoad(saveData);
    }

    public void Delete(int id)
    {
        SAVE.JsonDelete(RecordData.Instance.recordName[id]);
    }
}
