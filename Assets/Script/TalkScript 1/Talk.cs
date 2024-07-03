using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public GameObject Button;
    public GameObject talkUI;
    public GameObject panel;
    public PlayerController playerCtrl;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Button.SetActive(true);
        playerCtrl = other.GetComponent<PlayerController>();

        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Button.SetActive(false);
    }

    private void Update()
    {
        if ( Button.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            if (panel != null && panel.activeSelf == true)
                return;
            talkUI.SetActive(true);
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().SetBool("talk", true);
            }
            if (GetComponent<NPCTurnDirection>() != null)
            {
                GetComponent<NPCTurnDirection>().TurnDirection(playerCtrl.transform.position);
            }
        }

        if (talkUI.activeSelf == true)
        {
            playerCtrl.canMove = false;
        }
        else
        {
            //if (playerCtrl != null)
            //{
            //    playerCtrl.canMove = true;
            //}
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().SetBool("talk", false);
            }
        }


        if (panel != null && panel.activeSelf == true)
        {
            playerCtrl.canMove = false;
        }
        else if(!talkUI.activeSelf && panel != null && panel.activeSelf == false)
        {
            if (playerCtrl != null) playerCtrl.canMove = true;
        }
        //if (talkUI.activeSelf == false && Input.GetKeyDown(KeyCode.R))
        //{
        //    talkUI.SetActive(false);
        //}

        //if (talkUI.activeSelf == true)
        //{
        //    playerCtrl.canMove = false;
        //}
        //else
        //{
        //    playerCtrl.canMove = true;
        //}

    }
}
