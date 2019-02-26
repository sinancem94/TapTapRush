using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialButtons : MonoBehaviour
{
    public GameObject postProcessing;

    private void Start()
    {
        postProcessing = GameObject.FindGameObjectWithTag("PostProcessing");
        //if(!Data.isPostProcessing)
            //postProcessing.SetActive(false);
    }


    public void ChangeAngle() //for mode
    {
        if (!Data.isAngled)
        {
            Data.isAngled = true;
            //modeButton.GetComponentInChildren<Text>().text = "açılı";
            Platform.instance.ChangeAngle();
        }
        else
        {
            Data.isAngled = false;
            //modeButton.GetComponentInChildren<Text>().text = "açısız";
            Platform.instance.ChangeAngle();
        }
    }


    public void ChangeView() //for postprocessing or not
    {
        if(Data.isPostProcessing)
        {
            Data.isPostProcessing = false;
            postProcessing.SetActive(false);    
            //Camera.m
        }
        else
        {
            Data.isPostProcessing = true;
            postProcessing.SetActive(true);
        }

    }

    public void ChangeMode() // for 5 line or not
    {
        if(Data.is5Line)
        {
            Data.is5Line = false;
        }
        else
        {
            Data.is5Line = true;
        }
        Platform.instance.ChangeMode();
    }
}
