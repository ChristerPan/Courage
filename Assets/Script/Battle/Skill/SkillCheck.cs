using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheck : MonoBehaviour
{
    public static bool canUse;
    public void WaffleCheck1()
    {

        if (GameObject.Find("Wolf")==null)
        {
            canUse = true;
        }
        else
        {
            canUse = false;
        }
        //GameObject obj = GameObject.Find("Waffle");


        //if (obj.transform.GetChild(0).childCount == 0 && obj.transform.GetChild(1).childCount == 0)
        //{
        //    canUse = true;
        //}
        //else
        //{
        //    canUse = false;
        //}
    }
}
