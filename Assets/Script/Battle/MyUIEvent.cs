using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyUIEvent : MonoBehaviour, IPointerDownHandler
{
    public GameObject clockAttacking;

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log(this.gameObject.name + " Was Clicked.");
        clockAttacking.SendMessage("GameStart");
    }
}
