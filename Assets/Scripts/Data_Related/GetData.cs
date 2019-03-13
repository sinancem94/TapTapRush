using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wrapper Around Data for platform to use
public class GetData 
{
    public float GetBoreSpeed()
    {
        return Data.charSpeed;
    }

    public float GetMonsterSpeed()
    {
        return Data.monsSpeed;
    }
}
