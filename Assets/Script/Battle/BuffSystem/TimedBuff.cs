using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedBuff
{

    protected float Rounds;
    protected int EffectStacks;
    public ScriptableBuff Buff { get; }
    protected readonly GameObject Obj;
    public bool IsFinished;

    public TimedBuff(ScriptableBuff buff, GameObject obj)
    {
        Buff = buff;
        Obj = obj;
    }

    public void Tick()
    {
        Rounds -= 1;
        if (Rounds <= 0)
        {
            End();
            IsFinished = true;
        }
    }

    //如果 ScriptableBuff 將 IsDurationStacked 或 IsEffectStacked 設置為 true，則激活 buff 或延長持續時間。
    public void Activate()
    {
        
        if (Buff.IsEffectStacked || Rounds <= 0)
        {
            if (Buff.EffectiveImmediately)//如果攻擊時需要即時生效
            {
                ApplyEffect();
            }
            EffectStacks++;
        }

        if (Buff.IsDurationStacked || Rounds <= 0)
        {
            Rounds += Buff.Rounds;
            if (Buff.EffectiveImmediately)//如果攻擊時需要即時生效
            {
                Rounds += 1;
            }
        }

        if(!Buff.IsEffectStacked && !Buff.IsDurationStacked)//如果效果跟回合數都不能疊加
        {
            Rounds = Buff.Rounds;
            if (Buff.EffectiveImmediately)//如果攻擊時需要即時生效
            {
                Rounds += 1;
            }
        }


    }
    public abstract void ApplyEffect();
    public abstract void End();
}

