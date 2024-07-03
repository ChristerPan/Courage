using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum AttackType { AttackCircle, ChargeBar }
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public enum Arrow { UP, DOWN, LEFT, RIGHT, SPACE }
public class BattleSystem : MonoBehaviour
{
    [Header("UI")]
    public Image readyWord;                //準備的字樣
    public Image mpImg;
    public Text battleUnitsText;
    public Button atkBtn;//攻擊按鈕
    public Button skillBtn1;//技能按鈕1

    [Header("能量條圖片")]
    public Sprite[] mpSprites;

    [Header("遊戲物件")]
    public GameObject marked; //標示
    public GameObject lockImg;                      //鎖定目標的圖
    public GameObject space;                    //提示空白鍵
    public GameObject yourTurnWord;             //你的回合字樣
    public GameObject enemyTurnWord;            //敵人的回合字樣
    public GameObject chooseMonsterWordPre;       //選擇怪物字樣
    public GameObject timeBar;//時間條
    public GameObject playerPrefab;//玩家Prefab
    public GameObject attackCircle;//攻擊圈
    public GameObject floatPoint;//傷害數字
    public GameObject bloodEffect;//血的特效
    public GameObject[] monsters;//要生成的怪物
    public GameObject[] arrowPrefabs;//方向鍵
    public GameObject chargeBar;//集氣條

    public static List<GameObject> battleUnits;//所有參戰對象的列表
    [Header("Units")]
    public GameObject[] playerUnits;           //所有參戰玩家的列表
    public GameObject[] enemyUnits;            //所有參戰敵人的列表
    public GameObject[] remainingPlayerUnits;  //剩餘參戰玩家的列表
    public GameObject[] remainingEnemyUnits;   //剩餘參戰敵人的列表

    [Header("Transform")]
    public Transform wordStation;               //產生回合字樣的位置
    public Transform attackTarget; //有效區
    public Transform attackClock;  //攻擊指針
    public Transform playerBattleStation;//生成的位置
    public Transform enemyBattleStation;//生成的位置
    public Transform[] arrowPositions;//方向鍵生成位置




    public static GameObject currentActUnit;          //當前行動的單位
    public static GameObject currentActUnitTarget;    //當前行動的單位的目標
    public static bool defenseSucceeded;        //防禦成功
    public static int skillIndex;             //技能索引 
    private static LTDescr delay;


    private GameObject chooseMonsterWord;//被實例化選擇怪物字樣
    private GameObject playerGO;//生成的玩家
    private GameObject enemyGO;//生成的怪物

    private bool isWaitForPlayerToChooseSkill = false;            //玩家選擇技能UI的開關
    public bool isWaitForPlayerToChooseTarget = false;  //是否等待玩家選擇目標，控制射線的開關
    public bool waitPlayerChooseButton;    //等待玩家選擇按鈕
    private bool attacking, defending, usingProps, charging;//指針正在轉,跳出箭頭,使用道具中,集氣中
    private bool dontResetDefenseTime;//不重置防禦時間


    private float chargeGoal = 10;//集氣目標
    private float countPresses;//計算集氣按了幾次
    private float currentTimer; //時間條倒數
    private float timeBarTimer; //時間條時間
    private float attackDamageMultiplier;      //攻擊傷害係數
    private float attackData;                  //傷害值
    private float originalHealth; //原本血量
    //每 frame 旋轉速度。
    private float rotationspeed = 240;
    private float angle;//指針旋轉角度

    public int mp;//能量
    public int indexOfMonster; //選擇怪物的索引值
    private int amountOfArrows;//箭頭的總數
    private int arrowQuantity;//箭頭數量
    private int QuizCounter = 0;
    private int inputArrow;


    private BattleState state;
    private AttackType attackType;
    private List<Arrow> QuizArray = new List<Arrow>();

    private ImgFade imgFade;
    private RefreshPropsItem refreshProps;
    private Animator playerAni;  //玩家動畫控制器
    private SkillScriptableObject skill;
    private CharacterStats playerState;          //玩家基本狀態
    private Camera cam;
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        imgFade = FindObjectOfType<ImgFade>();
        cam = Camera.main;
        refreshProps = FindObjectOfType<RefreshPropsItem>();
    }


    void Update()
    {
        inputArrow = -1;

        if (defending)
        {
            Defense();
        }

        if (isWaitForPlayerToChooseTarget)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ChooseTarget();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                for (int i = 0; i < remainingEnemyUnits.Length; i++)
                {
                    if (remainingEnemyUnits[i].transform.position.y > remainingEnemyUnits[indexOfMonster].transform.position.y)
                    {
                        remainingEnemyUnits[indexOfMonster].GetComponent<MouseOverMonster>().HideMark();
                        remainingEnemyUnits[i].GetComponent<MouseOverMonster>().ShowMark();
                        indexOfMonster = i;
                        break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                for (int i = 0; i < remainingEnemyUnits.Length; i++)
                {
                    if (remainingEnemyUnits[i].transform.position.y < remainingEnemyUnits[indexOfMonster].transform.position.y)
                    {
                        remainingEnemyUnits[indexOfMonster].GetComponent<MouseOverMonster>().HideMark();
                        remainingEnemyUnits[i].GetComponent<MouseOverMonster>().ShowMark();
                        indexOfMonster = i;
                        break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentActUnitTarget = remainingEnemyUnits[indexOfMonster];
                currentActUnitTarget.GetComponent<MouseOverMonster>().HideMark();
                lockImg.SetActive(true);
                lockImg.transform.position = new Vector2(currentActUnitTarget.transform.position.x, currentActUnitTarget.transform.position.y + currentActUnitTarget.GetComponent<BoxCollider2D>().size.y * currentActUnitTarget.transform.localScale.y / 2);
                lockImg.gameObject.GetComponent<Animator>().Play("Lock");
                //Debug.Log("攻擊目標為：" + currentActUnitTarget.name);
                isWaitForPlayerToChooseTarget = false;

                if (chooseMonsterWord != null) Destroy(chooseMonsterWord);

                //ChooseSkill();
                ReadyAttack(attackType);
            }
        }

        if (attacking)
        {
            PointerRotation();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bool attack = DetectAttacks();
                if (attack)
                {
                    LaunchAttack();
                }
                else
                {
                    ToBattle();
                }
                attacking = false;
            }
        }

        if (charging)
        {
            playerAni.SetBool("ChargeUp", true);
            countPresses -= Time.deltaTime;
            currentTimer -= Time.deltaTime;


            if (Input.GetKeyDown(KeyCode.Space))
            {
                countPresses += 1;
            }

            if (countPresses <= 0)
            {
                countPresses = 0;
            }

            if (countPresses >= chargeGoal && currentTimer > 0)
            {
                timeBar.transform.parent.gameObject.SetActive(false);
                chargeBar.transform.parent.gameObject.SetActive(false);
                space.SetActive(false);
                charging = false;
                playerAni.SetBool("ChargeUp", false);
                countPresses = 0;
                LaunchAttack();
            }

            if (currentTimer <= 0)
            {
                timeBar.transform.parent.gameObject.SetActive(false);
                chargeBar.transform.parent.gameObject.SetActive(false);
                space.SetActive(false);
                charging = false;
                playerAni.SetBool("ChargeUp", false);
                ToBattle();
            }

            chargeBar.GetComponent<Image>().fillAmount = countPresses / chargeGoal;
            timeBar.GetComponent<Image>().fillAmount = currentTimer / timeBarTimer;
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) ClickBackButton();
        mpImg.sprite = mpSprites[mp];
    }


    public void FindRemainingUnit()
    {
        remainingEnemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
        remainingPlayerUnits = GameObject.FindGameObjectsWithTag("Player");
    }

    /// <summary>
    /// 戰鬥設置
    /// </summary>
    /// <returns></returns>
    IEnumerator SetupBattle()
    {
        playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerState = playerGO.GetComponent<CharacterStats>();
        originalHealth = playerState.characterData.currentHealth;
        playerAni = playerGO.GetComponent<Animator>();
        string playerName = playerGO.name;
        playerName = playerName.Replace("(Clone)", string.Empty);
        playerGO.name = playerName;

        enemyGO = Instantiate(monsters[(int)(Random.value * monsters.Length)], enemyBattleStation);
        string mstName = enemyGO.name;
        mstName = mstName.Replace("(Clone)", string.Empty);
        enemyGO.name = mstName;

        //創建參戰列表
        battleUnits = new List<GameObject>();

        //添加玩家單位至參戰列表
        playerUnits = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerUnit in playerUnits)
        {
            battleUnits.Add(playerUnit);
        }

        //添加怪物單位至參戰列表
        enemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyUnit in enemyUnits)
        {
            battleUnits.Add(enemyUnit);
        }

        yield return new WaitForSeconds(1f);//等待時間



        //對參戰單位列表進行排序
        //listSort();

        //開始戰鬥
        ToBattle();
    }


    /// <summary>
    /// 判斷戰鬥進行的條件是否滿足，取出參戰列表第一單位，並從列表移除該單位，單位行動
    /// 行動完後重新添加單位至隊列，繼續ToBattle()
    /// </summary>
    public void ToBattle()
    {
        StartCoroutine(DelayBattle());
    }

    public IEnumerator DelayBattle()
    {
        //string str = "";
        //foreach (GameObject obj in battleUnits)
        //{
        //    str += obj.name + ",";
        //}
        //battleUnitsText.text = str;

        yield return new WaitForSeconds(1f);
        //Debug.Log("ToBattle");

        lockImg.gameObject.SetActive(false);
        defenseSucceeded = false;

        FindRemainingUnit();


        //檢查存活敵人單位
        if (remainingEnemyUnits.Length == 0)
        {
            Debug.Log("敵人全滅，戰鬥勝利");
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        //檢查存活玩家單位
        else if (remainingPlayerUnits.Length == 0)
        {
            Debug.Log("我方全滅，戰鬥失敗");
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            //取出參戰列表第一單位，並從列表移除
            currentActUnit = battleUnits[0];
            battleUnits.Remove(currentActUnit);
            //重新將單位添加至參戰列表末尾
            battleUnits.Add(currentActUnit);

            //Debug.Log("當前攻擊者：" + currentActUnit.name);

            //獲取該行動單位的屬性組件
            CharacterStats currentActUnitStats = currentActUnit.GetComponent<CharacterStats>();
            BuffableEntity currentActUnitBuffs = currentActUnit.GetComponent<BuffableEntity>();
            //判斷取出的戰鬥單位是否存活
            if (!currentActUnitStats.IsDead() && currentActUnit.tag == "Player")
            {
                Instantiate(yourTurnWord, wordStation.position, Quaternion.identity);
                currentActUnitBuffs.CheckCharacterBuffs();
                state = BattleState.PLAYERTURN;
                delay = LeanTween.delayedCall(1.5f, () =>
                {
                    waitPlayerChooseButton = true;
                });
                atkBtn.Select();
            }
            else if (!currentActUnitStats.IsDead() && currentActUnit.tag == "Enemy")
            {
                Instantiate(enemyTurnWord, wordStation.position, Quaternion.identity);
                currentActUnitBuffs.CheckCharacterBuffs();
                state = BattleState.ENEMYTURN;
                EnemyChooseSkill();
                FindTarget();
                //產生箭頭
                //StartCoroutine(GenerateArrows());
            }
            else
            {
                //目標死亡，跳過回合
                StartCoroutine(DelayBattle());
            }
        }
    }


    /// <summary>
    /// 防禦
    /// </summary>
    void Defense()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) inputArrow = (int)Arrow.UP; //inputArrow = 0
        if (Input.GetKeyDown(KeyCode.DownArrow)) inputArrow = (int)Arrow.DOWN; //inputArrow = 1
        if (Input.GetKeyDown(KeyCode.LeftArrow)) inputArrow = (int)Arrow.LEFT; //inputArrow = 2
        if (Input.GetKeyDown(KeyCode.RightArrow)) inputArrow = (int)Arrow.RIGHT; //inputArrow = 3
        if (Input.GetKeyDown(KeyCode.Space)) inputArrow = (int)Arrow.SPACE; //inputArrow = 4
        if (inputArrow != -1)
        {
            if ((int)QuizArray[QuizCounter] == inputArrow)
            {
                // success
                for (int i = 0; i < arrowPositions[QuizCounter].childCount; i++)
                {
                    //Destroy(arrowPosition[QuizCounter].GetChild(i).gameObject);
                    arrowPositions[QuizCounter].GetChild(i).GetComponent<ArrowFadeOutAndDestory>().FadeOutAndDestory();
                }


                QuizCounter++;
            }
            else
            {
                // fail
                timeBar.transform.parent.gameObject.SetActive(false);
                defending = false;
                defenseSucceeded = false;
                dontResetDefenseTime = false;
                LaunchAttack();
                DestoryArrow();
            }

            if (QuizCounter >= arrowQuantity)
            {
                // full complete
                if (amountOfArrows == 0)
                {
                    timeBar.transform.parent.gameObject.SetActive(false);
                    defending = false;
                    defenseSucceeded = true;
                    dontResetDefenseTime = false;
                    LaunchAttack();
                }
                else
                {
                    if (currentActUnit.GetComponent<CharacterStats>().amountOfArrows > arrowPositions.Length)
                    {
                        dontResetDefenseTime = true;

                        if (amountOfArrows >= arrowPositions.Length)
                        {
                            StartCoroutine(GenerateArrows(arrowPositions.Length));

                        }
                        else
                        {
                            StartCoroutine(GenerateArrows(amountOfArrows));

                        }
                    }
                }
            }
        }
        if (QuizCounter < arrowPositions.Length)
        {
            currentTimer -= Time.deltaTime;
            timeBar.GetComponent<Image>().fillAmount = currentTimer / timeBarTimer;
            if (currentTimer < 0) currentTimer = 0;

            if (currentTimer <= 0)  //防禦時間到 防禦失敗
            {
                LaunchAttack();
                DestoryArrow();
                timeBar.transform.parent.gameObject.SetActive(false);
                defending = false;
                defenseSucceeded = false;
                dontResetDefenseTime = false;
            }
        }
    }




    /// <summary>
    /// 敵人選擇技能
    /// </summary>
    void EnemyChooseSkill()
    {
        if (currentActUnit.tag == "Enemy")
        {

            skillIndex = Random.Range(0, currentActUnit.GetComponent<CharacterStats>().totalSkills);
            skill = Resources.Load<SkillScriptableObject>
                ($"{currentActUnit.name}/{currentActUnit.name}Skill{skillIndex}");

            if (skill.needToCheck == true)
            {
                GameObject.Find("SkillCheck").SendMessage($"{currentActUnit.name}Check{skillIndex}");
                bool canUse = SkillCheck.canUse;
                if (canUse)
                {
                    if (skill.attackType == true)
                    {
                        amountOfArrows = currentActUnit.GetComponent<CharacterStats>().amountOfArrows;
                        if (amountOfArrows >= arrowPositions.Length)
                        {
                            StartCoroutine(GenerateArrows(arrowPositions.Length));
                        }
                        else
                        {
                            StartCoroutine(GenerateArrows(amountOfArrows));
                        }
                    }
                    else
                    {
                        LaunchAttack();
                    }
                }
                else
                {
                    EnemyChooseSkill();
                }
            }
            else
            {
                if (skill.attackType == true)
                {
                    amountOfArrows = currentActUnit.GetComponent<CharacterStats>().amountOfArrows;
                    if (amountOfArrows >= arrowPositions.Length)
                    {
                        StartCoroutine(GenerateArrows(arrowPositions.Length));
                    }
                    else
                    {
                        StartCoroutine(GenerateArrows(amountOfArrows));
                    }
                }
                else
                {
                    LaunchAttack();
                }
            }

            //Debug.Log($"選擇第{skillIndex}技能");
        }
        //else if (currentActUnit.tag == "Player")
        //{
        //    skillIndex = 0;
        //}
    }



    /// <summary>
    /// 查找攻擊目標，如果行動者是怪物則從剩餘玩家中隨機
    /// </summary>
    /// <returns></returns>
    void FindTarget()
    {
        if (currentActUnit.tag == "Enemy")
        {
            //如果行動單位是怪物則從存活玩家對象中隨機一個目標
            int targetIndex = Random.Range(0, remainingPlayerUnits.Length);
            currentActUnitTarget = remainingPlayerUnits[targetIndex];
        }
        else if (currentActUnit.tag == "Player")
        {
            //state = BattleState.PLAYERTURN;
            delay = LeanTween.delayedCall(0.1f, () =>
            {
                isWaitForPlayerToChooseTarget = true;
            });

        }
    }


    /// <summary>
    /// 點選攻擊對象
    /// </summary>
    void ChooseTarget()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider.gameObject.tag == "Enemy")
        {
            currentActUnitTarget = hit.collider.gameObject;

            lockImg.SetActive(true);
            lockImg.transform.position = new Vector2(currentActUnitTarget.transform.position.x, currentActUnitTarget.transform.position.y + currentActUnitTarget.GetComponent<BoxCollider2D>().size.y * currentActUnitTarget.transform.localScale.y / 2);
            lockImg.gameObject.GetComponent<Animator>().Play("Lock");
            //Debug.Log("攻擊目標為：" + currentActUnitTarget.name);
            isWaitForPlayerToChooseTarget = false;

            foreach (GameObject enemy in remainingEnemyUnits)
            {
                enemy.GetComponent<MouseOverMonster>().HideMark();
            }

            if (chooseMonsterWord != null) Destroy(chooseMonsterWord);

            //ChooseSkill();
            ReadyAttack(attackType);
        }
    }




    /// <summary>
    /// 當前行動單位執行攻擊動作
    /// </summary>
    public void LaunchAttack()
    {
        //播放攻擊動畫
        currentActUnit.GetComponent<Animator>().SetTrigger("Skill" + skillIndex);
        //Debug.Log("攻擊Skill" + skillIndex);

        //if (defenseSucceeded && skill.attackType == true)
        //{
        //    playerAni.SetTrigger("Defense");
        //}

        //currentActUnit.GetComponent<AudioSource>().Play();

        //Debug.Log(currentActUnit.name + "使用技能（" + attackTypeName + "）對" + currentActUnitTarget.name + "造成了" + attackData + "點傷害");
        //攻擊之後加個延時然後跑回來

        //在對象承受傷害前添加1s延遲（攻擊動作和特效需要時間）
        //StartCoroutine(WaitForTakeDamage());
    }



    /// <summary>
    /// 延時操作函數，避免在怪物回合操作過快
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForTakeDamage()
    {
        //存儲攻擊者和攻擊目標的屬性腳本
        CharacterStats attackOwner = currentActUnit.GetComponent<CharacterStats>();
        CharacterStats attackReceiver = currentActUnitTarget.GetComponent<CharacterStats>();


        //根據攻防計算傷害
        attackDamageMultiplier = currentActUnit.GetComponent<SkillData>().damageMultiplier;
        attackData = (attackOwner.CurrentAttack - attackReceiver.CurrentDefence + Random.Range(-2, 2)) * attackDamageMultiplier;
        //如果傷害小餘0 傷害等於0
        if (attackData <= 0) attackData = 0;
        //被攻擊者承受傷害
        attackReceiver.ReceiveDamage(attackData);
        //生成傷害的數字
        GameObject gb = Instantiate(floatPoint, currentActUnitTarget.transform.position, Quaternion.identity);
        //設定顯示的數字
        gb.transform.GetChild(0).GetComponent<TextMesh>().text = attackData.ToString();
        //生成血的特效
        Instantiate(bloodEffect, currentActUnitTarget.transform.position, Quaternion.identity);



        yield return new WaitForSeconds(1);


        //ToBattle();
    }



    /// <summary>
    /// 檢測是否攻擊成功
    /// </summary>
    /// <returns></returns>
    public bool DetectAttacks()
    {
        if (attackClock.eulerAngles.z >= attackTarget.eulerAngles.z - 16 && attackClock.eulerAngles.z <= attackTarget.eulerAngles.z + 16)
        {
            attackClock.eulerAngles = new Vector3(0, 0, 0);
            attackCircle.SetActive(false);
            space.SetActive(false);
            if (mp < 8) mp++;
            return true;
        }
        else
        {
            attackClock.eulerAngles = new Vector3(0, 0, 0);
            attackCircle.SetActive(false);
            space.SetActive(false);
            return false;
        }
    }





    /// <summary>
    /// 產生箭頭
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateArrows(int quantity)
    {
        arrowQuantity = quantity;
        QuizArray.Clear();
        QuizCounter = 0;

        if (!dontResetDefenseTime)
        {
            timeBarTimer = 2f;
            currentTimer = timeBarTimer;
            yield return new WaitForSeconds(1.5f);
        }

        for (int i = 0; i < quantity; i++)
        {
            int QTENumber = Random.Range(0, arrowPrefabs.Length);

            if (QTENumber == (int)Arrow.UP) { Instantiate(arrowPrefabs[QTENumber], arrowPositions[i]); QuizArray.Add(Arrow.UP); }
            if (QTENumber == (int)Arrow.DOWN) { Instantiate(arrowPrefabs[QTENumber], arrowPositions[i]); QuizArray.Add(Arrow.DOWN); }
            if (QTENumber == (int)Arrow.LEFT) { Instantiate(arrowPrefabs[QTENumber], arrowPositions[i]); QuizArray.Add(Arrow.LEFT); }
            if (QTENumber == (int)Arrow.RIGHT) { Instantiate(arrowPrefabs[QTENumber], arrowPositions[i]); QuizArray.Add(Arrow.RIGHT); }
            if (QTENumber == (int)Arrow.SPACE) { Instantiate(arrowPrefabs[QTENumber], arrowPositions[i]); QuizArray.Add(Arrow.SPACE); }
        }
        amountOfArrows -= quantity;
        defending = true;
        timeBar.transform.parent.gameObject.SetActive(true);

    }








    /// <summary>
    /// 刪除箭頭
    /// </summary>
    void DestoryArrow()
    {
        for (int i = 0; i < arrowPositions.Length; i++)
        {
            for (int k = 0; k < arrowPositions[i].transform.childCount; k++)
            {
                if (arrowPositions[i].transform.childCount == 0)
                    return;
                Destroy(arrowPositions[i].transform.GetChild(k).gameObject);
            }
        }
    }



    //public void EndBattleFadeToScene()
    //{
    //    StartCoroutine(EndBattle());
    //}
    /// <summary>
    /// 結束戰鬥 判斷贏或輸 跳回村莊
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndBattle()
    {
        if (remainingEnemyUnits.Length == 0)
        {
            yield return new WaitForSeconds(1f);
            playerAni.SetTrigger("Win");
            yield return new WaitForSeconds(3f);
            SceneData.SetPlayerPrePos = true;
            FindObjectOfType<SceneController>().FadeToScene(SceneData.PrevSceneName);

        }
        else if (remainingPlayerUnits.Length == 0)
        {
            yield return new WaitForSeconds(3f);
            SceneData.SetPlayerPrePos = true;
            FindObjectOfType<SceneController>().FadeToScene(SceneData.PrevSceneName);
            playerState.characterData.currentHealth = originalHealth;
        }

    }


    /// <summary>
    /// 準備攻擊 顯示攻擊轉盤、倒數
    /// </summary>
    /// <returns></returns>
    public void ReadyAttack(AttackType attackType)
    {
        if (attackType == AttackType.AttackCircle)
        {
            angle = 0;//指針角度從零開始
            attackTarget.eulerAngles = new Vector3(0, 0, Random.Range(45, 320));//有效區角度範圍
            attackCircle.SetActive(true);
            space.SetActive(true);
            imgFade.ImgFadeOutFadeIn(readyWord);
            //StartCoroutine(DelayAttack(2.7f));
            delay = LeanTween.delayedCall(2.7f, () =>
            {
                attacking = true;
            });
        }
        if (attackType == AttackType.ChargeBar)
        {
            timeBarTimer = 2f;
            currentTimer = timeBarTimer;
            countPresses = 0;
            chargeBar.GetComponent<Image>().fillAmount = countPresses / chargeGoal;
            timeBar.GetComponent<Image>().fillAmount = currentTimer / timeBarTimer;
            chargeBar.transform.parent.gameObject.SetActive(true);
            space.SetActive(true);
            timeBar.transform.parent.gameObject.SetActive(true);
            imgFade.ImgFadeOutFadeIn(readyWord);
            delay = LeanTween.delayedCall(2.7f, () =>
            {
                charging = true;
            });

        }


    }



    /// <summary>
    /// 指針旋轉
    /// </summary>
    public void PointerRotation()//指針旋轉
    {
        if (attackCircle.activeInHierarchy == false)
            return;
        //設定每個 frame 的旋轉速度。
        angle += rotationspeed * Time.deltaTime;

        //旋轉(每個 frame)。
        attackClock.eulerAngles = new Vector3(0, 0, angle);
        //logText.text = attackClock.eulerAngles.z.ToString("0");//顯示角度

        //轉完一圈關閉
        if (angle >= 360)
        {
            //攻擊失敗
            attackCircle.SetActive(false);
            space.SetActive(false);
            angle = 0;

            ToBattle();
            attacking = false;
        }
    }



    /// <summary>
    /// 點擊攻擊按鈕
    /// </summary>
    public void ClickAttackButton()
    {
        if (waitPlayerChooseButton)
        {
            attackType = AttackType.AttackCircle;
            EventSystem.current.SetSelectedGameObject(null);
            waitPlayerChooseButton = false;
            chooseMonsterWord = Instantiate(chooseMonsterWordPre, wordStation);
            //foreach(GameObject enemy in remainingEnemyUnits)
            //{
            //    float enemyPosY = enemy.transform.position.y;
            //    Instantiate(marked, new Vector3(enemy.transform.position.x, enemyPosY += (enemy.GetComponent<BoxCollider2D>().size.y)* enemy.transform.localScale.y, 0), Quaternion.identity);

            //}
            remainingEnemyUnits[0].GetComponent<MouseOverMonster>().ShowMark();
            indexOfMonster = 0;
            FindTarget();
            skillIndex = 0;
            //Debug.Log("點攻擊按鈕");
        }
    }



    /// <summary>
    /// 點擊道具按鈕
    /// </summary>
    public void ClickPropsButton()
    {
        if (waitPlayerChooseButton)
        {
            usingProps = true;
            waitPlayerChooseButton = false;
            refreshProps.CreateProps();
        }
    }



    /// <summary>
    /// 使用道具
    /// </summary>
    public void UseProps()
    {
        usingProps = false;
        refreshProps.DestoryProps();
        ToBattle();
    }



    //點擊返回按鈕
    public void ClickBackButton()
    {
        waitPlayerChooseButton = true;
        isWaitForPlayerToChooseTarget = false;
        refreshProps.DestoryProps();
        foreach (GameObject enemy in remainingEnemyUnits)
        {
            enemy.GetComponent<MouseOverMonster>().HideMark();
        }
        skillBtn1.gameObject.SetActive(false);
    }


    //點擊屬性按鈕
    public void ClickAttributesButton()
    {
        if (waitPlayerChooseButton)
        {
            skillBtn1.gameObject.SetActive(true);
            //EventSystem.current.SetSelectedGameObject(null);
            waitPlayerChooseButton = false;
        }
    }


    //點擊技能1
    public void ClickSkillBtn1()
    {
        if (mp >= 2)
        {
            attackType = AttackType.ChargeBar;
            mp -= 2;
            TooltipSystem.Hide();
            skillBtn1.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            chooseMonsterWord = Instantiate(chooseMonsterWordPre, wordStation);
            //foreach(GameObject enemy in remainingEnemyUnits)
            //{
            //    float enemyPosY = enemy.transform.position.y;
            //    Instantiate(marked, new Vector3(enemy.transform.position.x, enemyPosY += (enemy.GetComponent<BoxCollider2D>().size.y)* enemy.transform.localScale.y, 0), Quaternion.identity);

            //}
            remainingEnemyUnits[0].GetComponent<MouseOverMonster>().ShowMark();
            indexOfMonster = 0;
            FindTarget();
            skillIndex = 1;
            Debug.Log("點技能1按鈕");
        }
    }

    IEnumerator DelayAttack(float sec)
    {
        yield return new WaitForSeconds(sec);
        attacking = true;
    }
}
