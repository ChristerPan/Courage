using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTurnDirection : MonoBehaviour
{
    public Sprite up, down, left, right, leftUp, leftDown, rightUp, rightDown;

    public void TurnDirection(Vector3 playerPos)
    {
        Vector2 direction = playerPos - transform.position;
        direction.Normalize();

        
        if (direction.y > 0.5f)
        {
            GetComponent<SpriteRenderer>().sprite = up;
        }
        else if (direction.y < -0.5f)
        {
            GetComponent<SpriteRenderer>().sprite = down;
        }

        if (direction.x > 0.5f)
        {
            GetComponent<SpriteRenderer>().sprite = right;
        }
        else if (direction.x < -0.5f)
        {
            GetComponent<SpriteRenderer>().sprite = left;
        }

        if (direction.x > 0.5f && direction.y > 0.5f)
        {
            GetComponent<SpriteRenderer>().sprite = rightUp;
        }
        else if (direction.x > 0.5f && direction.y < -0.5f)
        {
            GetComponent<SpriteRenderer>().sprite = rightDown;
        }
        else if (direction.x < -0.5f && direction.y > 0.5f)
        {
            GetComponent<SpriteRenderer>().sprite = leftUp;
        }
        else if (direction.x < -0.5f && direction.y < -0.5f)
        {
            GetComponent<SpriteRenderer>().sprite = leftDown;
        }
    }
}
