using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoadSceneData
{
    public string PrevEntranceId;
    public string PrevSceneName;
    //public Vector3 PlayerPrevPos;
    public bool SetPlayerPrePos;
    public float currentPlayerPositionX;
    public float currentPlayerPositionY;
    public float currentPlayerPositionZ;
    public string currentSceneName;

    public LoadSceneData()
    {
        PrevEntranceId = SceneData.PrevEntranceId;
        PrevSceneName = SceneData.PrevSceneName;
        //PlayerPrevPos = SceneData.PlayerPrevPos;
        SetPlayerPrePos = SceneData.SetPlayerPrePos;
        currentPlayerPositionX = SceneData.currentPlayerPosition.x;
        currentPlayerPositionY = SceneData.currentPlayerPosition.y;
        currentPlayerPositionZ = SceneData.currentPlayerPosition.z;
        currentSceneName = SceneData.currentSceneName;
    }

}
