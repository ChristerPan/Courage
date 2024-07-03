using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrengthenEquipment: MonoBehaviour
{
    public Text eqmAtkTxet, eqmDfsText, eqmHealthText, eqmSpedText, atkValText, dfsValText, healthValText, spedValText;
    public Text prText, costText;
    public Inventory enhancedList;
    public GameObject emptySlot;
    public CharacterData_SO playerData;
    public List<GameObject> slots = new List<GameObject>();//管理生成的1個slots
    public Transform[] slotPos; //格子位置
    public Animator animator;

    private float enhanceProbability;//強化機率
    private int materialCost;//材料成本
    private float attackValue, defenseValue, healthValue, speedValue = 0;

    void Start()
    {
        
        RefreshItem();

    }

    void Update()
    {
        SetText();
    }

    public void EnhanceBtn()
    {
        if (enhancedList.itemList[0] == null || enhancedList.itemList[0].level >= 10 || playerData.currentMoney < materialCost)
            return;

        float randomValue = Random.value;

        if (randomValue < enhanceProbability && enhancedList.itemList[0].level < 10)
        {
            enhancedList.itemList[0].Attack += attackValue;
            enhancedList.itemList[0].Defense += defenseValue;
            enhancedList.itemList[0].Health += healthValue;
            enhancedList.itemList[0].Speed += speedValue;
            enhancedList.itemList[0].level++;
            playerData.currentMoney -= materialCost;

            slotPos[0].GetComponentInChildren<TooltipTrigger>().SetText();

            animator.SetTrigger("success");
        }
        else
        {
            //失敗

            playerData.currentMoney -= materialCost;
            animator.SetTrigger("fail");
        }
    }


    public void SetText()
    {
        if(enhancedList.itemList[0] != null)
        {
            int level = enhancedList.itemList[0].level;
            attackValue = 0;
            defenseValue = 0;
            healthValue = 0;
            speedValue = 0;

            switch (level)
            {
                case 1:
                    enhanceProbability = 1f;
                    materialCost = 20;
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Head)
                    {
                        defenseValue = 1;
                        healthValue = 1;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Body)
                    {
                        defenseValue = 1;
                        healthValue = 1;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Hand)
                    {
                        attackValue = 1;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Foot)
                    {
                        defenseValue = 1;
                        speedValue = 0.5f;
                    }
                    break;
                case 2:
                    enhanceProbability = 0.8f;
                    materialCost = 25;
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Head)
                    {
                        defenseValue = 1;
                        healthValue = 1;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Body)
                    {
                        defenseValue = 1;
                        healthValue = 1;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Hand)
                    {
                        attackValue = 1;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Foot)
                    {
                        defenseValue = 1;
                        speedValue = 0.5f;
                    }
                    break;
                case 3:
                    enhanceProbability = 0.7f;
                    materialCost = 30;
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Head)
                    {
                        defenseValue = 1;
                        healthValue = 1;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Body)
                    {
                        defenseValue = 1;
                        healthValue = 1;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Hand)
                    {
                        attackValue = 1;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Foot)
                    {
                        defenseValue = 1;
                        speedValue = 0.5f;
                    }
                    break;
                case 4:
                    enhanceProbability = 0.6f;
                    materialCost = 40;
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Head)
                    {
                        defenseValue = 1;
                        healthValue = 2;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Body)
                    {
                        defenseValue = 1;
                        healthValue = 2;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Hand)
                    {
                        attackValue = 2;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Foot)
                    {
                        defenseValue = 1;
                        speedValue = 0.5f;
                    }
                    break;
                case 5:
                    enhanceProbability = 0.5f;
                    materialCost = 50;
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Head)
                    {
                        defenseValue = 1;
                        healthValue = 2;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Body)
                    {
                        defenseValue = 1;
                        healthValue = 2;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Hand)
                    {
                        attackValue = 2;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Foot)
                    {
                        defenseValue = 1;
                        speedValue = 0.5f;
                    }
                    break;
                case 6:
                    enhanceProbability = 0.4f;
                    materialCost = 60;
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Head)
                    {
                        defenseValue = 1;
                        healthValue = 2;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Body)
                    {
                        defenseValue = 1;
                        healthValue = 2;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Hand)
                    {
                        attackValue = 2;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Foot)
                    {
                        defenseValue = 1;
                        speedValue = 0.5f;
                    }
                    break;
                case 7:
                    enhanceProbability = 0.3f;
                    materialCost = 70;
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Head)
                    {
                        defenseValue = 1;
                        healthValue = 3;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Body)
                    {
                        defenseValue = 1;
                        healthValue = 3;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Hand)
                    {
                        attackValue = 3;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Foot)
                    {
                        defenseValue = 1;
                        speedValue = 0.5f;
                    }
                    break;
                case 8:
                    enhanceProbability = 0.2f;
                    materialCost = 80;
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Head)
                    {
                        defenseValue = 1;
                        healthValue = 3;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Body)
                    {
                        defenseValue = 1;
                        healthValue = 3;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Hand)
                    {
                        attackValue = 3;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Foot)
                    {
                        defenseValue = 1;
                        speedValue = 0.5f;
                    }
                    break;
                case 9:
                    enhanceProbability = 0.2f;
                    materialCost = 90;
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Head)
                    {
                        defenseValue = 1;
                        healthValue = 3;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Body)
                    {
                        defenseValue = 1;
                        healthValue = 3;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Hand)
                    {
                        attackValue = 3;
                    }
                    if (enhancedList.itemList[0].equipType == Item.EquipType.Foot)
                    {
                        defenseValue = 1;
                        speedValue = 0.5f;
                    }
                    break;
                case 10:
                    enhanceProbability = 0f;
                    materialCost = 0;
                    break;
            }

            

            eqmAtkTxet.text = enhancedList.itemList[0].Attack.ToString();
            eqmDfsText.text = enhancedList.itemList[0].Defense.ToString();
            eqmHealthText.text = enhancedList.itemList[0].Health.ToString();
            eqmSpedText.text = enhancedList.itemList[0].Speed.ToString();

            atkValText.text = (enhancedList.itemList[0].Attack + attackValue).ToString();
            dfsValText.text = (enhancedList.itemList[0].Defense + defenseValue).ToString();
            healthValText.text = (enhancedList.itemList[0].Health + healthValue).ToString();
            spedValText.text = (enhancedList.itemList[0].Speed + speedValue).ToString();

            prText.text = (enhanceProbability * 100).ToString() + "%";
            costText.text = materialCost.ToString();
        }
        else
        {
            eqmAtkTxet.text = "";
            eqmDfsText.text = "";
            eqmHealthText.text = "";
            eqmSpedText.text = "";

            atkValText.text = "";
            dfsValText.text = "";
            healthValText.text = "";
            spedValText.text = "";

            prText.text = "";
            costText.text = "";
        }
    }

    public void RefreshItem()//刷新物品
    {
        //循環刪除slotGrid下的子集物體
        for (int i = 0; i < slotPos.Length; i++)
        {
            if (slotPos[i].childCount == 0)
                break;
            Destroy(slotPos[i].GetChild(0));
            slots.Clear();
        }
        //重新生成對應myBag裡面的物品的slot
        for (int i = 0; i < slotPos.Length; i++)
        {
            slots.Add(Instantiate(emptySlot, slotPos[i].position, Quaternion.identity, slotPos[i]));
            slots[i].GetComponent<Slot>().slotID = i;
            slots[i].name = "EnhancedSlot";
            slots[i].GetComponent<Slot>().SetupSlot(enhancedList.itemList[i]);
        }
    }
}
