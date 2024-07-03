using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowDeleteButton : MonoBehaviour, IPointerClickHandler
{
    public Button deleteButton;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            deleteButton.gameObject.SetActive(!deleteButton.gameObject.activeSelf);
        }
    }
}
