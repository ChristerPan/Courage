using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    public Inventory myBag, myEquip, myProps, enhancedList;
    public int originalItemID;//當前物品ID
    public Item.EquipType originalType;
    public string originalname = "";

    public State state;

    void Start()
    {
        state = FindObjectOfType<State>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {



        originalname = transform.name;//原本的名字
        originalParent = transform.parent;//原本的父級
        transform.position = eventData.position;//目前鼠標位置
        transform.SetParent(transform.parent.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;//射線阻擋關閉

        if (originalname == "item")
        {
            originalItemID = originalParent.GetComponent<Slot>().slotID;
            originalType = originalParent.GetComponent<Slot>().equipType;
        }

        GetComponentInChildren<Image>().canvas.sortingOrder += 10;
    }

    public void OnDrag(PointerEventData eventData)
    {


        transform.position = eventData.position;
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);//輸出鼠標當前位置下到第一個碰到的物體名字
    }



    public void OnEndDrag(PointerEventData eventData)
    {
        //歸位
        transform.SetParent(originalParent);
        transform.position = originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;//射線阻擋開啟, 不然無法再次選中移動的物品

        GetComponentInChildren<Image>().canvas.sortingOrder -= 10;

        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            //物品是從背包拖曳
            if (originalParent.name == "BagSlot")
            {
                //當前鼠標指到的物體名字是BagItemImg 那麼互換位置
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagItemImg")
                {

                    //改變Slot裡的資料
                    var itemTemp = originalParent.GetComponent<Slot>().slotItem;
                    originalParent.GetComponent<Slot>().SetupSlot(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponentInParent<Slot>().slotItem);
                    eventData.pointerCurrentRaycast.gameObject.transform.parent
                      .GetComponentInParent<Slot>().SetupSlot(itemTemp);


                    //改變資料庫數據
                    var temp = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;

                    return;
                }

                //當前鼠標指到的物體名字是BagSlot 代表沒東西
                if (eventData.pointerCurrentRaycast.gameObject.transform.name == "BagSlot")
                {
                    //解決自己放在自己位置的問題
                    if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID == originalItemID)
                        return;
                    //改變Slot裡的資料
                    eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //改變資料庫數據
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = null;
                    return;
                }

                //當前鼠標指到的物體名是 EquipSlot 代表目前無裝備 並且裝備類型一樣就穿上
                if (eventData.pointerCurrentRaycast.gameObject.transform.name == "EquipSlot" && eventData.pointerCurrentRaycast.gameObject.transform.GetComponent<Slot>().equipType == originalType)
                {

                    //改變Slot裡的資料
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //改變資料庫數據
                    myEquip.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = null;

                    //更新能力值
                    state.UpdateStatus();
                    Debug.Log("更新能力值");
                    return;
                }
                //當前鼠標指到的物體名是 EquipItemImg 代表目前有裝備 並且裝備類別一樣就交換數據
                if (eventData.pointerCurrentRaycast.gameObject.transform.name == "EquipItemImg" && eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().equipType == originalType)
                {
                    //改變Slot裡的資料
                    var itemTemp = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotItem;
                    eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(itemTemp);


                    //改變資料庫數據
                    var temp = myBag.itemList[originalParent.GetComponent<Slot>().slotID];
                    myBag.itemList[originalParent.GetComponent<Slot>().slotID] = myEquip.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID];
                    myEquip.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID] = temp;

                    //更新能力值
                    state.UpdateStatus();
                    Debug.Log("更新能力值");
                    return;
                }

                //當前鼠標指到的物體名是PropsSlot代表目前無道具 並且不是裝備 是藥水就裝上
                if (eventData.pointerCurrentRaycast.gameObject.name == "PropsSlot" && originalParent.GetComponent<Slot>().slotItem.Equip == false && originalParent.GetComponent<Slot>().slotItem.potion == true)
                {
                    //改變Slot裡的資料
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //改變資料庫數據
                    myProps.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = null;

                    return;

                }

                //當前鼠標指到的物體名是PropsItemImg代表目前有道具，並且不是裝備 是藥水就交換數據
                if (eventData.pointerCurrentRaycast.gameObject.name == "PropsItemImg" && originalParent.GetComponent<Slot>().slotItem.Equip == false && originalParent.GetComponent<Slot>().slotItem.potion ==true)
                {
                    //改變Slot裡的資料
                    var itemTemp = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotItem;
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.
                        GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(itemTemp);

                    //改變資料庫數據
                    var temp = myProps.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID];
                    myProps.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID] = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = temp;

                    return;

                }

                //當前鼠標指到的物體名是EnhancedSlot代表放到強化欄 並且是裝備
                if (eventData.pointerCurrentRaycast.gameObject.name == "EnhancedSlot" && originalParent.GetComponent<Slot>().slotItem.Equip == true)
                {
                    //改變Slot裡的資料
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //改變資料庫數據
                    enhancedList.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = null;

                    return;
                }
            }
            //從戰鬥道具拖曳
            else if (originalParent.name == "PropsSlot")
            {
                //鼠標指到的物體名是BagSlot 代表沒東西
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagSlot")
                {
                    //改變Slot裡的資料
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //改變資料庫數據
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = myProps.itemList[originalItemID];
                    myProps.itemList[originalItemID] = null;
                    return;
                }

                //鼠標指到的物體名是BagItemImg 代表有東西 並且不是裝備 是藥水就交換
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagItemImg" && eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotItem.Equip == false && eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotItem.potion == true)
                {
                    //改變Slot裡的資料
                    var itemTemp = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotItem;
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.
                        GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(itemTemp);

                    //改變資料庫數據
                    var temp = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID];
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID] = myProps.itemList[originalItemID];
                    myProps.itemList[originalItemID] = temp;
                    return;
                }

                //鼠標指到的物體名是PropsSlot 代表沒東西
                if (eventData.pointerCurrentRaycast.gameObject.name == "PropsSlot")
                {
                    //解決自己放在自己位置的問題
                    if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID == originalItemID)
                        return;
                    //改變Slot裡的資料
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //改變資料庫數據
                    myProps.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = myProps.itemList[originalItemID];
                    myProps.itemList[originalItemID] = null;
                    return;
                }

                //鼠標指到的物體名是PropsItemImg 代表有東西
                if (eventData.pointerCurrentRaycast.gameObject.name == "PropsItemImg")
                {
                    //改變Slot裡的資料
                    var itemTemp = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotItem;
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.
                        GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(itemTemp);

                    //改變資料庫數據
                    var temp = myProps.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID];
                    myProps.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID] = myProps.itemList[originalItemID];
                    myProps.itemList[originalItemID] = temp;
                }
            }
            //從裝備欄拖曳
            else if (originalParent.name == "EquipSlot")
            {
                //鼠標指到的物體名是BagItemImg 代表有物品並且裝備類型一樣就交換
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagItemImg" && eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().equipType == originalType)
                {

                    //改變Slot裡的資料
                    var itemTemp = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponentInParent<Slot>().slotItem;
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.
                        GetComponentInParent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(itemTemp);

                    //改變資料庫數據
                    var temp = myEquip.itemList[originalItemID];
                    myEquip.itemList[originalItemID] = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID];
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID] = temp;

                    //更新能力值
                    state.UpdateStatus();
                    Debug.Log("更新能力值");
                    return;
                }

                //鼠標指到的物體名是BagSlot代表沒物品
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagSlot")
                {
                    //改變Slot裡的資料
                    eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInParent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //改變資料庫數據
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = myEquip.itemList[originalItemID];
                    myEquip.itemList[originalParent.GetComponent<Slot>().slotID] = null;

                    //更新能力值
                    state.UpdateStatus();
                    Debug.Log("更新能力值");
                    return;
                }

                //當前鼠標指到的物體名是EnhancedSlot代表放到強化欄
                if (eventData.pointerCurrentRaycast.gameObject.name == "EnhancedSlot")
                {
                    //改變Slot裡的資料
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //改變資料庫數據
                    enhancedList.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myEquip.itemList[originalItemID];
                    myEquip.itemList[originalItemID] = null;

                    //更新能力值
                    state.UpdateStatus();
                    return;
                }

            }
            //物品是從強化欄拖曳
            else if (originalParent.name == "EnhancedSlot")
            {
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagSlot")//鼠標指到的物體名是BagSlot 代表沒東西
                {
                    //改變Slot裡的資料
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //改變資料庫數據
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = enhancedList.itemList[originalItemID];
                    enhancedList.itemList[originalItemID] = null;
                    return;
                }

                

                //當前鼠標指到的物體名是 EquipSlot 代表目前無裝備 並且裝備類型一樣就放上
                if (eventData.pointerCurrentRaycast.gameObject.transform.name == "EquipSlot" && eventData.pointerCurrentRaycast.gameObject.transform.GetComponent<Slot>().equipType == originalType)
                {

                    //改變Slot裡的資料
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //改變資料庫數據
                    myEquip.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = enhancedList.itemList[originalItemID];
                    enhancedList.itemList[originalItemID] = null;

                    //更新能力值
                    state.UpdateStatus();
                    Debug.Log("更新能力值");
                    return;
                }


            }


        }


    }

}
