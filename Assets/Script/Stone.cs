using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public Color finishColor;
    Color originColor;
    public GameManager gameManager;

    private void Start()
    {
        originColor = GetComponent<SpriteRenderer>().color;
        gameManager = FindObjectOfType<GameManager>();
    }

    //public bool CanMobeToDir(Vector3 dir)
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position+(Vector3)dir*0.5f, dir,0.5f);
    //    if (!hit)
    //    {
    //        transform.Translate(dir);
    //        return true;
    //    }
           
    //    return false;
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("石洞石頭觸發點"))
        {
            gameManager.StoneCavefinishedStone++;
            GetComponent<SpriteRenderer>().color=finishColor;
            gameManager.CheckStoneDoorFinishedStone();
        }
        if (collision.CompareTag("草叢石頭觸發點"))
        {
            gameManager.GrassfinishedStone++;
            GetComponent<SpriteRenderer>().color = finishColor;
            gameManager.CheckGrassFinishedStone();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("石洞石頭觸發點"))
        {
            gameManager.StoneCavefinishedStone--;
            gameManager.CheckStoneDoorFinishedStone();
            GetComponent<SpriteRenderer>().color = originColor;
        }
        if (collision.CompareTag("草叢石頭觸發點"))
        {
            gameManager.GrassfinishedStone--;
            gameManager.CheckGrassFinishedStone();
            GetComponent<SpriteRenderer>().color = originColor;
        }
    }
}
