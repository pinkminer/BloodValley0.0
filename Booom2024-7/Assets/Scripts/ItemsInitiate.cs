using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInitiate : MonoBehaviour
{
    public static List<GameObject> CheckItemsOnScreen()
    {
        List<GameObject> itemsListOnScreen = new List<GameObject>();
        PickUp[] items = UnityEngine.Object.FindObjectsOfType<PickUp>(); // �˴�PickUp���滻Ϊ��һ����item���еĹ��ؽű���
        // Debug.Log("item caught");
        foreach (PickUp scriptObject in items)
        {
            // �������ض��ű�����Ϸ������ӵ��б���
            itemsListOnScreen.Add(scriptObject.gameObject);
        }

        return itemsListOnScreen;
    }

    public static void InitiateItems()
    {
        List<GameObject> itemsListOnScreen = CheckItemsOnScreen();
        List<int> itemIdsOnScreen = new List<int>();

        int n = 0; //��Ļ����Ʒ������
        // �����Ļ��������Ʒ��id
        foreach (GameObject item in itemsListOnScreen)
        {
            item.GetComponent<SpriteRenderer>().enabled = true; // ȷ��һ��ʼ������Ʒ������
            int id = ItemsInfo.getInstance().getId(item.name);
            itemIdsOnScreen.Add(id);
            n++;
        }


        List<int> goodsIds = PickedItems.getInstance().pickedItems;
        List<int> usedGoodsIds = PickedItems.getInstance().usedItems;

        // ͨ�������ѻ��δʹ�ú��ѻ����ʹ�õ���Ʒid��������Ļ��Ӧ�ò������ڵ���Ʒ
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
