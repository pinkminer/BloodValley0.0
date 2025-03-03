using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;

public class SAVE
{
    static string GetPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    #region PlayerPref
    public static void PlayerPrefSave(string key, object data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public static string PlayerPrefLoad(string key)
    {
        return PlayerPrefs.GetString(key, null);
    }
    #endregion

    #region JSON
    public static void JsonSave(string fileName, object data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath(fileName), json);

        Debug.Log($"“—±£¥Ê{GetPath(fileName)}");
    }

    public static T JsonLoad<T>(string fileName)
    {
        string path = GetPath(fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(GetPath(fileName));
            var data = JsonUtility.FromJson<T>(json);

            Debug.Log($"∂¡»°{path}");
            return data;
        }
        else { return default; }

    }

    public static void JsonDelete(string fileName)
    {
        File.Delete(GetPath(fileName));
    }

    #endregion

}
