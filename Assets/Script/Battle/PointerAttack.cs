using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerAttack : MonoBehaviour//掛在攻擊圈上
{
    public GameObject attackCircle;
    public Transform attackTarget, attackClock;


    void OnEnable()
    {
        //attackClock.transform.rotation = Quaternion.Euler(0f, 0f, 0f);//指針回到最上面
        //attackTarget.eulerAngles = new Vector3(0, 0, Random.Range(45, 320));//有效區角度範圍
    }

    
}
