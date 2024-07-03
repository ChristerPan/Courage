using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Sprite> StoneDoorSpts;
    public List<Sprite> GrassSpts;
    public int SpriteIndex;

    public bool StoneDoorOpened, StoneDoorClosing, GrassOpened, GrassClosing;

    public int StoneCaveToalStone;
    public int StoneCavefinishedStone;

    public int GrassTotalStone;
    public int GrassfinishedStone;

    public GameObject StoneDoor;
    public GameObject Grass, Grass2;

    private SpriteRenderer stoneDoorRenderer;
    private SpriteRenderer grassRenderer, grassRenderer2;

    private Coroutine openCor, openCor2, closeCor, closeCor2;
    void Start()
    {
        SpriteIndex = 0;

        StoneCaveToalStone = GameObject.FindGameObjectsWithTag("石洞石頭觸發點").Length;
        GrassTotalStone = GameObject.FindGameObjectsWithTag("草叢石頭觸發點").Length;

        stoneDoorRenderer = StoneDoor.GetComponent<SpriteRenderer>();
        grassRenderer = Grass.GetComponent<SpriteRenderer>();
        grassRenderer2 = Grass2.GetComponent<SpriteRenderer>();
    }

    public void CheckStoneDoorFinishedStone()
    {
        if (StoneCavefinishedStone == StoneCaveToalStone)
        {
            openCor = StartCoroutine(Carousel(StoneDoorSpts, stoneDoorRenderer, 0.05f));

            StoneDoorOpened = true;
            StoneDoor.GetComponent<Door>().CanEnter = true;
            Debug.Log("OpenStoneDoor");
            return;
        }
        if ( StoneCavefinishedStone != StoneCaveToalStone && StoneDoorOpened)
        {
            closeCor = StartCoroutine(ReverseCarousel(StoneDoorSpts, stoneDoorRenderer, 0.05f));

            StoneDoorOpened = false;
            StoneDoor.GetComponent<Door>().CanEnter = false;
            Debug.Log("CloseStoneDoor");
            return;
        }



        
    }
    public void CheckGrassFinishedStone()
    {
        if (GrassfinishedStone == GrassTotalStone)
        {
            openCor = StartCoroutine(Carousel(GrassSpts, grassRenderer, 0.1f));
            openCor2 = StartCoroutine(Carousel(GrassSpts, grassRenderer2, 0.1f));

            Grass.GetComponent<BoxCollider2D>().enabled = false;
            Grass2.GetComponent<BoxCollider2D>().enabled = false;

            GrassOpened = true;
            GrassClosing = false;
            Debug.Log("OpenGrass");
            return;
        }
        if (GrassfinishedStone != GrassTotalStone && GrassOpened)
        {
            closeCor = StartCoroutine(ReverseCarousel(GrassSpts, grassRenderer, 0.1f));
            closeCor2 = StartCoroutine(ReverseCarousel(GrassSpts, grassRenderer2, 0.1f));

            Grass.GetComponent<BoxCollider2D>().enabled = true;
            Grass2.GetComponent<BoxCollider2D>().enabled = true;

            GrassOpened = false;
            GrassClosing = true;
            Debug.Log("CloseGrass");
            return;
        }
    }

    IEnumerator Carousel(List<Sprite> spriteList, SpriteRenderer spriteRenderer, float speed)
    {

        if (closeCor != null)
        {
            StopCoroutine(closeCor);

            if(closeCor2 != null)
            {
                StopCoroutine(closeCor2);
            }
            
        }
        
        for (int i = SpriteIndex; i < spriteList.Count; i++)
        {
            yield return new WaitForSeconds(speed);
            spriteRenderer.sprite = spriteList[i];
            SpriteIndex = i;
        }

    }
    IEnumerator ReverseCarousel(List<Sprite> spriteList, SpriteRenderer spriteRenderer, float speed)
    {
        if (openCor != null)
        {
            StopCoroutine(openCor);

            if(openCor2 != null)
            {
                StopCoroutine(openCor2);
            }
            
        }

        for (int i = SpriteIndex; i >= 0; i--)
        {
            yield return new WaitForSeconds(speed);
            spriteRenderer.sprite = spriteList[i];
            SpriteIndex = i;
        }
    }

    
}
