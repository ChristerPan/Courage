using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    //public int skillNum;
    //public GameObject[] skill;
    //public GameObject floatPoint;
    //public GameObject bloodEffect;
    public Animator playerAni;                         //玩家動畫控制器
    public float damageMultiplier;
    public BattleSystem BattleSystem;
    private static LTDescr delay;
    public GameObject whiteLine;
    private void Start()
    {
        playerAni = GameObject.Find("Player_Battle").GetComponent<Animator>();
        BattleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
    }

    //IEnumerator WaitForTakeDamage()
    //{
    //    BattleSystem.SendMessage("WaitForTakeDamage");
    //    yield return new WaitForSeconds(0.3f);
    //}
    public void ShowWhiteLine()
    {
        GameObject obj = GameObject.Find("Player_Battle");
        Vector3 pos = obj.transform.position;
        Instantiate(whiteLine, new Vector3(0, pos.y + 2.2f, 0), Quaternion.identity);
    }

    //勇者技能
    public void BraveSkill0()
    {
        //var skill = Resources.Load<SkillScriptableObject>("Brave/BraveSkill0");
        //AudioManager.CurrentActUnitAudio(skill.skillAudio);

        damageMultiplier = 1f;
        BattleSystem.SendMessage("WaitForTakeDamage");
        BattleSystem.SendMessage("ToBattle");
    }

    public void BraveSkill1()
    {
        var skill = Resources.Load<SkillScriptableObject>("Brave/BraveSkill1");
        int times = skill.times;

        Instantiate(skill.skillText, BattleSystem.wordStation);

        damageMultiplier = 1f;

        BattleSystem.SendMessage("WaitForTakeDamage");

        if (BattleSystem.currentActUnitTarget.GetComponent<CharacterStats>().CurrentHealth > 0)
        {
            for (int i = 0; i < times - 1; i++)
            {
                delay = LeanTween.delayedCall(0.2f, () =>
                {
                    BattleSystem.SendMessage("WaitForTakeDamage");
                });

            }
        }


        BattleSystem.SendMessage("ToBattle");
    }


    //史萊姆技能
    public void SlimeSkill0()
    {
        //var skill = Resources.Load<SkillScriptableObject>("Wolf/WolfSkill0");
        //AudioManager.CurrentActUnitAudio(skill.skillAudio);

        if (BattleSystem.defenseSucceeded == true)
        {
            playerAni.SetTrigger("Defense");
        }
        else
        {
            damageMultiplier = 1f;
            BattleSystem.SendMessage("WaitForTakeDamage");
        }
        BattleSystem.SendMessage("ToBattle");

    }

    //小沃夫技能
    public void WolfSkill0()
    {
        //var skill = Resources.Load<SkillScriptableObject>("Wolf/WolfSkill0");
        //AudioManager.CurrentActUnitAudio(skill.skillAudio);

        if (BattleSystem.defenseSucceeded == true)
        {
            playerAni.SetTrigger("Defense");
        }
        else
        {
            damageMultiplier = 1f;
            BattleSystem.SendMessage("WaitForTakeDamage");
        }
        BattleSystem.SendMessage("ToBattle");
    }


    //沃夫爾技能
    public void WaffleSkill0()
    {
        //var skill = Resources.Load<SkillScriptableObject>("Waffle/WaffleSkill0");
        //AudioManager.CurrentActUnitAudio(skill.skillAudio);


        if (BattleSystem.defenseSucceeded == true)
        {
            playerAni.SetTrigger("Defense");
        }
        else
        {
            damageMultiplier = 1f;
            BattleSystem.SendMessage("WaitForTakeDamage");
        }

        BattleSystem.SendMessage("ToBattle");
    }
    public void WaffleSkill1()
    {
        var skill = Resources.Load<SkillScriptableObject>("Waffle/WaffleSkill1");
        //AudioManager.CurrentActUnitAudio(skill.skillAudio);

        Instantiate(skill.skillText, BattleSystem.wordStation);

        GameObject obj = GameObject.Find("Waffle");
        GameObject objChild;
        for (int i = 0; i < 2; i++)
        {
            objChild = Instantiate(skill.skillPrefab[(int)(Random.value * skill.skillPrefab.Count)], obj.transform.GetChild(i).position, Quaternion.identity);
            //objChild.transform.SetParent(obj.transform.GetChild(i));
            BattleSystem.battleUnits.Add(objChild);
            string childName = objChild.name;
            childName = childName.Replace("(Clone)", string.Empty);
            objChild.name = childName;
        }


        //Debug.Log("召喚小怪");
        BattleSystem.SendMessage("ToBattle");


    }


    //受汙染的史萊姆技能
    public void TaintedSlimeSkill0()
    {
        //var skill = Resources.Load<SkillScriptableObject>("TaintedSlime/TaintedSlimeSkill0");
        //AudioManager.CurrentActUnitAudio(skill.skillAudio);

        if (BattleSystem.defenseSucceeded == true)
        {
            playerAni.SetTrigger("Defense");
        }
        else
        {
            damageMultiplier = 1f;
            BattleSystem.SendMessage("WaitForTakeDamage");
        }

        BattleSystem.SendMessage("ToBattle");
    }
    public void TaintedSlimeSkill1()
    {
        var skill = Resources.Load<SkillScriptableObject>("TaintedSlime/TaintedSlimeSkill1");
        //AudioManager.CurrentActUnitAudio(skill.skillAudio);

        Instantiate(skill.skillText, BattleSystem.wordStation);

        if (BattleSystem.defenseSucceeded == true)
        {
            playerAni.SetTrigger("Defense");
        }
        else
        {
            damageMultiplier = 1f;
            var targetBuffableEntity = BattleSystem.currentActUnitTarget.GetComponent<BuffableEntity>();
            targetBuffableEntity.AddBuff(skill.scriptableBuff.InitializeBuff(BattleSystem.currentActUnitTarget));
            BattleSystem.SendMessage("WaitForTakeDamage");
        }

        BattleSystem.SendMessage("ToBattle");
    }




    //草叢鬼
    public void BushGhostSkill0()
    {
        if (BattleSystem.defenseSucceeded == true)
        {
            playerAni.SetTrigger("Defense");
        }
        else
        {
            damageMultiplier = 1f;
            BattleSystem.SendMessage("WaitForTakeDamage");
        }

        BattleSystem.SendMessage("ToBattle");
    }


    //綠色史萊姆
    public void GreenSlimeSkill0()
    {
        if (BattleSystem.defenseSucceeded == true)
        {
            playerAni.SetTrigger("Defense");
        }
        else
        {
            damageMultiplier = 1f;
            BattleSystem.SendMessage("WaitForTakeDamage");
        }

        BattleSystem.SendMessage("ToBattle");
    }
}

