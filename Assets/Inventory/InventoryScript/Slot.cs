using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Slot : MonoBehaviour
{
    //public CharacterStats characterStats;
    public int slotID;//空格ID 等於 物品ID
    public Item slotItem;
    public Image slotImage;
    public TextMeshProUGUI slotNum;
    public string slotInfo;
    public Item.EquipType equipType;
    public GameObject itemInSlot;
    public Inventory myProps;
    public Button slotBtn;
    
    public void ItemOnClicked(string potionId)
    {
        switch (potionId)
        {
            case "補血":
                BattleSystem.currentActUnit.GetComponent<CharacterStats>().CurrentHealth += slotItem.restoreValue;
                Debug.Log("補血");
                slotItem.itemHeld -= 1;
                if (slotItem.itemHeld <= 0)//如果數量等於0
                {
                    myProps.itemList[slotID] = null; //刪除資料庫裡的item
                }
                GameObject.Find("Player_Battle").GetComponent<Animator>().SetTrigger("UseProps");
                GameObject.Find("BattleSystem").SendMessage("UseProps");
                TooltipSystem.Hide();
                break;
            case "補魔":
                FindObjectOfType<BattleSystem>().mp += (int)slotItem.restoreValue;
                Debug.Log("補魔");
                slotItem.itemHeld -= 1;
                if (slotItem.itemHeld <= 0)//如果數量等於0
                {
                    myProps.itemList[slotID] = null; //刪除資料庫裡的item
                }
                GameObject.Find("Player_Battle").GetComponent<Animator>().SetTrigger("UseProps");
                GameObject.Find("BattleSystem").SendMessage("UseProps");
                TooltipSystem.Hide();
                break;
            case "消除負面狀態":
                BuffableEntity buffableEntity = BattleSystem.currentActUnit.GetComponent<BuffableEntity>();

                List<ScriptableBuff> buffs = buffableEntity._buffs.Keys.ToList();
                for (int i = 0; i< buffableEntity._buffs.Count; i++)
                {
                    if (buffs[i].isDebuff)
                    {
                        buffableEntity._buffs.Remove(buffs[i]);
                    }
                }


                Debug.Log("消除負面狀態");
                slotItem.itemHeld -= 1;
                if (slotItem.itemHeld <= 0)//如果數量等於0
                {
                    myProps.itemList[slotID] = null; //刪除資料庫裡的item
                }
                GameObject.Find("Player_Battle").GetComponent<Animator>().SetTrigger("UseProps");
                GameObject.Find("BattleSystem").SendMessage("UseProps");
                TooltipSystem.Hide();
                break;
        }
        
        

    }

    public void SetupSlot(Item item)
    {
        if (item == null)
        {
            itemInSlot.SetActive(false);
            slotItem = null;
            slotImage.sprite = null;
            slotInfo = "";
            //equipType = Item.EquipType.Null;
            gameObject.GetComponent<TooltipTrigger>().header = null;
            gameObject.GetComponent<TooltipTrigger>().content = null;
            return;
        }
        if (item == true)
        {
            itemInSlot.SetActive(true);
        }
        slotItem = item;
        slotImage.sprite = item.itemImage;
        slotNum.text = item.itemHeld.ToString();
        slotInfo = item.itemInfo;
        equipType = item.equipType;


        if (item.Equip == true)//如果是裝備
        {
            slotNum.gameObject.SetActive(false);//把數字顯示關掉
        }
    }
}
