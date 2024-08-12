using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData : MonoBehaviour
{
    #region 单例
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

    public const int recordNum = 7; //档位数
    public const string NAME = "RecordData";

    public string[] recordName = new string[recordNum];
    public int lastID; //最后存档的栏位，如果没有继续游戏功能也可以不用

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
