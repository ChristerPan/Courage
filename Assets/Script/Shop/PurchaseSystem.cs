using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseSystem : MonoBehaviour
{
    public TMP_InputField inputField;
    public Text totalPriceText;
    private int totalPrice;
    public GameObject purchasedPanel;

    public CharacterData_SO characterData;
    public Item thisItem;
    public Inventory playerBag;
    public Inventory playerProps;
    public GameObject NoMoneyPlane;
    State state;
    void Start()
    {
        state = FindObjectOfType<State>();

        inputField.onValueChanged.AddListener(value =>
        {
            if (inputField.text.StartsWith("-"))
            {
                inputField.text = "";
            }
        });
    }
    void Update()
    {
        //totalPriceText.text = int.Parse(inputField.text) * thisItem.price + "¤¸";
    }


    public void IncreaseAmount()
    {
        inputField.text = (int.Parse(inputField.text) + 1).ToString();
    }

    public void ReduceAmount()
    {
        if (int.Parse(inputField.text) > 0)
        {
            inputField.text = (int.Parse(inputField.text) - 1).ToString();
        }
    }
    public void OnCancelButtonClick()
    {
        inputField.text = "1";
        purchasedPanel.SetActive(false);
    }

    public void OnConfirmButtonClick()
    {
        totalPrice = thisItem.price * int.Parse(inputField.text);
        if (characterData.currentMoney >= totalPrice)
        {
            characterData.currentMoney -= totalPrice;
            state.UpdateMoney();

            Item item = Instantiate(thisItem);
            AddNewItem(item);
            
            inputField.text = "1";
            purchasedPanel.SetActive(false);
        }
        else
        {
            NoMoneyPlane.SetActive(true);
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
                    playerProps.itemList[i].itemHeld += int.Parse(inputField.text);
                    InventoryManger.RefreshPropsItem();
                    return;
                }
            }
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i] != null && playerBag.itemList[i].itemName == item.itemName)
                {
                    playerBag.itemList[i].itemHeld += int.Parse(inputField.text);
                    InventoryManger.RefreshBag();
                    return;
                }
            }
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i] == null)
                {
                    playerBag.itemList[i] = item;
                    item.itemHeld += int.Parse(inputField.text);
                    InventoryManger.RefreshBag();
                    return;
                }
            }

        }

        if (item.Equip == true)
        {
            for(int k = 0; k < int.Parse(inputField.text); k++)
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
}
