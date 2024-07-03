using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverMonster : MonoBehaviour
{
    public GameObject mark;
    private BattleSystem battleSystem;

    void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
    }


    void OnMouseEnter()
    {
        if (battleSystem.isWaitForPlayerToChooseTarget)
            ShowMark();
    }

    void OnMouseExit()
    {
        if (battleSystem.isWaitForPlayerToChooseTarget)
            HideMark();
    }

    public void ShowMark()
    {
        mark.SetActive(true);
        BattleSystem battleSystem = FindObjectOfType<BattleSystem>();
        for(int i =0; i < battleSystem.remainingEnemyUnits.Length; i++)
        {
            if (battleSystem.remainingEnemyUnits[i] == gameObject)
            {
                battleSystem.indexOfMonster = i;
            }
            else
            {
                battleSystem.remainingEnemyUnits[i].GetComponent<MouseOverMonster>().HideMark();
            }
        }
        
    }

    public void HideMark()
    {
        mark.SetActive(false);
    }
    
}
