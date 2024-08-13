using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInitiate : MonoBehaviour
{
    public static List<GameObject> CheckItemsOnScreen()
    {
        List<GameObject> itemsListOnScreen = new List<GameObject>();
        PickUp[] items = UnityEngine.Object.FindObjectsOfType<PickUp>(); // 此处PickUp可替换为任一所有item独有的挂载脚本名
        // Debug.Log("item caught");
        foreach (PickUp scriptObject in items)
        {
            // 将包含特定脚本的游戏物体添加到列表中
            itemsListOnScreen.Add(scriptObject.gameObject);
        }

        return itemsListOnScreen;
    }

    public static void InitiateItems()
    {
        List<GameObject> itemsListOnScreen = CheckItemsOnScreen();
        List<int> itemIdsOnScreen = new List<int>();

        int n = 0; //屏幕上物品的数量
        // 获得屏幕上所有物品的id
        foreach (GameObject item in itemsListOnScreen)
        {
            item.GetComponent<SpriteRenderer>().enabled = true; // 确保一开始所有物品都存在
            int id = ItemsInfo.getInstance().getId(item.name);
            itemIdsOnScreen.Add(id);
            n++;
        }


        List<int> goodsIds = PickedItems.getInstance().pickedItems;
        List<int> usedGoodsIds = PickedItems.getInstance().usedItems;

        // 通过遍历已获得未使用和已获得已使用的物品id，查找屏幕上应该不复存在的物品
        for (int i = 0; i<n; i++)
        {
            foreach (int gId in goodsIds)
            {
                if (itemIdsOnScreen[i] == gId)
                {
                    itemsListOnScreen[i].GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            foreach(int ugId in usedGoodsIds)
            {
                if (itemIdsOnScreen[i] == ugId)
                {
                    itemsListOnScreen[i].GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }

        // Debug.Log("ok");
    }

}
