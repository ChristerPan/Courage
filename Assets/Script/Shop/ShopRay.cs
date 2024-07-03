using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRay : MonoBehaviour
{
    public Item thisItem;
    public GameObject purchasedPanel;
    public PurchaseSystem purchaseSystem;
    void Start()
    {
        purchaseSystem = purchasedPanel.GetComponent<PurchaseSystem>();
    }
    public void AddNewItem()
    {
        purchaseSystem.thisItem = thisItem;
        purchasedPanel.SetActive(true);
        
    }
}
