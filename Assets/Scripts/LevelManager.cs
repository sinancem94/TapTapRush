using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager 
{
    private int LevelNumber;
    public bool isBoostAllowed;

    public LevelWidth levelWidth;
    public LevelBlockType levelBlockType;
    public LevelFinishtype levelFinishtype;


    public LevelManager(int lvl)
    {
        LevelNumber = lvl;
    }

    public enum LevelWidth
    {
        Three,
        Five
    }

    public enum LevelBlockType
    {
        Normal,
        Reverse,
        Mixed
    }

    public enum LevelFinishtype
    {
        Time,
        Distance,
        Length,
        Boost
    }

    public void SetParametersForLevel(ref float monsSpeed)
    {
        monsSpeed = SetMonsterSpeed(monsSpeed);
        SetLengthOfRoad();
        SetLevelType();
    }

    private float SetMonsterSpeed(float monsterSpeed)
    {
        float coefficient = (LevelNumber != 1) ? (float)(LevelNumber - 1) / 12 : 0;
        return monsterSpeed = MathCalculation.GetCoeffNum(coefficient, Data.monsSpeed, 1.5f);
    }

    //Level Finishing parameters arrangment START.

    //Could use etiher of length or time to finish game..
    //Going with length for know.

    private void SetLengthOfRoad()
    {
        
    }

    private void SetTimeOfLevel()
    {
        
    }

    //Level finishing parameters END.

    //Is road width will be 3 or 5.
    //Is blocks will be reverse or normal.
    //If reverse in what rate.
    //Can be used for deciding wherther level ends with road or time. 


    /*
     * First 3 level is normal
     * 1. Normal level short player default speed 1f. Monster is 0.5f. Finish when distance between monster and player 15. No Boost.
     * 2. Normal level monster speed increased. Player needs to enter boost in order to finish.
     * 3. Normal level long. Player needs to escape from monster. Monster could speed up during level ?
     * REVERSE BEGAN
     * 4. Full reverse short level. Same with level 1 beside reverse
     * 5. Reverse Normal mix. Reverse blocks are rare. Same with level 2 beside that.
     * 6. Reverse Normal mix. Same as level 3 beside that.
     * 
     * 7. 5 line start. Normal 5 line level same as level 1.
     * 8. 5 li orta uzunlukta.
     * 9. 5 li biraz zor bir bölüm yine canavar zamanla hızlanabilir ?
     * 
     * 10. 5 li ve komple ters bir bölüm zor olcak...
     * 11. 5 li ve ters ve düz karmaşık ebesi olcak..
     * 12. 5li ters düz boost karmakarışık ebesinin dini olcak.
     *  
     * 12 levelda bittikten sonra başka bir world olabilir ama oyun kısa oluyor böyle olunca.
    */

    private void SetLevelType()
    {
        if ((int)(LevelNumber / 3) == 0) // If first three level
        {
            levelBlockType = LevelBlockType.Normal;
            levelWidth = LevelWidth.Three;

            if (LevelNumber % 3 == 1)
            { 
                levelFinishtype = LevelFinishtype.Distance;
                isBoostAllowed = false;
            }
            else if (LevelNumber % 3 == 2)
            {
                levelFinishtype = LevelFinishtype.Distance;
                isBoostAllowed = false;
            }

        }
        else if ((int)(LevelNumber / 3) == 1) // If second three level
        {

        } 
        else if ((int)(LevelNumber / 3) == 2) // If third three level
        {

        } 
        else if ((int)(LevelNumber / 3) == 3) // If fourth three level
        {

        } 
       
    }

    public void IncreaseLevelNumber()
    {
        LevelNumber += 1;
        Data.Level = LevelNumber;

        if(Data.Level > Data.MaxLevel)
        {
            PlayerPrefs.SetInt("MaxLevel", Data.Level);
            Data.MaxLevel = Data.Level;
        }
        PlayerPrefs.SetInt("Level", Data.Level);
    }
}
