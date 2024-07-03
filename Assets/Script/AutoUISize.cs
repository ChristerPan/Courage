using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoUISize : MonoBehaviour
{

    public RectTransform content;
    private RectTransform thisTransform;

    void Start()
    {
        thisTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        thisTransform.sizeDelta = content.sizeDelta + new Vector2(50, 50);
    }
}