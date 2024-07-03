using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFadeOutAndDestory : MonoBehaviour
{
    [SerializeField] private float alpha;
    float fadeSpeed = 8f;
    SpriteRenderer sprite;

    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    public void FadeOutAndDestory()
    {
        StartCoroutine(FadeOutDestory());
    }
    IEnumerator FadeOutDestory()
    {
        alpha = 1;
        while (sprite.color.a > 0)
        {
            alpha -= fadeSpeed*Time.deltaTime;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            yield return new WaitForSeconds(0);//等待幾秒以執行下一個function
        }
        if (sprite.color.a <= 0)
        {
            Destroy(gameObject);
        }
        
    }
}
