using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private CharacterStats characterStats;
    
    // Start is called before the first frame update
    void Start()
    {
        
        characterStats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //血量條
        //HealthBar.currentHp = characterStats.CurrentHealth;
        //HealthBar.maxHp = characterStats.MaxHealth;
    }

}
