using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public float maxHealth;
    public float currentHealth;
    public float currentDefence;
    public float currentSpeed;
    public float currentAttack;
    public int currentMoney;

    public void Load(CharacterData characterData)
    {
        maxHealth = characterData.maxHealth;
        currentHealth = characterData.currentHealth;
        currentDefence = characterData.currentDefence;
        currentSpeed = characterData.currentSpeed;
        currentAttack = characterData.currentAttack;
        currentMoney = characterData.currentMoney;
    }
}
