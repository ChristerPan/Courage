using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TimedPoisonBuff : TimedBuff
{
    private readonly CharacterStats _characterStats;
    float damage;
    public TimedPoisonBuff(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        _characterStats = obj.GetComponent<CharacterStats>();

    }

    public override void ApplyEffect()
    {
        ScriptablePoisonBuff poisonBuff = (ScriptablePoisonBuff)Buff;
        damage = (int)((_characterStats.MaxHealth * poisonBuff.DeductMaxHpPercentage) / 100);
        _characterStats.ReceiveDamage(damage);


        //生成傷害的數字
        GameObject floatPoint = Object.Instantiate(Resources.Load<GameObject>("FloatPoint/FloatPointBase"), _characterStats.transform.position, Quaternion.identity);
        //設定顯示的數字
        floatPoint.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
        floatPoint.transform.GetChild(0).GetComponent<TextMesh>().color = new Color32(155, 9, 171, 255);

        //Debug.Log($"DeductMaxHpPercentage：{(_characterStats.MaxHealth * poisonBuff.DeductMaxHpPercentage) / 100}，{_characterStats.gameObject.name}");
    }

    public override void End()
    {
        EffectStacks = 0;
    }
}
