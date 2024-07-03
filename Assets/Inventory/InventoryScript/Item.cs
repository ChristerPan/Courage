using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName ="New Item",menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    
    public string itemName;
    public Sprite itemImage;
    public int itemHeld;
    
    public bool Equip;
    public bool potion;
    public int level=1;
    public float Attack;
    public float Defense;
    public float Speed;
    public float Health;
    public float restoreValue; //恢復值
    public int price;//價格
    public string potionId;

    [TextArea]
    public string shopInfo;
    [TextArea]
    public string itemInfo;
    public enum EquipType { Head, Hand, Body, Foot, Null } //列舉裝備類型
    public EquipType equipType = EquipType.Null;

    public void Load(ItemData item)
    {
        itemName = item.itemName;
        itemImage = Resources.Load<Item>("Items/" + item.assetsName.Replace("(Clone)", string.Empty)).itemImage;
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
