using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshPropsItem : MonoBehaviour
{
    //static InventoryManger instance;
    public Inventory myProps;
    public GameObject propsGrid;
    public GameObject emptySlot;
    public List<GameObject> propsSlots = new List<GameObject>();
    
    public void CreateProps()
    {
        //重新生成對應propsGrid裡面的物品的slot
        for (int i = 0; i < myProps.itemList.Count; i++)
        {
            GameObject obj = Instantiate(emptySlot);
            obj.name = "Props"+ i;
            Destroy(obj.GetComponentInChildren<ItemOnDrag>());
            propsSlots.Add(obj);
            propsSlots[i].transform.SetParent(propsGrid.transform);
            propsSlots[i].transform.localScale = new Vector3(1, 1, 1);
            propsSlots[i].GetComponent<Slot>().slotID = i;
            propsSlots[i].GetComponent<Slot>().SetupSlot(myProps.itemList[i]);
            if (myProps.itemList[i] != null && myProps.itemList[i].potion)
            {
                Slot slot = propsSlots[i].GetComponent<Slot>();
                int temp = i;
                slot.slotBtn.onClick.AddListener(() => 
                {
                    slot.ItemOnClicked(myProps.itemList[temp].potionId);
                });
            }
        }
    }

    public void DestoryProps()
    {
        //循環刪除propsGrid下的子集物體
        for (int i = 0; i < propsGrid.transform.childCount; i++)
        {
            if (propsGrid.transform.childCount == 0)
                break;
            Destroy(propsGrid.transform.GetChild(i).gameObject);
        }
        propsSlots.Clear();
    }
}
