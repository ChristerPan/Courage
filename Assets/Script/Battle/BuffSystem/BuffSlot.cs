using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;


public class BuffSlot : MonoBehaviour
{
    public Image buffSlotImage;
    public List<ScriptableBuff> scriptableBuffs;
    private int index;

    private BuffableEntity playerBuffableEntity;
    private TooltipTrigger tooltip;

    void Start()
    {
        index = 0;
        tooltip = gameObject.GetComponent<TooltipTrigger>();
        InvokeRepeating("SetUpBuffSlot", 1f, 2f);
        //Debug.Log("SetUpBuffSlot：" + IsInvoking("SetUpBuffSlot"));
    }
    void Update()
    {
        if (playerBuffableEntity == null)
        {
            playerBuffableEntity = GameObject.Find("Player_Battle").GetComponent<BuffableEntity>();
        }
        
    }

    public void SetUpBuffSlot()
    {
        scriptableBuffs = playerBuffableEntity._buffs.Keys.ToList();

        if (scriptableBuffs.Count == 0)
        {
            buffSlotImage.gameObject.SetActive(false);
            return;
        }
        else 
        {
            buffSlotImage.gameObject.SetActive(true);
            buffSlotImage.sprite = scriptableBuffs[index].buffImage;
            tooltip.header = scriptableBuffs[index].Name;
            tooltip.content = scriptableBuffs[index].description;
            index += 1;
            if(index >= scriptableBuffs.Count)
            {
                index = 0;
            }
            Debug.Log("Index：" + index);
        }
        
    }
}
