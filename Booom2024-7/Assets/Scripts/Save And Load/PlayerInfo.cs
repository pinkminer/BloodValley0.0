using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    #region ����
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

    public string scensName; //����ʱ���ڳ�����

    public const int itemNum = 20; //��Ʒ����
    public const int potionNum = 5; //ҩˮ����
    public bool[] hasGotItems = new bool[itemNum];
    public bool[] hasGotPotions = new bool[potionNum];
    public bool[] hasUsedPotions = new bool[potionNum];

    //������ȶ�ȡ��Ӧ��Ҫ�Ⱦ���ľ������̳����ٸ�
    public bool isChattingWithNPC;
    public int npcStatus;
    /*   npcStatus   ����
     *   0           ��������Ի���
     *   1           ��npc1�Ի���
     *   2           ��npc2�Ի���
     *   3           ��npc3�Ի���
     */
    public int dialogueProgress; //�浵ʱ����̨�ʵ�����

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
