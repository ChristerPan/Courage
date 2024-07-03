using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public float maxHealth;
    public float currentHealth;
    public float currentDefence;
    public float currentSpeed;
    public float currentAttack;
    public int currentMoney;

    public CharacterData(LoadCharacterData loadCharacterData)
    {
        maxHealth = loadCharacterData.maxHealth;
        currentHealth = loadCharacterData.currentHealth;
        currentDefence = loadCharacterData.currentDefence;
        currentSpeed = loadCharacterData.currentSpeed;
        currentAttack = loadCharacterData.currentAttack;
        currentMoney = loadCharacterData.currentMoney;
    }
}
