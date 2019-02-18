using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomPos
{
    public static int RandomPosition(int exRandom, int samenumb,int maxValue)
    {
        int minValue = 0;

        if(Data.is5Line)
        {
            int r = Random.Range(0, 4);
            maxValue = (r == 3) ? maxValue : maxValue - 1;
            minValue = (r == 3) ? minValue : minValue + 1;
        }

        int randomIndex = Random.Range(minValue, maxValue);

        if (exRandom == randomIndex && samenumb >= 4)
        {
            if (randomIndex == minValue)
            {
                randomIndex += 1;
            }
            else if (randomIndex == maxValue)
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
