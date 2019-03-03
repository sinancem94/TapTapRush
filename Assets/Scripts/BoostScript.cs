﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostScript : MonoBehaviour
{
    public void StartBoost(float timeChangeSpeed)
    {
        StartCoroutine(SlowTime(timeChangeSpeed, true));
    }
   

    public void StopBoost(float timeChangeSpeed)
    {
        StartCoroutine(SlowTime(timeChangeSpeed, false));
    }

    private IEnumerator SlowTime(float changeSpeed, bool isStarted)
    {
        if(isStarted)
        {
            Platform.instance.SetBoost(true); // setted true if start boost called this
        }

        while(Time.timeScale > 0.4f)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0.3f, Time.deltaTime * changeSpeed);
            yield return new WaitForSeconds(.01f);  
        }

        Time.timeScale = 0.4f;

        yield return new WaitForSeconds(.4f);

        while(Time.timeScale < 1f)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1.1f, Time.deltaTime * changeSpeed);
            yield return new WaitForSeconds(.01f);  
        }
        Time.timeScale = 1f;

        if(!isStarted)
        {
            Platform.instance.SetBoost(false); //Setted False after time speeds again
        }
          
        StopCoroutine(SlowTime(changeSpeed,isStarted));
    }
}