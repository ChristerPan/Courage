using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManger : MonoBehaviour
{
    static InventoryManger instance;

    public Inventory myBag, myEquip, myProps;
    public GameObject bagGrid, propsGrid;
    //public Slot SlotPerfab;
    public GameObject emptySlot, emptyEquipSlot;
    public Text itemInfromation;

    public List<GameObject> slots = new List<GameObject>();//管理背包生成的18個slots
    public List<GameObject> equipSlots = new List<GameObject>();//生成裝備的Slots
    public List<GameObject> propsSlots = new List<GameObject>();//生成戰鬥道具的Slots
    public GameObject[] equipTr;//裝備欄裡4個裝備的位置
    public Sprite[] equipImage; //裝備欄裡4個裝備的圖
    void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }
    void Start()
    {
        RefreshBag();
        RefreshEquipItem();
        RefreshPropsItem();
        //instance.itemInfromation.text = "";
    }
    

    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInfromation.text = itemDescription;
    }

    /*public static void CreatNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.SlotPerfab,instance.slotGrid.transform.position,Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemHeld.ToString();
    }*/

    public static void RefreshBag()//刷新背包物品
    {
        //循環刪除slotGrid下的子集物體
        for (int i = 0; i < instance.bagGrid.transform.childCount; i++)
        {
            if (instance.bagGrid.transform.childCount == 0)
                return;
            Destroy(instance.bagGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }
        //重新生成對應myBag裡面的物品的slot
        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            //CreatNewItem(instance.myBag.itemList[i]);
            GameObject obj = Instantiate(instance.emptySlot);
            obj.name = "BagSlot";
            obj.transform.GetChild(0).GetChild(0).name = "BagItemImg";
            
            
            instance.slots.Add(obj);
            instance.slots[i].transform.SetParent(instance.bagGrid.transform);
            instance.slots[i].transform.localScale = new Vector3(1, 1, 1);
            instance.slots[i].GetComponent<Slot>().slotID = i;
            instance.slots[i].GetComponent<Slot>().SetupSlot(instance.myBag.itemList[i]);
            
        }
    }

    public static void RefreshEquipItem()//刷新裝備
    {
        //循環刪除equipTr下的子集物體
        for (int i = 0; i < instance.equipTr.Length; i++)
        {
            for (int k = 0; k < instance.equipTr[i].transform.childCount; k++)
            {
                if (instance.equipTr[i].transform.childCount == 0)
                    return;
                Destroy(instance.equipTr[i].transform.GetChild(k).gameObject);
                instance.equipSlots.Clear();
            }
        }
        //重新生成對應myEquip裡面的物品的slot
        for (int i = 0; i < instance.myEquip.itemList.Count; i++)
        {
            GameObject obj = Instantiate(instance.emptySlot);
            obj.name = "EquipSlot";
            obj.transform.GetChild(0).GetChild(0).name = "EquipItemImg";
            obj.GetComponent<Slot>().equipType = (Item.EquipType)i;// Item.EquipType.Head;
            obj.GetComponent<Image>().sprite = instance.equipImage[i];
            
            instance.equipSlots.Add(obj);
            instance.equipSlots[i].transform.SetParent(instance.equipTr[i].transform);
            instance.equipSlots[i].transform.localScale = new Vector3(1, 1, 1);
            instance.equipSlots[i].transform.position = instance.equipTr[i].transform.position;
            instance.equipSlots[i].GetComponent<Slot>().slotID = i;
            instance.equipSlots[i].GetComponent<Slot>().SetupSlot(instance.myEquip.itemList[i]);
        }


    }

    public static void RefreshPropsItem()//刷新道具
    {
        //循環刪除propsGrid下的子集物體
        for (int i = 0; i < instance.propsGrid.transform.childCount; i++)
        {
            if (instance.propsGrid.transform.childCount == 0)
                return;
            Destroy(instance.propsGrid.transform.GetChild(i).gameObject);
            instance.propsSlots.Clear();
        }
        //重新生成對應propsGrid裡面的物品的slot
        for (int i = 0; i < instance.myProps.itemList.Count; i++)
        {
            GameObject obj = Instantiate(instance.emptySlot);
            obj.name = "PropsSlot";
            obj.transform.GetChild(0).GetChild(0).name = "PropsItemImg";
            instance.propsSlots.Add(obj);
            instance.propsSlots[i].transform.SetParent(instance.propsGrid.transform);
            instance.propsSlots[i].transform.localScale = new Vector3(1, 1, 1);
            instance.propsSlots[i].GetComponent<Slot>().slotID = i;
            instance.propsSlots[i].GetComponent<Slot>().SetupSlot(instance.myProps.itemList[i]);
        }


    }

    /*void OnGUI()
    {
        GUI.color = Color.red;

        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("");

        System.Text.StringBuilder SB = new System.Text.StringBuilder();

        for (int i = 0; i < myBag.itemList.Count; i++)
        {
            if (myBag.itemList[i] != null)
                SB.AppendFormat("({0},{1})   ", myBag.itemList[i].name, myBag.itemList[i].equipType);
            else
                SB.AppendFormat("({0},  )   ", "---");
            if ((i + 1) % 6 == 0) { SB.Append('\n'); }

        }

        SB.Append('\n');
        SB.Append('\n');

        GameObject baggrid = GameObject.Find("Grid");
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        SB.Append(allChildren.Length.ToString());
        int ii = 0;
        for (int i = 0; i < allChildren.Length; i++)
        {
            if (allChildren[i].name == "Slot(Clone)")
            {
                Slot aa = allChildren[i].GetComponent<Slot>();
                if (aa != null && aa.slotItem != null)
                    SB.AppendFormat("({0},  )   ", aa.slotItem.itemName);
                else
                    SB.AppendFormat("({0},  )   ", "---");
                //if (myBag.itemList[i] != null)
                //    SB.AppendFormat("({0},{1})   ", myBag.itemList[i].name, myBag.itemList[i].equipType);
                //else
                //    SB.AppendFormat("({0},  )   ", "---");
                ii++;
                if (ii % 6 == 0) { SB.Append('\n'); }
            }
        }

        GUILayout.Label(SB.ToString());

        GUILayout.Label("");
        GUILayout.Label("");


    }*/
}
