using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffableEntity : MonoBehaviour
{
    public readonly Dictionary<ScriptableBuff, TimedBuff> _buffs = new Dictionary<ScriptableBuff, TimedBuff>();

    public void AddBuff(TimedBuff buff)
    {
        if (_buffs.ContainsKey(buff.Buff))
        {
            _buffs[buff.Buff].Activate();
        }
        else
        {
            _buffs.Add(buff.Buff, buff);
            buff.Activate();
        }
    }
    /// <summary>
    /// 角色每個Buff回合數-1，回合數等於0移除
    /// </summary>
    public void CheckCharacterBuffs()
    {
        //可選，如果遊戲暫停，則在更新每個 buff 之前返回
        //if (Game.isPaused)
        //    return;

        foreach (var buff in _buffs.Values.ToList())
        {
            if (buff.Buff.EffectiveEveryRound)
            {
                buff.ApplyEffect();
            }
            buff.Tick();
            if (buff.IsFinished)
            {
                _buffs.Remove(buff.Buff);
            }
        }
    }
}
