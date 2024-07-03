using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class State : MonoBehaviour
{
    public CharacterData_SO characterData;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI defenceText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI money;
    public Inventory myEquip;
    

    public static float baseHealth = 100, baseDefence = 10, baseAttack = 10;//設定基本值
    public static float baseSpeed = 10f;//設定基本值
    
    void Start()
    {
        UpdateStatus();
        UpdateMoney();
    }

    void Update()
    {
        UpdateStatus();
        UpdateMoney();
    }
    public void UpdateMoney()
    {
        money.text = characterData.currentMoney.ToString();
    }
    public void UpdateStatus()//更新裝備欄數據
    {
        float health = 0, defence = 0, speed = 0, attack = 0;
        
        
        for (int i = 0; i < myEquip.itemList.Count; i++)
        {
            if (myEquip.itemList[i] != null)//如果裝備列表第i個有裝備，就增加它的數值
            {
                //把裝備數值給變數
                health += myEquip.itemList[i].Health;
                defence += myEquip.itemList[i].Defense;
                speed += myEquip.itemList[i].Speed;
                attack += myEquip.itemList[i].Attack;
            }
            else
            {
                health += 0;
                defence += 0;
                speed += 0;
                attack += 0;
            }
        }
        //數值等於基本值加裝備數值
        characterData.maxHealth = baseHealth + health;
        characterData.currentDefence = baseDefence + defence;
        characterData.currentSpeed = baseSpeed + speed;
        characterData.currentAttack = baseAttack + attack;

        //更新裝備欄文字
        healthText.text = $"{characterData.currentHealth}";
        defenceText.text = $"{characterData.currentDefence}";
        speedText.text = $"{characterData.currentSpeed}";
        attackText.text = $"{characterData.currentAttack}";
    }
}
