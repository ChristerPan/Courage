using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/PoisonBuff")]
public class ScriptablePoisonBuff : ScriptableBuff
{
    /// <summary>
    /// 扣除最大血量百分比
    /// </summary>
    public float DeductMaxHpPercentage;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedPoisonBuff(this, obj);
    }
}
