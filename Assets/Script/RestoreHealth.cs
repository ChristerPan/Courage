using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHealth : MonoBehaviour
{
    public CharacterData_SO characterData;
    public GameObject noMoneyPanel;
    public GameObject talkUI;
    public void RestoreCharacterHealth()
    {
        if (characterData.currentMoney >= 100)
        {
            characterData.currentMoney -= 100;
            characterData.currentHealth = characterData.maxHealth;
            talkUI.SetActive(false);
        }
        else
        {
            noMoneyPanel.SetActive(true);
        }
    }
}
