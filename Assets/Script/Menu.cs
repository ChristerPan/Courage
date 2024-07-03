using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class Menu : MonoBehaviour
{
    public GameObject settingMenu;
    public GameObject saveBackground;
    public AudioMixer audioMixer;
    public bool touchToStart;


    

    void Update()
    {
        if (Input.anyKeyDown && touchToStart)
        {
            saveBackground.SetActive(true);
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void ClickSettingBtn()
    {
        settingMenu.SetActive(!settingMenu.activeSelf);
    }

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("MainVolume", value);
    }


    public void SaveBtn()
    {
        saveBackground.SetActive(true);
    }

    public void ClickStartSceneBtn()
    {
        FindObjectOfType<SceneController>().FadeToScene("開始畫面");
    }
}
