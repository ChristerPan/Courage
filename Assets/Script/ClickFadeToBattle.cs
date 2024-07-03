using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickFadeToBattle : MonoBehaviour
{
    public string battleSceneName;
    private void OnMouseDown()
    {
        SceneData.PlayerPrevPos = FindObjectOfType<PlayerController>().transform.position;
        SceneData.PrevSceneName = gameObject.scene.name;
        FindObjectOfType<SceneController>().FadeToScene(battleSceneName);
    }
}
