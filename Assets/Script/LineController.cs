using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public int canAttack;
    private float speed = 500f;
    public Transform[] movePos;

    private int i;

    public GameObject attackBar;
    

    private float lineOffset = 7.6f;
    private float distance = 10f;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        canAttack = 0;
        i = 1;
    }

    // Update is called once per frame
    void Update()
    {
        PhysicsCheck();

        transform.position = Vector2.MoveTowards(transform.position, movePos[i].position, speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, movePos[i].position) < 0.1f)
        {
            if(i == 0)
            {
                i = 1;
            }
            else
            {
                i = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(canAttack == 1)
            {
                print("攻擊成功");
            }
            else
            {
                print("攻擊失敗");
            }
            attackBar.SetActive(false);
        }



    }

    void CloseAttack()
    {
        
    }

    void PhysicsCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-lineOffset, 0f), Vector2.left, distance, layerMask);
        RaycastHit2D rightCheck = Raycast(new Vector2(lineOffset, 0f), Vector2.left, distance, layerMask);

        if (leftCheck || rightCheck)
        {
            canAttack = 1;
        }
        else
        {
            canAttack = 0;
        }
    }


    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);
        Debug.DrawRay(pos + offset, rayDiraction * length);
        return hit;
    }

}
