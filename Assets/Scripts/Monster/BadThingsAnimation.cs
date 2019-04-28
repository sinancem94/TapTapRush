using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadThingsAnimation
{
    private bool canGetCloser;

    private static BadThingsAnimation instance;

    public BadThingsAnimation()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("There is already a BadThingsAnimation cannot create on more!!!");

        canGetCloser = true;
    }

    public Vector3 TranslationVector(float spd)
    {
        if (!canGetCloser && Platform.instance.distanceBtwRunner <= 5f)
            spd = 0f;

        return new Vector3(0f, spd * Time.deltaTime, 0f);    
    }

    public float BadThingBoostEntering()
    {
        float monsSpeed = Platform.instance.GetMonsterSpeed();
        Platform.instance.SetMonsterSpeed(0);

        return monsSpeed;
    }

    public void BadThingsAnimationEnter(float spd)
    {
        canGetCloser = false;
        Platform.instance.SetMonsterSpeed(spd * 4);
    }

    public void BadThingsBoostExit(float spd)
    {
        canGetCloser = true;
        Platform.instance.SetMonsterSpeed(spd);
    }
}