using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetData 
{
    public void GetSpeedData(Runner runner,BadThingParticleSystem nightmare)
    {
        runner.CharacterSpeed = Data.charSpeed;
        nightmare.monsterSpeed = Data.monsSpeed;
    }
}
