using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    //�˳����W�Ҧ������M��
    public Door[] DoorList;

    public GameObject player;

    public Image BlackImage;

    public ImgFade imgFade;
    //�ݭn�Ǫ�������
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

        if(SceneManager.GetActiveScene().name == "�˪L" && PlotProgress.drawSwordComplete)
        {
            needMonsters = true;
        }

        if(SceneManager.GetActiveScene().name == "�}�l�e��")
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
            Debug.Log("RamdomEncounterMonster�G" + IsInvoking("RamdomEncounterMonster"));
        }


        SceneData.SetPlayerPrePos = false;
        SceneData.SetPlayerCurrentPos = false;

        imgFade = GetComponent<ImgFade>();
        //�H�J
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

        //���o�i�Ӫ�����ID
        eid = SceneData.PrevEntranceId;

        //�M�䦹ID����
        Door door = GetDoorById(eid);


        if (door)
        {
            //�V���n���f����m
            Vector3 pos = door.GetFrontPosition();
            //�⪱�a�����f�y��
            player.transform.position = pos;
            //���o�H���n�ª���V
            playerCtrl.prevMoveDirection = door.Direction;
        }
        else
        {

            Debug.Log("�b�o�����䤣���ID�G" + eid);
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
            Debug.Log("RamdomEncounterMonster�G" + IsInvoking("RamdomEncounterMonster"));
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
            FadeToScene("��l����");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("ctr+x");
            FadeToScene("�˪L");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("ctr+c");
            FadeToScene("�۬}");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("ctr+v");
            FadeToScene("�԰�");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("ctr+b");
            FadeToScene("�Ĥ@���԰�����");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
        {
            FadeToScene("�j�a��");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M))
        {
            FadeToScene("�D�@�ɥ���");
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A))
        {
            FadeToScene("����");
        }
    }
}
