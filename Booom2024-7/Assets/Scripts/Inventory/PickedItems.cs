using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PickedItems : MonoBehaviour
{
    public List<int> pickedItems;
    public List<int> usedItems;
    public int pickedItemNum;
    public int usedItemNum;

    public static PickedItems instance;

     #region 单例
    private static PickedItems _instance;
    public static PickedItems getInstance()
    {
        return _instance;
    }
    #endregion
    void Awake(){
        _instance=this;
        instance = this;
         PlayerPrefs.DeleteAll();

        //pickedItemNum = PlayerPrefs.GetInt("PickedItemNum");
        //usedItemNum = PlayerPrefs.GetInt("UsedItemNum");

        //for(int i=1;i<=pickedItemNum;i++){
        //    //picked+捡到道具的顺序：道具ID
        //    pickedItems.Add(PlayerPrefs.GetInt("picked"+i.ToString()));
        //}

        //for(int i=1;i<=usedItemNum;i++){
        //    //used+捡到道具的顺序：道具ID
        //    pickedItems.Add(PlayerPrefs.GetInt("used"+i.ToString()));
        //}

        // Debug.Log("11111111111");
        // Debug.Log(Item);
    }
}
