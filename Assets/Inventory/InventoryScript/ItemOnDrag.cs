using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    public Inventory myBag, myEquip, myProps, enhancedList;
    public int originalItemID;//��e���~ID
    public Item.EquipType originalType;
    public string originalname = "";

    public State state;

    void Start()
    {
        state = FindObjectOfType<State>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {



        originalname = transform.name;//�쥻���W�r
        originalParent = transform.parent;//�쥻������
        transform.position = eventData.position;//�ثe���Ц�m
        transform.SetParent(transform.parent.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;//�g�u��������

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
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);//��X���з�e��m�U��Ĥ@�ӸI�쪺����W�r
    }



    public void OnEndDrag(PointerEventData eventData)
    {
        //�k��
        transform.SetParent(originalParent);
        transform.position = originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;//�g�u���׶}��, ���M�L�k�A���襤���ʪ����~

        GetComponentInChildren<Image>().canvas.sortingOrder -= 10;

        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            //���~�O�q�I�]�즲
            if (originalParent.name == "BagSlot")
            {
                //��e���Ы��쪺����W�r�OBagItemImg ���򤬴���m
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagItemImg")
                {

                    //����Slot�̪����
                    var itemTemp = originalParent.GetComponent<Slot>().slotItem;
                    originalParent.GetComponent<Slot>().SetupSlot(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponentInParent<Slot>().slotItem);
                    eventData.pointerCurrentRaycast.gameObject.transform.parent
                      .GetComponentInParent<Slot>().SetupSlot(itemTemp);


                    //���ܸ�Ʈw�ƾ�
                    var temp = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;

                    return;
                }

                //��e���Ы��쪺����W�r�OBagSlot �N��S�F��
                if (eventData.pointerCurrentRaycast.gameObject.transform.name == "BagSlot")
                {
                    //�ѨM�ۤv��b�ۤv��m�����D
                    if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID == originalItemID)
                        return;
                    //����Slot�̪����
                    eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //���ܸ�Ʈw�ƾ�
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = null;
                    return;
                }

                //��e���Ы��쪺����W�O EquipSlot �N��ثe�L�˳� �åB�˳������@�˴N��W
                if (eventData.pointerCurrentRaycast.gameObject.transform.name == "EquipSlot" && eventData.pointerCurrentRaycast.gameObject.transform.GetComponent<Slot>().equipType == originalType)
                {

                    //����Slot�̪����
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //���ܸ�Ʈw�ƾ�
                    myEquip.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = null;

                    //��s��O��
                    state.UpdateStatus();
                    Debug.Log("��s��O��");
                    return;
                }
                //��e���Ы��쪺����W�O EquipItemImg �N��ثe���˳� �åB�˳����O�@�˴N�洫�ƾ�
                if (eventData.pointerCurrentRaycast.gameObject.transform.name == "EquipItemImg" && eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().equipType == originalType)
                {
                    //����Slot�̪����
                    var itemTemp = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotItem;
                    eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(itemTemp);


                    //���ܸ�Ʈw�ƾ�
                    var temp = myBag.itemList[originalParent.GetComponent<Slot>().slotID];
                    myBag.itemList[originalParent.GetComponent<Slot>().slotID] = myEquip.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID];
                    myEquip.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID] = temp;

                    //��s��O��
                    state.UpdateStatus();
                    Debug.Log("��s��O��");
                    return;
                }

                //��e���Ы��쪺����W�OPropsSlot�N��ثe�L�D�� �åB���O�˳� �O�Ĥ��N�ˤW
                if (eventData.pointerCurrentRaycast.gameObject.name == "PropsSlot" && originalParent.GetComponent<Slot>().slotItem.Equip == false && originalParent.GetComponent<Slot>().slotItem.potion == true)
                {
                    //����Slot�̪����
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //���ܸ�Ʈw�ƾ�
                    myProps.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = null;

                    return;

                }

                //��e���Ы��쪺����W�OPropsItemImg�N��ثe���D��A�åB���O�˳� �O�Ĥ��N�洫�ƾ�
                if (eventData.pointerCurrentRaycast.gameObject.name == "PropsItemImg" && originalParent.GetComponent<Slot>().slotItem.Equip == false && originalParent.GetComponent<Slot>().slotItem.potion ==true)
                {
                    //����Slot�̪����
                    var itemTemp = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotItem;
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.
                        GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(itemTemp);

                    //���ܸ�Ʈw�ƾ�
                    var temp = myProps.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID];
                    myProps.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID] = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = temp;

                    return;

                }

                //��e���Ы��쪺����W�OEnhancedSlot�N����j���� �åB�O�˳�
                if (eventData.pointerCurrentRaycast.gameObject.name == "EnhancedSlot" && originalParent.GetComponent<Slot>().slotItem.Equip == true)
                {
                    //����Slot�̪����
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //���ܸ�Ʈw�ƾ�
                    enhancedList.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemList[originalItemID];
                    myBag.itemList[originalItemID] = null;

                    return;
                }
            }
            //�q�԰��D��즲
            else if (originalParent.name == "PropsSlot")
            {
                //���Ы��쪺����W�OBagSlot �N��S�F��
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagSlot")
                {
                    //����Slot�̪����
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //���ܸ�Ʈw�ƾ�
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = myProps.itemList[originalItemID];
                    myProps.itemList[originalItemID] = null;
                    return;
                }

                //���Ы��쪺����W�OBagItemImg �N���F�� �åB���O�˳� �O�Ĥ��N�洫
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagItemImg" && eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotItem.Equip == false && eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotItem.potion == true)
                {
                    //����Slot�̪����
                    var itemTemp = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotItem;
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.
                        GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(itemTemp);

                    //���ܸ�Ʈw�ƾ�
                    var temp = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID];
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID] = myProps.itemList[originalItemID];
                    myProps.itemList[originalItemID] = temp;
                    return;
                }

                //���Ы��쪺����W�OPropsSlot �N��S�F��
                if (eventData.pointerCurrentRaycast.gameObject.name == "PropsSlot")
                {
                    //�ѨM�ۤv��b�ۤv��m�����D
                    if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID == originalItemID)
                        return;
                    //����Slot�̪����
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //���ܸ�Ʈw�ƾ�
                    myProps.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = myProps.itemList[originalItemID];
                    myProps.itemList[originalItemID] = null;
                    return;
                }

                //���Ы��쪺����W�OPropsItemImg �N���F��
                if (eventData.pointerCurrentRaycast.gameObject.name == "PropsItemImg")
                {
                    //����Slot�̪����
                    var itemTemp = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotItem;
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.
                        GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(itemTemp);

                    //���ܸ�Ʈw�ƾ�
                    var temp = myProps.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID];
                    myProps.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID] = myProps.itemList[originalItemID];
                    myProps.itemList[originalItemID] = temp;
                }
            }
            //�q�˳���즲
            else if (originalParent.name == "EquipSlot")
            {
                //���Ы��쪺����W�OBagItemImg �N�����~�åB�˳������@�˴N�洫
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagItemImg" && eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().equipType == originalType)
                {

                    //����Slot�̪����
                    var itemTemp = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponentInParent<Slot>().slotItem;
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.
                        GetComponentInParent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(itemTemp);

                    //���ܸ�Ʈw�ƾ�
                    var temp = myEquip.itemList[originalItemID];
                    myEquip.itemList[originalItemID] = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID];
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().slotID] = temp;

                    //��s��O��
                    state.UpdateStatus();
                    Debug.Log("��s��O��");
                    return;
                }

                //���Ы��쪺����W�OBagSlot�N��S���~
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagSlot")
                {
                    //����Slot�̪����
                    eventData.pointerCurrentRaycast.gameObject.transform.GetComponentInParent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //���ܸ�Ʈw�ƾ�
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = myEquip.itemList[originalItemID];
                    myEquip.itemList[originalParent.GetComponent<Slot>().slotID] = null;

                    //��s��O��
                    state.UpdateStatus();
                    Debug.Log("��s��O��");
                    return;
                }

                //��e���Ы��쪺����W�OEnhancedSlot�N����j����
                if (eventData.pointerCurrentRaycast.gameObject.name == "EnhancedSlot")
                {
                    //����Slot�̪����
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //���ܸ�Ʈw�ƾ�
                    enhancedList.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myEquip.itemList[originalItemID];
                    myEquip.itemList[originalItemID] = null;

                    //��s��O��
                    state.UpdateStatus();
                    return;
                }

            }
            //���~�O�q�j����즲
            else if (originalParent.name == "EnhancedSlot")
            {
                if (eventData.pointerCurrentRaycast.gameObject.name == "BagSlot")//���Ы��쪺����W�OBagSlot �N��S�F��
                {
                    //����Slot�̪����
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //���ܸ�Ʈw�ƾ�
                    myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = enhancedList.itemList[originalItemID];
                    enhancedList.itemList[originalItemID] = null;
                    return;
                }

                

                //��e���Ы��쪺����W�O EquipSlot �N��ثe�L�˳� �åB�˳������@�˴N��W
                if (eventData.pointerCurrentRaycast.gameObject.transform.name == "EquipSlot" && eventData.pointerCurrentRaycast.gameObject.transform.GetComponent<Slot>().equipType == originalType)
                {

                    //����Slot�̪����
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().SetupSlot(originalParent.GetComponent<Slot>().slotItem);
                    originalParent.GetComponent<Slot>().SetupSlot(null);

                    //���ܸ�Ʈw�ƾ�
                    myEquip.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = enhancedList.itemList[originalItemID];
                    enhancedList.itemList[originalItemID] = null;

                    //��s��O��
                    state.UpdateStatus();
                    Debug.Log("��s��O��");
                    return;
                }


            }


        }


    }

}
