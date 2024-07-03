using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esc : MonoBehaviour
{
    public List<GameObject> objs;
    public GameObject settingPanel;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(objs.TrueForAll(go => !go.activeSelf))
            {
                settingPanel.SetActive(true);
            }
            else
            {
                CloseObjectsInList();
            }
            
        }
    }

    public void CloseObjectsInList()
    {
        foreach(GameObject obj in objs)
        {
            obj.SetActive(false);
        }
    }
}
