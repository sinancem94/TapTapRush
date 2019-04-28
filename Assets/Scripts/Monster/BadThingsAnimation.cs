using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadThingsAnimation
{ 

    public float BadThingBoostEntering()
    {
        float monsSpeed = Platform.instance.GetMonsterSpeed();
        Platform.instance.SetMonsterSpeed(0);

        return monsSpeed;
    }

    public void BadThingsAnimationEnter(float spd)
    {
        Platform.instance.SetMonsterSpeed(spd * 4);
    }

    public void BadThingsBoostExit(float spd)
    {                      
        Platform.instance.SetMonsterSpeed(spd);
    }
}