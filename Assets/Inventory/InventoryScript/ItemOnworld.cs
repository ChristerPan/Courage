using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnworld : MonoBehaviour
{
    public Item thisItem;
    public int money;
    public Inventory playerBag, playerProps;
    public CharacterData_SO characterData_SO;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (thisItem != null)
            {
                Item item = Instantiate(thisItem);
                AddNewItem(item);
            }
            characterData_SO.currentMoney += money;
            Destroy(gameObject);
        }
    }
    public void AddNewItem(Item item)
    {
        if (item.Equip == false)
        {
            for (int i = 0; i < playerProps.itemList.Count; i++)
            {
                if (playerProps.itemList[i] != null && playerProps.itemList[i].itemName == item.itemName)
                {
                    playerProps.itemList[i].itemHeld += 1;
                    InventoryManger.RefreshPropsItem();
                    return;
                }
            }
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i] != null && playerBag.itemList[i].itemName == item.itemName)
                {
                    playerBag.itemList[i].itemHeld += 1;
                    InventoryManger.RefreshBag();
                    return;
                }
            }
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i] == null)
                {
                    playerBag.itemList[i] = item;
                    item.itemHeld += 1;
                    InventoryManger.RefreshBag();
                    return;
                }
            }

        }

        if (item.Equip == true)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i] == null)
                {
                    playerBag.itemList[i] = item;
                    InventoryManger.RefreshBag();
                    break;
                }
            }

        }
    }
}
