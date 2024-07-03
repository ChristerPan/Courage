using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string Id = null;
    public string GoToSceneName = null;
    private SceneController sceneController;
    public bool CanEnter;
    public Vector3 Direction;

    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanEnter == false)
            return;
        
        if(collision.tag == "Player")
        {
            //collision.GetComponent<PlayerController>().canMove = false;

            FadeToScene();
        }
    }

    public Vector3 GetFrontPosition()
    {
        //���o���e�y��
        return transform.GetChild(0).position;
    }

    public void FadeToScene()
    {
        //���������W�r
        SceneData.PrevEntranceId = Id;
        //�H�X�����
        sceneController.FadeToScene(GoToSceneName);
    }
}
