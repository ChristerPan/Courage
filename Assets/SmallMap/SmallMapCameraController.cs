using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMapCameraController : MonoBehaviour
{

    public float _speed;

    public GameObject Follow;

    private Vector3 Pos;

    public float XLeft, XRight, YUp, YDown;


    void LateUpdate()
    {
        //if (gameObject.transform.position.x <= XLeft || gameObject.transform.position.x >= XRight || gameObject.transform.position.y <= YDown || gameObject.transform.position.y >= YUp)
        
        

        Pos = Follow.transform.position - gameObject.transform.position;
        Pos.z = 0;    
        gameObject.transform.position += Pos / 20;

        Vector3 pos = gameObject.transform.position;
        if (pos.x < XLeft)
        {
            pos.x = XLeft;
        }
        if (pos.x > XRight)
        {
            pos.x = XRight;
        }
        if (pos.y < YDown)
        {
            pos.y = YDown;
        }
        if (pos.y > YUp)
        {
            pos.y = YUp;
        }
        gameObject.transform.position = pos;
    }

    
    void Start()
    {
        _speed = 0.01f;

        if(Follow == null)
        {
            Follow = GameObject.Find("Player");
        }
    }

    
    void Update()
    {

    }
}
