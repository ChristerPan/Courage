using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    //裝場景上所有門的清單
    public Door[] DoorList;

    public GameObject player;

    public Image BlackImage;

    public ImgFade imgFade;
    //需要怪物的場景
    public bool needMonsters;

    public string battleSceneName;
    [SerializeField] 
    string eid;

    public PlayerController playerCtrl;


    private void Awake()
    {
        if (player != null)
        {
            playerCtrl = player.GetComponent<PlayerController>();
            playerCtrl.canMove = false;
        }

        if(SceneManager.GetActiveScene().name == "森林" && PlotProgress.drawSwordComplete)
        {
            needMonsters = true;
        }

        if(SceneManager.GetActiveScene().name == "開始畫面")
        {
            PlotProgress.drawSwordComplete = false;
            PlotProgress.dontShowPlotOfLibrary = false;
            PlotProgress.dontShowPlotOfVillage = false;
        }
    }
    void Start()
    {
        if (SceneData.SetPlayerCurrentPos)
        {
            player.transform.position = SceneData.currentPlayerPosition;
        }
        else if (SceneData.SetPlayerPrePos)
        {
            player.transform.position = SceneData.PlayerPrevPos;
        }
        else
        {
            PutPlayerInDoor();
        }


        if (needMonsters)
        {
            InvokeRepeating("RamdomEncounterMonster", 7f, 3f);
            Debug.Log("RamdomEncounterMonster：" + IsInvoking("RamdomEncounterMonster"));
        }


        SceneData.SetPlayerPrePos = false;
        SceneData.SetPlayerCurrentPos = false;

        imgFade = GetComponent<ImgFade>();
        //淡入
        if (playerCtrl != null)
        {
            imgFade.ImgFadeIn(BlackImage, playerCtrl);
        }
        else
        {
            imgFade.ImgFadeIn(BlackImage);
        }
        

        
        
    }


    public void PutPlayerInDoor()
    {
        DoorList = FindObjectsOfType<Door>();

        //取得進來的門的ID
        eid = SceneData.PrevEntranceId;

        //尋找此ID的門
        Door door = GetDoorById(eid);


        if (door)
        {
            //向門要門口的位置
            Vector3 pos = door.GetFrontPosition();
            //把玩家放到門口座標
            player.transform.position = pos;
            //取得人物要朝的方向
            playerCtrl.prevMoveDirection = door.Direction;
        }
        else
        {

            Debug.Log("在這場景找不到門ID：" + eid);
        }
    }
    public Door GetDoorById(string doorId)
    {
        foreach(Door door in DoorList)
        {
            if(door.Id== doorId)
            {
                return door;
            }
        }
        return null;
    }
    
    public void RamdomEncounterMonster()
    {
        int i = Random.Range(0, 2);
        Debug.Log(i);
        if(i == 1)
        {
            SceneData.PrevSceneName = SceneManager.GetActiveScene().name;
            SceneData.PlayerPrevPos = player.transform.position;
            playerCtrl.battleSceneName = battleSceneName;
            playerCtrl.canMove = false;
            player.GetComponent<Animator>().SetTrigger("Frightened");
            CancelInvoke("RamdomEncounterMonster");
            Debug.Log("RamdomEncounterMonster：" + IsInvoking("RamdomEncounterMonster"));
            //FadeToScene(battleSceneName);
        }
    }

    public void EscpaeBattle()
    {
        SceneData.SetPlayerPrePos = true;
        FadeToScene(SceneData.PrevSceneName);
    }

    public void FadeToScene(string sceneName)
    {
        //FindObjectOfType<SaveGameManager>().SaveAll();
        if (player != null)
        {
            playerCtrl.canMove = false;
        }
        
        imgFade.FadeToScene(BlackImage, sceneName);
    }


    void Update()
    {
        //if (BlackImage.color.a >= 0.05f && BlackImage.color.a <= 0.1f)
        //{
        //    playerCtrl.canMove = true;
        //    Debug.Log(BlackImage.color.a);
        //}
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("ctr+w");
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject obj in objs)
            {
                obj.tag = "DeadUnit";
            }
            FindObjectOfType<BattleSystem>().ToBattle();
            
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("ctr+z");
            FadeToScene("初始村莊");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("ctr+x");
            FadeToScene("森林");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("ctr+c");
            FadeToScene("石洞");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("ctr+v");
            FadeToScene("戰鬥");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("ctr+b");
            FadeToScene("第一章戰鬥場景");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
        {
            FadeToScene("大地圖");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M))
        {
            FadeToScene("主世界平原");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
        {
            FadeToScene("漁村");
        }
    }
}
