using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableBuff : ScriptableObject
{
    public string Name;

    /// <summary>
    /// buff的描述
    /// </summary>
    [Multiline()]
    public string description;
    /// <summary>
    /// buff 的持續回合
    /// </summary>
    public float Rounds;

    /// <summary>
    /// 是否每次應用buff時，持續時間都會增加。
    /// </summary>
    public bool IsDurationStacked;

    /// <summary>
    /// 是否每次應用buff時效果值都會增加。
    /// </summary>
    public bool IsEffectStacked;

    /// <summary>
    /// 是否每回合生效。
    /// </summary>
    public bool EffectiveEveryRound;

    /// <summary>
    /// 是否攻擊時需要即時生效。
    /// </summary>
    public bool EffectiveImmediately;

    public abstract TimedBuff InitializeBuff(GameObject obj);

    /// <summary>
    /// buff的圖示
    /// </summary>
    public Sprite buffImage;

    public bool isDebuff;
}
