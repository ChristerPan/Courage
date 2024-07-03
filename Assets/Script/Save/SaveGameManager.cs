using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveGameManager : MonoBehaviour
{
    public Inventory bag, equipment, props, enhancedList;
    public CharacterData_SO playerData_SO;
    
    const string ITEM_KEY = "/item";
    const string ITEMSID_KEY = "/itemsID";
    const string EQUIPMENT_KEY = "/equipment";
    const string EQUIPMENTID_KEY = "/equipmentID";
    const string PROPS_KEY = "/props";
    const string PROPSID_KEY = "/propsID";
    const string ENHANCEDLIST_KEY = "/enhancedList";
    const string ENHANCEDLISTID_KEY = "/enhancedListID";
    const string PLAYERDATA_KEY = "/playerData";
    const string SCENEDATA_KEY = "/sceneData";
    const string PLOTPROGRESS_KEY = "/plotProgress";

    void Awake()
    {
        //DontDestroyOnLoad(this);
        //LoadBag();
        //LoadEquipment();
        //LoadProps();
        //LoadEnhancedList();
        //LoadPlayerData();
    }

    //void OnApplicationQuit()
    //{
    //    //Save();
    //    SaveBag();
    //    SaveEquipment();
    //    SaveProps();
    //    SaveEnhancedList();
    //    SavePlayerData();
    //}

    public void SaveAll(int slot)
    {
        SaveBag(slot);
        SaveEquipment(slot);
        SaveProps(slot);
        SaveEnhancedList(slot);
        SavePlayerData(slot);
        SaveSceneData(slot);
        SavePlotProgress(slot);
    }
    public void LoadAll(int slot)
    {
        if(Directory.Exists(Application.dataPath + "/saves" + slot))
        {
            LoadBag(slot);
            LoadEquipment(slot);
            LoadProps(slot);
            LoadEnhancedList(slot);
            LoadPlayerData(slot);
            LoadSceneData(slot);
            LoadPlotProgress(slot);
        }
        else
        {
            FindObjectOfType<SaveLoadButton>().StartNewGame();
            Debug.Log("®S¶≥¶s¿…");
        }
        
    }

    public void SaveBag(int slot)
    {
        List<int> itemsID = new List<int>();

        for(int i = 0; i < bag.itemList.Count; i++)
        {
            if (bag.itemList[i] != null)
            {
                LoadItem loadItem = new LoadItem(bag.itemList[i]);
                ItemData data = new ItemData(loadItem);
                itemsID.Add(i);
                SaveSystem.Save(data, slot, "/Bag", ITEM_KEY + i);
            }
        }
        
        SaveSystem.Save(itemsID, slot, "/Bag", ITEMSID_KEY);
    }
    public void SaveEquipment(int slot)
    {
        List<int> equipmentID = new List<int>();

        for (int i = 0; i < equipment.itemList.Count; i++)
        {
            if (equipment.itemList[i] != null)
            {
                LoadItem loadItem = new LoadItem(equipment.itemList[i]);
                ItemData data = new ItemData(loadItem);
                equipmentID.Add(i);
                SaveSystem.Save(data, slot, "/Equipment", EQUIPMENT_KEY + i);
            }
        }

        SaveSystem.Save(equipmentID, slot, "/Equipment", EQUIPMENTID_KEY);
    }

    public void SaveProps(int slot)
    {
        List<int> propsID = new List<int>();

        for (int i = 0; i < props.itemList.Count; i++)
        {
            if (props.itemList[i] != null)
            {
                LoadItem loadItem = new LoadItem(props.itemList[i]);
                ItemData data = new ItemData(loadItem);
                propsID.Add(i);
                SaveSystem.Save(data, slot, "/Props", PROPS_KEY + i);
            }
        }

        SaveSystem.Save(propsID, slot, "/Props", PROPSID_KEY);
    }

    public void SaveEnhancedList(int slot)
    {
        List<int> enhancedListID = new List<int>();

        for (int i = 0; i < this.enhancedList.itemList.Count; i++)
        {
            if (this.enhancedList.itemList[i] != null)
            {
                LoadItem loadItem = new LoadItem(this.enhancedList.itemList[i]);
                ItemData data = new ItemData(loadItem);
                enhancedListID.Add(i);
                SaveSystem.Save(data, slot, "/EnhancedList", ENHANCEDLIST_KEY + i);
            }
        }

        SaveSystem.Save(enhancedListID, slot, "/EnhancedList", ENHANCEDLISTID_KEY);
    }

    public void SavePlayerData(int slot)
    {
        LoadCharacterData loadCharacter = new LoadCharacterData(playerData_SO);
        CharacterData characterData = new CharacterData(loadCharacter);
        SaveSystem.Save(characterData, slot, "/CharacterData", PLAYERDATA_KEY);
    }

    public void SaveSceneData(int slot)
    {
        LoadSceneData loadSceneData = new LoadSceneData();
        SaveSystem.Save(loadSceneData, slot, "/SceneData", SCENEDATA_KEY);
    }

    public void SavePlotProgress(int slot)
    {
        LoadPlotProgress loadPlotProgress = new LoadPlotProgress();
        SaveSystem.Save(loadPlotProgress, slot, "/PlotProgress", PLOTPROGRESS_KEY);
    }

    public void LoadBag(int slot)
    {
        List<int> itemsID = SaveSystem.Load<List<int>>(slot, "/Bag", ITEMSID_KEY);

        for (int i = 0; i < itemsID.Count; i++)
        {
            int id = itemsID[i];
            ItemData itemData = SaveSystem.Load<ItemData>(slot, "/Bag", ITEM_KEY + id);
            Item item = Instantiate(Resources.Load<Item>("Items/" + itemData.assetsName.Replace("(Clone)", string.Empty)));
            item.Load(itemData);
            bag.itemList[id] = item;
        }
    }

    public void LoadEquipment(int slot)
    {
        List<int> equipmentID = SaveSystem.Load<List<int>>(slot, "/Equipment", EQUIPMENTID_KEY);

        for (int i = 0; i < equipmentID.Count; i++)
        {
            int id = equipmentID[i];
            ItemData itemData = SaveSystem.Load<ItemData>(slot, "/Equipment", EQUIPMENT_KEY + id);
            Item item = Instantiate(Resources.Load<Item>("Items/" + itemData.assetsName.Replace("(Clone)", string.Empty)));
            item.Load(itemData);
            equipment.itemList[id] = item;
        }
    }

    public void LoadProps(int slot)
    {
        List<int> propsID = SaveSystem.Load<List<int>>(slot, "/Props", PROPSID_KEY);

        for (int i = 0; i < propsID.Count; i++)
        {
            int id = propsID[i];
            ItemData itemData = SaveSystem.Load<ItemData>(slot, "/Props", PROPS_KEY + id);
            Item item = Instantiate(Resources.Load<Item>("Items/" + itemData.assetsName.Replace("(Clone)", string.Empty)));
            item.Load(itemData);
            props.itemList[id] = item;
        }
    }

    public void LoadEnhancedList(int slot)
    {
        List<int> enhancedListID = SaveSystem.Load<List<int>>(slot, "/EnhancedList", ENHANCEDLISTID_KEY);

        for (int i = 0; i < enhancedListID.Count; i++)
        {
            int id = enhancedListID[i];
            ItemData itemData = SaveSystem.Load<ItemData>(slot, "/EnhancedList", ENHANCEDLIST_KEY + id);
            Item item = Instantiate(Resources.Load<Item>("Items/" + itemData.assetsName.Replace("(Clone)", string.Empty)));
            item.Load(itemData);
            enhancedList.itemList[id] = item;
        }
    }

    public void LoadPlayerData(int slot)
    {
        CharacterData characterData = SaveSystem.Load<CharacterData>(slot, "/CharacterData", PLAYERDATA_KEY);
        playerData_SO.Load(characterData);
    }

    public void LoadSceneData(int slot)
    {
        LoadSceneData sceneData = SaveSystem.Load<LoadSceneData>(slot, "/SceneData", SCENEDATA_KEY);
        SceneData.Load(sceneData);
    }

    public void LoadPlotProgress(int slot)
    {
        LoadPlotProgress loadPlotProgress = SaveSystem.Load<LoadPlotProgress>(slot, "/PlotProgress", PLOTPROGRESS_KEY);
        PlotProgress.Load(loadPlotProgress);
    }
}
