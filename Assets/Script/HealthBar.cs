using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public CharacterData_SO characterData;
    public Text healthText;
    public float maxHp;
    public float currentHp;
    public Image healthbar;


    // Start is called before the first frame update
    void Start()
    {
        healthbar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHp = characterData.currentHealth;
        maxHp = characterData.maxHealth;

        healthbar.fillAmount = currentHp / maxHp;
        healthText.text= $"{currentHp}/{maxHp}";
    }
}
