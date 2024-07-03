using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadItem : MonoBehaviour
{
    public string assetsName;
    public string itemName;
    //public Sprite itemImage;
    public int itemHeld;

    public bool Equip;
    public bool potion;
    public int level;
    public float Attack;
    public float Defense;
    public float Speed;
    public float Health;
    public float restoreValue; //«ì´_­È
    public int price;//»ù®æ
    public string potionId;

    [TextArea]
    public string shopInfo;
    [TextArea]
    public string itemInfo;

    public Item.EquipType equipType;

    public LoadItem(Item item)
    {
        assetsName = item.name;
        itemName = item.itemName;
        //itemImage = item.itemImage;
        itemHeld = item.itemHeld;
        Equip = item.Equip;
        potion = item.potion;
        level = item.level;
        Attack = item.Attack;
        Defense = item.Defense;
        Speed = item.Speed;
        Health = item.Health;
        restoreValue = item.restoreValue;
        price = item.price;
        potionId = item.potionId;
        shopInfo = item.shopInfo;
        itemInfo = item.itemInfo;
        equipType = item.equipType;
    }
}
