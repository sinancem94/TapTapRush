﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenLevelPanel : MonoBehaviour
{
    private GameObject panel;

    private void Start()
    {
        panel = GameObject.FindWithTag("LevelPanel");
        if (!panel)
            panel = transform.GetChild(0).GetChild(0).gameObject;

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
