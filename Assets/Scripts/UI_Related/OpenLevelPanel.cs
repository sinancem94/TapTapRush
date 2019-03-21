using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevelPanel : MonoBehaviour
{
    public void ChangePanelStatus()
    {
        GameObject panel = transform.GetChild(1).gameObject;

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
