using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

[System.Serializable]
public enum ItemType
{
    Item=1,
    Potion
}

[System.Serializable]
public class ItemData{
    [System.Serializable]
    public class Item{
        public int id;
        public string ItemName;
        public string ItemEnglishName;
        public string ItemType;
        public string ItemDescribe;
        public string SpritePath;
    }
    public string dataType;
    public Item[] Items;
}

[System.Serializable]
public class PotionData{
    [System.Serializable]
    public class Potion{
        public int id;
        public string PotionName;
        public string PotionEnglishName;
        public string PotionDescribe;
        public string SpritePath;
    }
    public string dataType;
    public Potion[] Potions;
}

//阅读json文件
public class LoadJson<T> : MonoBehaviour{
    public static T LoadJsonFromFile(string _FileName){
        if(!File.Exists(Application.dataPath + "/Data/"+_FileName+".json")){
            //Debug.Log(Application.dataPath + "/Data/"+_FileName+".json");
            return default(T);
        }

        StreamReader sr = new StreamReader(Application.dataPath + "/Data/"+_FileName+".json");
        if(sr==null){
            // Debug.Log("2不存在");
            return default(T);
        }
        string json = sr.ReadToEnd();
        if(json.Length > 0){
            // Debug.Log("存在");
            return JsonUtility.FromJson<T>(json);
        }
        // Debug.Log("全不存在");
        return default(T);
    }
}


public class ItemsInfo : MonoBehaviour {
    ItemData Item;


     #region debug
    private void ReadTest_Drug(ItemData item)
    {
        Debug.Log("( ItemsInfo脚本 ) 测试Json读取：" + item.dataType);
        foreach (var data in item.Items)
            Debug.Log(data.id + " " + data.ItemName + " " + data.ItemEnglishName);
        Debug.Log("\n");
    }
    
    #endregion

    public string getSpritePath(int id){
        if(id>0&&id<=50){
            foreach(var data in Item.Items){
                if(data.id == id){
                    return data.SpritePath;
                }
            }
        }
        return "none";
    }

    public int getId(string name){
        if(name!=null){
            foreach(var data in Item.Items){
                if(name==data.ItemEnglishName){
                    return data.id;
                }
            }
        }
        return 0;
    }

    public string getItemDescribe(int id){
        if(id>0&&id<=19){
            foreach(var data in Item.Items){
                if(data.id == id){
                    return data.ItemDescribe;
                }
            }
        }
        return "none";
    }

    #region 单例
    private static ItemsInfo _instance;
    public static ItemsInfo getInstance()
    {
        return _instance;
    }
    #endregion

    void Awake(){
        _instance=this;

        Item = LoadJson<ItemData>.LoadJsonFromFile("ItemInfo");
        // Debug.Log("11111111111");
        // Debug.Log(Item);
    }

}
