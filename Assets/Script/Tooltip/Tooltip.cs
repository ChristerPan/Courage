using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;

    public TextMeshProUGUI contentField;

    public TextMeshProUGUI howmuchField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;

    public VerticalLayoutGroup verticalLayoutGroup;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header) && string.IsNullOrEmpty(content))
        {
            verticalLayoutGroup.enabled = false;
        }
        else
        {
            verticalLayoutGroup.enabled = true;
        }

        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;
        howmuchField.gameObject.SetActive(false);

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    public void SetText(string content, string header = "", string howmuch = "")
    {
        if (string.IsNullOrEmpty(header) && string.IsNullOrEmpty(content))
        {
            verticalLayoutGroup.enabled = false;
        }
        else
        {
            verticalLayoutGroup.enabled = true;
        }
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;
        howmuchField.text = howmuch;
        howmuchField.gameObject.SetActive(true);

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }
    void LateUpdate()
    {
        if (Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
        }

        Vector2 position = Input.mousePosition;
        
        float posX = position.x;/* = position.x / Screen.width;*/
        float posY = position.y;/* = position.y / Screen.height;*/
        float pivotX = 0;
        float pivotY = 0;
        if (position.x >= Screen.width/2 && position.y >= Screen.height / 2)
        {
            pivotX = 1;
            pivotY = 1;
        }
        if (position.x >= Screen.width / 2 && position.y <= Screen.height / 2)
        {
            pivotX = 1;
            pivotY = 0;
        }
        if (position.x <= Screen.width / 2 && position.y >= Screen.height / 2)
        {
            posY = position.y - 50;
            pivotX = 0;
            pivotY = 1;
        }
        if (position.x <= Screen.width / 2 && position.y <= Screen.height / 2)
        {
            pivotX = 0;
            pivotY = 0;
        }


        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = new Vector2(posX, posY);
    }
}
