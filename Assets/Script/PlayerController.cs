using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text tipText;
    public string battleSceneName;
    public bool canMove, pushStone, plotIsPlaying;
    public bool removeShake;
    public float walkSpeed;
    public SceneController sceneController;
    public Animator animator;
    public Vector3 prevMoveDirection;

    public CharacterData_SO characterData;
    public GameObject myBag, myEquip, myProps;
    bool isOpen, isEquipOpen;



    void Start()
    {
        animator = GetComponent<Animator>();
        sceneController = FindObjectOfType<SceneController>();
        SceneData.currentSceneName = gameObject.scene.name;
        SceneData.currentPlayerPosition = transform.position;
        if (PlotProgress.drawSwordComplete)
        {
            tipText.text = "探索村莊";
            animator.SetTrigger("CompleteDrawSword");
        }
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
        {
            characterData.currentMoney += 1000;
        }

        walkSpeed = characterData.currentSpeed;

        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenMyBag();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenEquip();
        }


        if (Time.timeScale == 1)
        {
            animator.SetFloat("MoveX", prevMoveDirection.x);
            animator.SetFloat("MoveY", prevMoveDirection.y);

            if (!canMove)
            {
                animator.SetBool("walk", false);
                return;
            }


            Vector2 from = transform.position;
            from.y += 0.9f;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                from.x -= 0.5f;
                moveDirection.x = -1f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                from.x += 0.5f;
                moveDirection.x = 1f;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                from.y += 0.75f;
                moveDirection.y = 1f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                from.y -= 0.75f;
                moveDirection.y = -1f;
            }



            if (moveDirection.sqrMagnitude > 0)
            {
                if (pushStone)
                {
                    if (PlotProgress.drawSwordComplete)
                    {
                        animator.SetBool("PushStoneWithSword", true);
                        animator.SetBool("walkWithSword", false);
                    }
                    else
                    {
                        animator.SetBool("PushStone", true);
                        animator.SetBool("walk", false);
                    }
                }
                else
                {
                    if (PlotProgress.drawSwordComplete)
                    {
                        animator.SetBool("walkWithSword", true);
                        animator.SetBool("PushStoneWithSword", false);
                    }
                    else
                    {
                        animator.SetBool("walk", true);
                        animator.SetBool("PushStone", false);
                    }
                    
                }

                prevMoveDirection = moveDirection;
                SceneData.currentPlayerPosition = transform.position;
            }
            else
            {
                if (PlotProgress.drawSwordComplete)
                {
                    animator.SetBool("walkWithSword", false);
                    animator.SetBool("PushStoneWithSword", false);
                }
                else
                {
                    animator.SetBool("walk", false);
                    animator.SetBool("PushStone", false);
                }
            }



            Vector2 v2Dis = moveDirection * walkSpeed * Time.deltaTime;

            if (removeShake)
            {
                Vector2 to = from + v2Dis;

                RaycastHit2D hit = Physics2D.Linecast(from, to, LayerMask.GetMask("Wall"));

                Debug.DrawLine(from, to, Color.red);

                if (hit)
                {
                    v2Dis = hit.point - from;
                }
            }


            transform.position += new Vector3(v2Dis.x, v2Dis.y, 0);
        }



    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "石頭")
        {
            pushStone = true;

        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "石頭")
        {
            pushStone = false;
        }
    }

    

    public void OpenMyBag()
    {
        isOpen = myBag.activeSelf;
        myProps.SetActive(myBag.activeSelf);

        isOpen = !isOpen;
        myBag.SetActive(isOpen);
        myProps.SetActive(isOpen);
        //InventoryManger.RefreshItem();
        //InventoryManger.RefreshPropsItem();
    }
    public void OpenEquip()
    {
        isEquipOpen = myEquip.activeSelf;

        isEquipOpen = !isEquipOpen;
        myEquip.SetActive(isEquipOpen);
    }





    public void FadeToScene()
    {
        sceneController.FadeToScene(battleSceneName);
    }
}
