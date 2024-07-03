using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/New Skill")]
public class SkillScriptableObject : ScriptableObject
{
    public bool attackType;
    public List<GameObject> skillPrefab = new List<GameObject>();
    public bool needToCheck;
    public ScriptableBuff scriptableBuff;
    public int times = 1;
    public GameObject skillText;
}
