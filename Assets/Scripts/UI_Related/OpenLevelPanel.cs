using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevelPanel : MonoBehaviour
{
    private GameObject panel;

    private void Start()
    {
        panel = GameObject.FindWithTag("LevelPanel");
        panel.SetActive(false);
    }

    public void ChangePanelStatus()
    {
        if (!panel.activeInHierarchy) 
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }
}
