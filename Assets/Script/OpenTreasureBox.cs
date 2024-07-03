using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTreasureBox : MonoBehaviour
{
    public Sprite openSprite;
    public Item[] items;
    public int money;
    public GameObject rButton;
    public Inventory playerBag, playerProps;
    public CharacterData_SO characterData_SO;
    public GameObject treasurePrefab;
    private bool isOpen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rButton.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        rButton.SetActive(false);
    }

    void Update()
    {
        if (rButton.activeSelf && Input.GetKeyDown(KeyCode.R) && !isOpen)
        {
            GetComponent<SpriteRenderer>().sprite = openSprite;
            isOpen = true;


            Vector3 pos = transform.position;

            GameObject moneyTreasure = Instantiate(treasurePrefab, new Vector3(pos.x + Random.Range(-0.4f, 0.5f), pos.y + Random.Range(-0.4f, 0.5f), pos.z), Quaternion.identity);
            moneyTreasure.AddComponent<ItemOnworld>().money = money;
            moneyTreasure.AddComponent<BoxCollider2D>().isTrigger = true;
            moneyTreasure.GetComponent<ItemOnworld>().playerBag = playerBag;
            moneyTreasure.GetComponent<ItemOnworld>().playerProps = playerProps;
            moneyTreasure.GetComponent<ItemOnworld>().characterData_SO = characterData_SO;
            Vector3 moneyScale = moneyTreasure.transform.localScale;
            moneyTreasure.transform.localScale = new Vector3(moneyScale.x * 2, moneyScale.y * 2, moneyScale.z * 2);

            if (items.Length <= 0)
                return;

            for (int i =0; i < items.Length; i++)
            {
                Item item = Instantiate(items[i]);

                GameObject treasure = Instantiate(treasurePrefab, new Vector3(pos.x + Random.Range(-0.4f, 0.5f), pos.y + Random.Range(-0.4f, 0.5f), pos.z), Quaternion.identity);
                treasure.AddComponent<ItemOnworld>().thisItem = item;
                treasure.AddComponent<BoxCollider2D>().isTrigger = true;
                treasure.GetComponent<ItemOnworld>().playerBag = playerBag;
                treasure.GetComponent<ItemOnworld>().playerProps = playerProps;
                treasure.GetComponent<ItemOnworld>().characterData_SO = characterData_SO;
                treasure.GetComponent<SpriteRenderer>().sprite = item.itemImage;

                Vector3 treasureScale = treasure.transform.localScale;
                treasure.transform.localScale = new Vector3(treasureScale.x * 2, treasureScale.y * 2, treasureScale.z * 2);
            }
        }
    }

    
}
