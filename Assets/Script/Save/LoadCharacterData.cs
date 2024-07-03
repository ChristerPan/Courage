using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacterData : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float currentDefence;
    public float currentSpeed;
    public float currentAttack;
    public int currentMoney;

    public LoadCharacterData(CharacterData_SO characterData_SO)
    {
        maxHealth = characterData_SO.maxHealth;
        currentHealth = characterData_SO.currentHealth;
        currentDefence = characterData_SO.currentDefence;
        currentSpeed = characterData_SO.currentSpeed;
        currentAttack = characterData_SO.currentAttack;
        currentMoney = characterData_SO.currentMoney;
    }
}
