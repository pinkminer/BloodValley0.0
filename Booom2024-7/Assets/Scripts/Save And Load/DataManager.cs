using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DataManager : MonoBehaviour
{
    private string saveFolderPath;

    public List<Button> saveButtons;  // List to store the 6 save buttons
    public List<Button> loadButtons;  // List to store the 6 load buttons

    [SerializeField] private Inventory inventory; // Reference to the Inventory script

    [System.Serializable]
    public class SaveData
    {
        public List<int> goodsIds;  // Use List<int> to store multiple item IDs
        public List<int> usedGoodsIds; /* 将已获得和使用的物品从场景里清除
         * 因为已有代码的逻辑是，当合成系统将背包中Items消耗后，此物品也会从
         * PickedItems.pickedItems中删除，移到另一个列表里，这里先写一个
         * usedGoodsIds用来接收这些已经用过的物品id，然后清除它以及goodsIds
         * 中所有的物品。
         */
    }

    void Awake()
    {
        saveFolderPath = Path.Combine(Application.persistentDataPath, "Saves");

        // Create the save folder if it doesn't exist
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
    }

    void Start()
    {
        // Ensure we have exactly 6 buttons
        if (saveButtons.Count != 6 || loadButtons.Count != 6)
        {
            Debug.LogError("Please assign exactly 6 save buttons and 6 load buttons in the Inspector.");
            return;
        }

        // Assign each button to its corresponding save and load slot
        for (int i = 0; i < 6; i++)
        {
            int slot = i + 1;  // Slots are 1-6, so add 1 to index
            saveButtons[i].onClick.AddListener(() => OnSaveButtonClicked(slot));
            loadButtons[i].onClick.AddListener(() => OnLoadButtonClicked(slot));

            // Check if the slot has a save, and update save button text accordingly
            UpdateButtonText(saveButtons[i], slot);

            // Ensure load buttons always display "Load"
            TextMeshProUGUI loadButtonText = loadButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (loadButtonText != null)
            {
                loadButtonText.text = "Load";
            }
        }
    }

    void UpdateButtonText(Button button, int slot)
    {
        string saveFilePath = GetSaveFilePath(slot);
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        if (File.Exists(saveFilePath))
        {
            buttonText.text = "Saved";
        }
        else
        {
            buttonText.text = "Empty";
        }
    }

    void OnSaveButtonClicked(int slot)
    {
        // Save picked items to the selected slot
        if (PickedItems.getInstance().pickedItems.Count > 0)
        {
            SaveItemIds(slot, PickedItems.getInstance().pickedItems, 
                PickedItems.getInstance().usedItems);
        }

        // Update button text after save
        UpdateButtonText(saveButtons[slot - 1], slot);
    }

    // Load item IDs from a specific slot (1-6) and update the inventory
    public void OnLoadButtonClicked(int slot)
    {
        List<int> loadedGoodsIds = LoadItemIds(slot);
        UpdateInventory(loadedGoodsIds);

        // Load button text remains "Load"
    }

    // Save item IDs to a specific slot (1-6)
    public void SaveItemIds(int slot, List<int> goodsIds, List<int> usedGoodsIds)
    {
        if (slot < 1 || slot > 6)
        {
            Debug.LogError("Invalid slot number. Please use a slot between 1 and 6.");
            return;
        }

        SaveData data = new SaveData();
        data.goodsIds = goodsIds;
        data.usedGoodsIds = usedGoodsIds;

        string json = JsonUtility.ToJson(data);
        string saveFilePath = GetSaveFilePath(slot);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Item IDs saved to JSON slot " + slot + ": " + string.Join(", ", goodsIds));
    }

    // Load item IDs from a specific slot (1-6)
    public List<int> LoadItemIds(int slot)
    {
        if (slot < 1 || slot > 6)
        {
            Debug.LogError("Invalid slot number. Please use a slot between 1 and 6.");
            return new List<int>();  // Return an empty list if the slot is invalid
        }

        string saveFilePath = GetSaveFilePath(slot);

        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("Item IDs loaded from JSON slot " + slot + ": " + string.Join(", ", data.goodsIds));
            return data.goodsIds;
        }
        else
        {
            Debug.LogWarning("No saved item IDs found in slot " + slot);
            return new List<int>();  // Return an empty list if no data found
        }
    }

    // Update the inventory cells with the loaded item IDs
    private void UpdateInventory(List<int> goodsIds)
    {
        // Update the picked items list
        PickedItems.getInstance().pickedItems = goodsIds;

        // Update the inventory UI
        inventory.ItemUpdate();
    }

    // Get the file path for a specific save slot
    private string GetSaveFilePath(int slot)
    {
        return Path.Combine(saveFolderPath, "savedItem_slot" + slot + ".json");
    }

}
