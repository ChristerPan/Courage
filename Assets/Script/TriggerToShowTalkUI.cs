using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToShowTalkUI : MonoBehaviour
{
    public GameObject talkUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        talkUI.SetActive(true);
    }
}
