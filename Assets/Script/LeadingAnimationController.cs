using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LeadingAnimationController : MonoBehaviour
{
    public List<VideoClip> videos;
    public SceneController sceneController;
    private VideoPlayer videoPlayer;
    public int index;
    public bool canClick;

    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        videoPlayer = GetComponent<VideoPlayer>();
        index = 0;
        StartCoroutine(FadeIn());
    }

    
    void Update()
    {
        if (Input.anyKeyDown && canClick)
        {
            if (index == videos.Count-1)
            {
                sceneController.FadeToScene("古文館");
            }
            else 
            {
                StartCoroutine(NextVideo());
            }
            
        }
    }

    IEnumerator FadeIn()
    {
        canClick = false;
        videoPlayer.targetCameraAlpha = 0;
        
        while (videoPlayer.targetCameraAlpha < 1)
        {
            videoPlayer.targetCameraAlpha += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
        canClick = true;
    }
    IEnumerator NextVideo()
    {
        index++;
        canClick = false;
        videoPlayer.targetCameraAlpha = 1;

        while (videoPlayer.targetCameraAlpha > 0.1f)
        {
            //alpha -= fadeSpeed * Time.deltaTime;
            videoPlayer.targetCameraAlpha -= Time.deltaTime;
            yield return new WaitForSeconds(0);//等待幾秒以執行下一個function
        }

        videoPlayer.clip = videos[index];

        while (videoPlayer.targetCameraAlpha < 1)
        {
            videoPlayer.targetCameraAlpha += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }

        canClick = true;
    }
}
