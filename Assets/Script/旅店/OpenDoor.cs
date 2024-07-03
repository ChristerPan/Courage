using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject R;
    public Sprite openDoorSprite;
    public Sprite closeDoorSprite;
    public bool isOpen;
    public BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (R.activeSelf && Input.GetKeyDown(KeyCode.R) && isOpen)
        {
            boxCollider.enabled = true;
            spriteRenderer.sprite = closeDoorSprite;
            isOpen = false;
        }
        else if (R.activeSelf && Input.GetKeyDown(KeyCode.R) && !isOpen)
        {
            boxCollider.enabled = false;
            spriteRenderer.sprite = openDoorSprite;
            isOpen = true;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        R.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        R.SetActive(false);
    }

}
