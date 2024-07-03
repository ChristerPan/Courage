using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveLoadButton : MonoBehaviour
{
    private SceneController sceneController;
    private SaveGameManager saveGameManager;
    public Button[] saveSlotButtons;
    public Button[] loadSlotButtons;
    public Button[] deleteSlotButtons;
    public GameObject savedSuccessfullyPre;
    public GameObject deletedSuccessfullyPre;
    public Transform wordTransform;

    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        saveGameManager = FindObjectOfType<SaveGameManager>();
        SaveButtonAddListener();
        LoadButtonAddListener();
        DeleteButtonAddListener();
    }

    public void SaveButtonAddListener()
    {
        for (int i = 0; i < saveSlotButtons.Length; i++)
        {
            int slot = i;
            saveSlotButtons[i].onClick.AddListener(() =>
            {
                saveGameManager.SaveAll(slot);
                ShowSavedSuccessfullyPre();
            });
        }
    }

    public void LoadButtonAddListener()
    {
        for (int i = 0; i < loadSlotButtons.Length; i++)
        {
            int slot = i;
            loadSlotButtons[i].onClick.AddListener(() =>
            {
                saveGameManager.LoadAll(slot);

                if (Directory.Exists(Application.dataPath + "/saves" + slot))
                    FadeToSceneInFile();
            });
        }
    }

    public void DeleteButtonAddListener()
    {
        for (int i = 0; i < deleteSlotButtons.Length; i++)
        {
            int slot = i;
            deleteSlotButtons[i].onClick.AddListener(() =>
            {
                ShowDeletedSuccessfullyPre();
                DeleteDirectory(slot);
            });
        }

    }

    public void DeleteDirectory(int slot)
    {
        string path = Application.dataPath + "/saves" + slot;
        Directory.Delete(path, true);
    }

    public void StartNewGame()
    {
        sceneController.FadeToScene("前導動畫");
    }

    public void FadeToSceneInFile()
    {
        SceneData.SetPlayerCurrentPos = true;
        sceneController.FadeToScene(SceneData.currentSceneName);
    }

    public void ShowSavedSuccessfullyPre()
    {
        Instantiate(savedSuccessfullyPre, wordTransform.position, Quaternion.identity, wordTransform);
    }

    public void ShowDeletedSuccessfullyPre()
    {
        Instantiate(deletedSuccessfullyPre, wordTransform.position, Quaternion.identity, wordTransform);
    }
}
