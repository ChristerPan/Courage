using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImgFade : MonoBehaviour
{
    public float alpha;
    private float fadeSpeed = 4f;
    
    /// <summary>
    /// 由清晰變透明
    /// </summary>
    /// <param name="image"></param>
    public void ImgFadeIn(Image image)
    {
        StartCoroutine(FadeIn(image));
    }
    public void ImgFadeIn(Image image,PlayerController playerCtrl)
    {
        StartCoroutine(FadeIn(image,playerCtrl));
    }

    /// <summary>
    /// 由透明變清晰
    /// </summary>
    /// <param name="image"></param>
    public void ImgFadeOut(Image image)
    {
        StartCoroutine(FadeOut(image));
    }

    /// <summary>
    /// 淡出，轉場景
    /// </summary>
    /// <param name="image"></param>
    /// <param name="sceneName"></param>
    public void FadeToScene(Image image,string sceneName)
    {
        StartCoroutine(FadeTo(image,sceneName));
    }

    /// <summary>
    /// 由透明到清晰再到透明
    /// </summary>
    /// <param name="image"></param>
    public void ImgFadeOutFadeIn(Image image)
    {
        StartCoroutine(FadeOutFadeIn(image));
    }

    IEnumerator FadeIn(Image image)
    {
        alpha = 1;
        while (alpha > 0)
        {
            //alpha -= fadeSpeed * Time.deltaTime;
            alpha -= Time.deltaTime * 2;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return new WaitForSeconds(0);//等待幾秒以執行下一個function
        }
        
        image.canvas.sortingOrder = -10000;
    }

    IEnumerator FadeIn(Image image, PlayerController playerCtrl)
    {
        alpha = 1;
        while (alpha > 0)
        {
            //alpha -= fadeSpeed * Time.deltaTime;
            alpha -= Time.deltaTime * 2;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return new WaitForSeconds(0);//等待幾秒以執行下一個function
        }
        if (!playerCtrl.plotIsPlaying)
        {
            playerCtrl.canMove = true;
        }
        image.canvas.sortingOrder = -10000;
    }

    IEnumerator FadeOut(Image image)
    {
        image.canvas.sortingOrder = 10000;
        alpha = 0;
        while (alpha < 1)
        {
            //alpha += fadeSpeed * Time.deltaTime;
            alpha += Time.deltaTime * 2;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return new WaitForSeconds(0);//等待幾秒以執行下一個function
        }
    }

    IEnumerator FadeTo(Image image,string sceneName)
    {
        image.canvas.sortingOrder = 10000;
        alpha = 0;
        while (alpha < 1)
        {
            //alpha += fadeSpeed * Time.deltaTime;
            alpha += Time.deltaTime * 2;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return new WaitForSeconds(0);//等待幾秒以執行下一個function
        }
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOutFadeIn(Image image)
    {
        alpha = 0;
        while (alpha <= 1)
        {
            alpha += fadeSpeed * Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return new WaitForSeconds(0);//等待幾秒以執行下一個function
        }
        if (alpha >= 1)
        {
            yield return new WaitForSeconds(2);
        }
        while (alpha >= 0)
        {
            alpha -= fadeSpeed * Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return new WaitForSeconds(0);
        }
    }


}
