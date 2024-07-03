using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayer : MonoBehaviour
{
    //Renderer[] childrenRend;
    //int[] childernOrder;

    public Renderer spriteRenderer;


    void Start()
    {

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<Renderer>();
        //childrenRend = GetComponentsInChildren<Renderer>();
        //childernOrder = new int[childrenRend.Length];
        //for (int i = 0; i < childrenRend.Length; i++)
        //{
        //    childernOrder[i] = childrenRend[i].sortingOrder;
        //}

        //Debug.Log(gameObject.name+childrenRend.Length);
    }



    private void LateUpdate()
    {
        

        //for (int i = 0; i < childrenRend.Length; i++)
        //{
        //    childrenRend[i].sortingOrder = (int)((transform.localPosition.y + 100) * 1000 /*- childernOrder[i]*/);
        //}
        spriteRenderer.sortingOrder = -(int)((transform.position.y + 100) * 100 /*- childernOrder[i]*/);

       
    }
}
