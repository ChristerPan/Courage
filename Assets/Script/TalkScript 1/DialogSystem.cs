using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogSystem : MonoBehaviour
{
    public GameObject panel;

    public GameObject plotDialogBox;
    private PlayerController playerCtrl;
    [Header("村莊劇情")]
    public bool showPlotOfVillage;

    [Header("古文館劇情")]
    public bool showPlotOfLibrary;
    public Sprite playerReadingSprite;
    public Sprite playerLeftUpSprite;

    [Header("UI")]
    public Text textLabel;
    public Image faceImage;

    [Header("文本文件")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;
    public bool showAllText;//文字直接全部顯示
    bool textFinished;//是否完成打字
    bool cancelTyping;//取消打字


    [Header("頭像")]
    public Sprite face01, face02;

    List<string> textList = new List<string>();
    
    void Awake()
    {
        GetTextFormFile(textFile);

        playerCtrl = FindObjectOfType<PlayerController>();

        if (showPlotOfVillage || showPlotOfLibrary) playerCtrl.plotIsPlaying = true;
        
        if(PlotProgress.dontShowPlotOfLibrary && PlotProgress.dontShowPlotOfVillage) 
            playerCtrl.plotIsPlaying = false;

        if (PlotProgress.dontShowPlotOfLibrary)
        {
            showPlotOfLibrary = false;
        }
        if (PlotProgress.dontShowPlotOfVillage)
        {
            showPlotOfVillage = false;
        }
        
    }

    private void OnEnable()
    {
        //textLabel.text = textList[index];
        //index++;
        if (!showPlotOfLibrary && !showPlotOfVillage)
        {
            StartCoroutine(SetTextUI());
        }
        
        textFinished = true;
    }

    void Start()
    {
        if (showPlotOfLibrary)
        {
            playerCtrl.GetComponent<Animator>().enabled = false;
            playerCtrl.GetComponent<SpriteRenderer>().sprite = playerReadingSprite;
            playerCtrl.prevMoveDirection = new Vector3(-1, 1, 0);
            StartCoroutine(ShowDialogBox());

            
        }
        if (showPlotOfVillage)
        {
            StartCoroutine(ShowDialogBox());
        }

    }

    void OnDisable()
    {
        index = 0;
        FindObjectOfType<PlayerController>().canMove = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.R) && index == textList.Count || Input.GetMouseButtonDown(1) && index == textList.Count)
        {
            gameObject.SetActive(false);
            FindObjectOfType<PlayerController>().canMove = true;
            index = 0;

            if (showPlotOfLibrary)
            {
                PlotProgress.dontShowPlotOfLibrary = true;
                playerCtrl.GetComponent<Animator>().enabled = true;
                playerCtrl.plotIsPlaying = false;
                playerCtrl.canMove = true;
            }
            if (showPlotOfVillage)
            {
                PlotProgress.dontShowPlotOfVillage = true;
                playerCtrl.plotIsPlaying = false;
                playerCtrl.canMove = true;
            }
            if(panel != null)
            {
                panel.SetActive(true);
            }
            return;
        }
        
        //if (Input.GetKeyDown(KeyCode.R)&& TextFinished)
        //{
        //    //textLabel.text = textList[index];
        //    //index++;
        //    StartCoroutine(SetTextUI());
        //}

        if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1))
        {
            if (plotDialogBox != null && plotDialogBox.activeSelf == false || gameObject.activeSelf == false)
                return;
            if (textFinished && !cancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished && !cancelTyping)
            {
                cancelTyping = true;
            }
        }
    }

    IEnumerator ShowDialogBox()
    {
        yield return new WaitForSeconds(1.3f);
        plotDialogBox.SetActive(true);
        StartCoroutine(SetTextUI());
    }

    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        if (showAllText)
        {
            textList.Add(file.text);
        }
        else
        {
            var lineData = file.text.Split('\n');

            foreach (var line in lineData)
            {
                textList.Add(line);
            }
        }
        
    }

    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";


        int faceID = -1;

        if (textList[index].Trim() == "A") faceID = 0;
        else if (textList[index].Trim() == "B") faceID = 1;


        switch (faceID)
        {
            case 0:
                faceImage.sprite = face01;
                index++;
                break;
            case 1:
                faceImage.sprite = face02;
                index++;
                break;
        }

        if (textList[index].Trim() == "哈哈哈~")
        {
            playerCtrl.GetComponent<SpriteRenderer>().sprite = playerLeftUpSprite;
        }

        int letter = 0;
        while (!cancelTyping && letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;

    }

}
