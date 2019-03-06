using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetData 
{
    public void GetParameters()
    {
        if (!Data.enteredSession)
        {
            Data.enteredSession = true;

            Data.charSpeed = 1f;
            Data.monsSpeed = 0.5f;
        }
    }

    public void SetParameters(Runner runner,BadThingParticleSystem nightmare)
    {
        runner.CharacterSpeed = Data.charSpeed;
        nightmare.monsterSpeed = Data.monsSpeed;
    }

}
