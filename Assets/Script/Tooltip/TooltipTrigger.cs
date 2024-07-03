using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Slot slot;
    private static LTDescr delay;
    public string header;
    

    [Multiline()]
    public string content;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        
        if (string.IsNullOrEmpty(header) && string.IsNullOrEmpty(content))
        {
            slot = GetComponent<Slot>();
            if (slot != null && slot.slotItem != null)
            {
                SetText();
            }
        }

        delay = LeanTween.delayedCall(0.1f, () =>
        {
            TooltipSystem.Show(content, header);
        });
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }

    public void SetText()
    {
        header = slot.slotItem.itemName;
        content = slot.slotItem.itemInfo;
        if (slot.slotItem.Attack != 0) content += "\n攻擊+" + slot.slotItem.Attack;
        if (slot.slotItem.Defense != 0) content += "\n防禦+" + slot.slotItem.Defense;
        if (slot.slotItem.Health != 0) content += "\n血量+" + slot.slotItem.Health;
        if (slot.slotItem.Speed != 0) content += "\n速度+" + slot.slotItem.Speed;
        if (slot.slotItem.Equip == true)
        {
            content += "\n等級 " + slot.slotItem.level;
        }
    }
}
