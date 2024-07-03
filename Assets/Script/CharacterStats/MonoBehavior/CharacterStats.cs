using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterData;
    public Inventory playerBag, playerProps;
    public BattleSystem battleSystem;
    public int totalSkills;

    private bool dead = false;
    private Animator animator;
    public float MaxHealth;
    public float CurrentHealth;
    public float CurrentDefence;
    public float CurrentSpeed;
    public float CurrentAttack;
    public int amountOfArrows;
    public List<Item> treasures;
    public GameObject treasurePrefab;
    public int money; //掉落金錢量
    private int maxDrops = 2; // 最大掉落數量
    private float baseDropRate = 0.9f; // 基本掉落機率
    private float dropRatePerExtraDrop = 0.22f; // 每多掉落一個寶物，掉落機率就減少這個值

    private void Start()
    {
        battleSystem = FindObjectOfType<BattleSystem>();
        animator = GetComponent<Animator>();
        if (gameObject.tag == "Player")
        {
            MaxHealth = characterData.maxHealth;
            CurrentHealth = characterData.currentHealth;
            CurrentDefence = characterData.currentDefence;
            CurrentSpeed = characterData.currentSpeed;
            CurrentAttack = characterData.currentAttack;
        }
    }
    private void Update()
    {
        if (gameObject.tag == "Player")
        {
            characterData.maxHealth = MaxHealth;
            characterData.currentHealth = CurrentHealth;
            characterData.currentDefence = CurrentDefence;
            characterData.currentSpeed = CurrentSpeed;
            characterData.currentAttack = CurrentAttack;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
            }
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
                if (characterData == null)
                    return;
                characterData.currentHealth = MaxHealth;
            }

        }

        float SetFloat = CurrentHealth % MaxHealth <= MaxHealth * 0.1 &&
                CurrentHealth % MaxHealth != 0 ? 1 : 0;
        animator.SetFloat("Tired", SetFloat);

    }

    public void ReceiveDamage(float damage)
    {
        CurrentHealth -= damage;
        //characterData.currentHealth -= damage;
        if (CurrentHealth > 0)
        {
            animator.SetTrigger("TakeDamage");
        }
        else
        {
            gameObject.tag = "DeadUnit";
            CurrentHealth = 0;
            animator.SetTrigger("Dead");
            dead = true;
            Destroy(gameObject, 3f);
            BattleSystem.battleUnits.Remove(gameObject);
            if (gameObject.tag != "Player")
            {
                DropTreasure();
            }

            if (gameObject.tag == "Player")
            {
                characterData.currentHealth = CurrentHealth;
            }



            //battleSystem.FindRemainingUnit();
            //if (battleSystem.remainingPlayerUnits.Length == 0 || battleSystem.remainingEnemyUnits.Length == 0)
            //{
            //    battleSystem.SendMessage("EndBattleFadeToScene");
            //}
        }

    }

    public bool IsDead()
    {
        return dead;
    }

    private void DropTreasure()
    {
        Vector3 pos = transform.position;
        Instantiate(treasurePrefab, new Vector3(pos.x + Random.Range(-0.4f, 0.5f), pos.y + Random.Range(-0.4f, 0.5f), pos.z), Quaternion.identity);
        characterData.currentMoney += money;

        if (treasures.Count <= 0)
            return;

        int numDrops = Random.Range(0, maxDrops + 1); // 隨機決定掉落數量
        float dropChance = baseDropRate - (numDrops * dropRatePerExtraDrop); // 計算掉落機率

        if (Random.value <= dropChance) // 根據掉落機率決定是否掉落
        {
            for (int i = 0; i < numDrops; i++)
            {
                int prefabIndex = Random.Range(0, treasures.Count);
                Item treasure = Instantiate(treasures[prefabIndex]);
                AddNewItem(treasure);
                GameObject obj = Instantiate(treasurePrefab, new Vector3(pos.x + Random.Range(-0.4f, 0.5f), pos.y + Random.Range(-0.4f, 0.5f), pos.z), Quaternion.identity);
                obj.GetComponent<SpriteRenderer>().sprite = treasure.itemImage;
            }
        }
    }

    public void AddNewItem(Item item)
    {
        if (item.Equip == false)
        {
            for (int i = 0; i < playerProps.itemList.Count; i++)
            {
                if (playerProps.itemList[i] != null && playerProps.itemList[i].itemName == item.itemName)
                {
                    playerProps.itemList[i].itemHeld += 1;
                    return;
                }
            }
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i] != null && playerBag.itemList[i].itemName == item.itemName)
                {
                    playerBag.itemList[i].itemHeld += 1;
                    return;
                }
            }
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i] == null)
                {
                    playerBag.itemList[i] = item;
                    item.itemHeld += 1;
                    return;
                }
            }

        }

        if (item.Equip == true)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i] == null)
                {
                    playerBag.itemList[i] = item;
                    break;
                }
            }

        }
    }



    public void PlayCurrentActUnitAudio(AudioClip audioClip)
    {
        AudioManager.CurrentActUnitAudio(audioClip);
    }

    public void PlayCurrentActUnitTargetAudio(AudioClip audioClip)
    {
        AudioManager.CurrentActUnitTargetAudio(audioClip);
    }



    //public float MaxHealth
    //{
    //    get { if (characterData != null) return characterData.maxHealth; else return 0; }
    //    set { characterData.maxHealth = value; }
    //}
    //public float CurrentHealth
    //{
    //    get { if (characterData != null) return characterData.currentHealth; else return 0; }
    //    set
    //    {
    //        characterData.currentHealth = value;
    //        if (characterData.currentHealth <= 0)
    //            characterData.currentHealth = 0;
    //        if (characterData.currentHealth >= characterData.maxHealth)
    //            characterData.currentHealth = characterData.maxHealth;
    //    }

    //}
    //public float CurrentDefence
    //{
    //    get { if (characterData != null) return characterData.currentDefence; else return 0; }
    //    set { characterData.currentDefence = value; }
    //}
    //public float CurrentSpeed
    //{
    //    get { if (characterData != null) return characterData.currentSpeed; else return 0; }
    //    set { characterData.currentSpeed = value; }
    //}
    //public float CurrentAttack
    //{
    //    get { if (characterData != null) return characterData.currentAttack; else return 0; }
    //    set { characterData.currentAttack = value; }
    //}




}
