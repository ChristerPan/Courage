using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TootlShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ShopRay shopray;
    private static LTDescr delay;
    public string header;


    [Multiline()]
    public string content;
    public string howmuch;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        
        if (string.IsNullOrEmpty(header) && string.IsNullOrEmpty(content))
        {
            shopray = GetComponent<ShopRay>();
            header = shopray.thisItem.itemName;
            content = shopray.thisItem.itemInfo;
            howmuch = shopray.thisItem.price.ToString() + "元";
        }

        delay = LeanTween.delayedCall(0.1f, () =>
        {
            TooltipSystem.Show(content, header, howmuch);
        });

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }
}
