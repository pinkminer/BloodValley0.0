using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData : MonoBehaviour
{
    #region ����
    public static RecordData Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != null)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public const int recordNum = 7; //��λ��
    public const string NAME = "RecordData";

    public string[] recordName = new string[recordNum];
    public int lastID; //���浵����λ�����û�м�����Ϸ����Ҳ���Բ���

    class SaveData
    {
        public string[] recordName = new string[recordNum];
        public int lastID;
    }

    SaveData ForSave()
    {
        var savedata = new SaveData();
        for (int i = 0; i < recordNum; i++)
        {
            savedata.recordName[i] = recordName[i];
        }
        savedata.lastID = lastID;
        return savedata;
    }

    void ForLoad(SaveData savedata)
    {
        lastID = savedata.lastID;
        for (int i = 0; i < recordNum; i++)
        {
            recordName[i] = savedata.recordName[i];
        }
    }

    public void Save()
    {
        SAVE.PlayerPrefSave(NAME, ForSave());
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(NAME))
        {
            string json = SAVE.PlayerPrefLoad(NAME);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            ForLoad(saveData);
        }
    }

}
