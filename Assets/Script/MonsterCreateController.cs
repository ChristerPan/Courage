using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCreateController : MonoBehaviour
{
    public GameObject monster;
    public int monsterCount;
    // Start is called before the first frame update
    void Start()
    {
        float x;
        for (int i = 0; i < monsterCount; i++)
        {
            x = UnityEngine.Random.Range(-5f, 20f);
            Instantiate(monster, new Vector3(x, 3, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
