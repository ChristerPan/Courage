using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneData
{
    /// <summary>
    /// �e�@�ӤJ�fID
    /// </summary>
    public static string PrevEntranceId;
    /// <summary>
    /// �W�@�ӳ����W��
    /// </summary>
    public static string PrevSceneName;
    /// <summary>
    /// ���a�W�@�Ӧ�m
    /// </summary>
    public static Vector3 PlayerPrevPos;

    public static bool SetPlayerPrePos;
    public static bool SetPlayerCurrentPos;

    public static Vector3 currentPlayerPosition;
    public static string currentSceneName;

    public static void Load(LoadSceneData loadSceneData)
    {
        PrevEntranceId = loadSceneData.PrevEntranceId;
        PrevSceneName = loadSceneData.PrevSceneName;
        //PlayerPrevPos = loadSceneData.PlayerPrevPos;
        SetPlayerPrePos = loadSceneData.SetPlayerPrePos;
        currentPlayerPosition = new Vector3(loadSceneData.currentPlayerPositionX, loadSceneData.currentPlayerPositionY, loadSceneData.currentPlayerPositionZ);
        currentSceneName = loadSceneData.currentSceneName;
    }
}
