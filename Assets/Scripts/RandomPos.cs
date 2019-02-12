using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomPos
{
    public static int RandomPosition(int exRandom, int samenumb,int maxValue)
    {
        int randomIndex = Random.Range(0, maxValue);

        if (exRandom == randomIndex && samenumb >= 4)
        {
            if (randomIndex == 0)
            {
                randomIndex += 1;
            }
            else if (randomIndex == 1)
            {
                randomIndex -= 1;
            }
            return randomIndex;
        }
        else if (exRandom == randomIndex && samenumb < 1)
        {
            return randomIndex;
        }
        else
        {
            return randomIndex;
        }
    }
}
