using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DrawSword : MonoBehaviour
{
    public List<Sprite> DrawSwordSprites;
    public GameObject SpaceWord;
    public Image WhiteImg;

    public GameObject ChargeBar;//�����

    public Transform DrawSwordPosition;

    public static float chargeGoal = 10;//����ؼ�
    
    public bool CanDrawSword;//�i�H�޼C

    public float CountPresses;//�p����F�X��

    public bool DrawingSword;//���b�޼C

    public Canvas[] Canvases;

    private GameObject collisionObj;//�I��������
    private Animator collisionAni;
    private SpriteRenderer collosionSprite;
    private SpriteRenderer SwordSprite;

    private VideoPlayer videoPlayer;

    void Awake()
    {
        if (PlotProgress.drawSwordComplete)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        SwordSprite = gameObject.GetComponent<SpriteRenderer>();
        videoPlayer = GetComponent<VideoPlayer>();
        Canvases = FindObjectsOfType<Canvas>();
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    
    void Update()
    {
        if (CanDrawSword)
        {
            
            if (Input.GetKeyDown(KeyCode.Space) && !DrawingSword)
            {
                DrawingSword = true;
                collisionObj.GetComponent<PlayerController>().canMove = false;
                collisionObj.transform.position = DrawSwordPosition.position;
                collosionSprite = collisionObj.GetComponent<SpriteRenderer>();
                collisionAni = collisionObj.GetComponent<Animator>();
                collisionAni.enabled = false;
                ChargeBar.transform.parent.gameObject.SetActive(true);
                SwordSprite.color = new Color(SwordSprite.color.r, SwordSprite.color.g, SwordSprite.color.b, 0);
            }

            if (DrawingSword)
            {
                CountPresses -= Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CountPresses += 1;
                }

                if (CountPresses >= chargeGoal)
                {
                    //CountPresses = chargeGoal;
                    CountPresses = 0;
                    CanDrawSword = false; 
                    ChargeBar.transform.parent.gameObject.SetActive(false);
                    collisionObj.GetComponent<PlayerController>().tipText.text = "��������";
                    if (videoPlayer.clip != null)
                    {
                        PlotProgress.drawSwordComplete = true;
                        WhiteImg.gameObject.SetActive(true);
                        StartCoroutine(EnlargeUIImg(WhiteImg, 50));
                    }
                    else
                    {
                        collisionAni.enabled = true;
                        collisionObj.GetComponent<PlayerController>().canMove = true;
                        Destroy(gameObject);
                    }
                    
                }
                
                if (CountPresses <= 0)
                {
                    CountPresses = 0;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CountPresses = 0;
                    collisionObj.GetComponent<PlayerController>().canMove = true;
                    DrawingSword = false;
                    collisionAni.enabled = true;
                    ChargeBar.transform.parent.gameObject.SetActive(false);
                    SwordSprite.color = new Color(SwordSprite.color.r, SwordSprite.color.g, SwordSprite.color.b, 1);
                }

                ChargeBar.GetComponent<Image>().fillAmount = CountPresses / chargeGoal;
                var i = System.Math.Round((double)DrawSwordSprites.Count / 100, 2);
                i *= (CountPresses / chargeGoal) * 100;
                float v = (float)i;
                i = Mathf.Round(v);
                int index = (int)i;
                
                if(index >= DrawSwordSprites.Count)
                {
                    index = DrawSwordSprites.Count - 1;
                }
                collosionSprite.sprite = DrawSwordSprites[index];

            }


        }
        
        if (videoPlayer.time >= 0.35f)
        {
            foreach (Canvas canvas in Canvases)
            {
                canvas.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SpaceWord.SetActive(true);
            CanDrawSword = true;
            collisionObj = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SpaceWord.SetActive(false);
        CanDrawSword = false;
    }

    IEnumerator EnlargeUIImg(Image img, int multiple)
    {
        Debug.Log("EnlargeUIImg");
        for (int i = 0; i <= multiple; i+=5)
        {
            img.transform.localScale = new Vector2(i, i);
            yield return new WaitForSeconds(0);
        }
        if(FindObjectOfType<AudioSource>()!=null) FindObjectOfType<AudioSource>().enabled = false;
        videoPlayer.Play();
        
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        videoPlayer.Stop();
        if (FindObjectOfType<AudioSource>() != null) FindObjectOfType<AudioSource>().enabled = true;
        Debug.Log("Video finished playing!");
        foreach (Canvas canvas in Canvases)
        {
            canvas.gameObject.SetActive(true);
        }
        collisionAni.enabled = true;
        collisionAni.SetTrigger("CompleteDrawSword");
        WhiteImg.gameObject.SetActive(false);
        collisionObj.GetComponent<PlayerController>().canMove = true;
        Destroy(gameObject);
    }
}
