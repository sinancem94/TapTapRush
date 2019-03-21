public class LevelManager 
{
    private int LevelNumber;
    public bool isBoostAllowed;
    private readonly float initialLength;
    private readonly float initialDistance;
    private readonly float initialMonsterSpeed; 

    public float length;
    public float distanceThreshold; //Used for level endings which distance btw monster & player used for determining whether condition occured.

    public LevelWidth levelWidth;
    public LevelBlockType levelBlockType;
    public LevelFinishtype levelFinishtype;


    public LevelManager(int lvl)
    {
        LevelNumber = lvl;
        initialLength = 50;
        initialDistance = 12f;
        initialMonsterSpeed = Data.GetInitialMonsterSpeed();
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
        Boost,
        None
    }

    public void SetParametersForLevel(int lvl,ref float monsSpeed, ref bool boostAllow)
    {
        LevelNumber = lvl;
        monsSpeed = SetMonsterSpeed();
        boostAllow = SetLevelType();
        SetLevelParamsForEnding();
    }

    private float SetMonsterSpeed()
    {
        float speedExtended = initialMonsterSpeed * 100;

        float coefficient =  (LevelNumber != 0 && LevelNumber < 7) ? (float)(LevelNumber) / 12 : (LevelNumber == 0) ? 0 : (float)(LevelNumber) / 30;
        speedExtended = MathCalculation.GetCoeffNum(coefficient, speedExtended, 120f);

        return speedExtended / 100f;
    }

    //Level Finishing parameters arrangment START.

    //Could use etiher of length or time to finish game..
    //Going with length for know.

    private void SetLengthOfRoad()
    {
        float coefficient = (LevelNumber != 0) ? (float)(LevelNumber) / 12 : 0;
        length =(int)MathCalculation.GetCoeffNum(coefficient, initialLength, 200);
    }

    private void SetDistanceForPassCondition() 
    {
        float coefficient = (LevelNumber != 0) ? (float)(LevelNumber) / 12 : 0;
        distanceThreshold = (int)MathCalculation.GetCoeffNum(coefficient, initialDistance, 20f);
    }

    private void SetTimeOfLevel()
    {
        
    }

    private void SetLevelParamsForEnding()
    {
        switch (levelFinishtype)
        {
            case LevelFinishtype.Length:
                SetLengthOfRoad();
                break;
            case LevelFinishtype.Distance:
                SetDistanceForPassCondition();
                break;
            default:
                break;
        }
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
     *
    */

    private bool SetLevelType()
    {
       /* switch((int)(LevelNumber / 3))
        {
            case 0: // If first three level
                break;
            case 1: // If second three level
                break;
            case 2: // If third three level
                break;
            case 3: // If fourth three level
                break;
        }*/

        if ((int)(LevelNumber / 3) == 0) // If first three level
        {
            levelBlockType = LevelBlockType.Normal;
            levelWidth = LevelWidth.Three;

            if (LevelNumber % 3 == 0) //First level
            { 
                levelFinishtype = LevelFinishtype.Distance;
                isBoostAllowed = false;
            }
            else if (LevelNumber % 3 == 1) //second level
            {
                levelFinishtype = LevelFinishtype.Length;
                isBoostAllowed = true;
            }
            else if (LevelNumber % 3 == 2) //3rd level
            {
                levelFinishtype = LevelFinishtype.Length;
                isBoostAllowed = true;
            }
        }
        else if ((int)(LevelNumber / 3) == 1) // If second three level
        {
            levelWidth = LevelWidth.Three;

            if (LevelNumber % 3 == 0) //4th level
            {
                levelBlockType = LevelBlockType.Reverse;
                levelFinishtype = LevelFinishtype.Length;
                isBoostAllowed = false;
            }
            else if (LevelNumber % 3 == 1) //5th level
            {
                levelBlockType = LevelBlockType.Mixed;
                levelFinishtype = LevelFinishtype.Distance;
                isBoostAllowed = true;
            }
            else if (LevelNumber % 3 == 2) //6th level
            {
                levelBlockType = LevelBlockType.Mixed;
                levelFinishtype = LevelFinishtype.Length;
                isBoostAllowed = true;
            }
        } 
        else if ((int)(LevelNumber / 3) == 2) // If third three level
        {
            levelWidth = LevelWidth.Five;
            levelBlockType = LevelBlockType.Normal;

            if (LevelNumber % 3 == 0) //7th level
            {
                levelFinishtype = LevelFinishtype.Distance;
                isBoostAllowed = true;
            }
            else if (LevelNumber % 3 == 1) //8th level
            {

                levelFinishtype = LevelFinishtype.Length;
                isBoostAllowed = false;
            }
            else if (LevelNumber % 3 == 2) //9th level
            {

                levelFinishtype = LevelFinishtype.Length;
                isBoostAllowed = true;
            }
        } 
        else if ((int)(LevelNumber / 3) == 3) // If fourth three level
        {
            levelWidth = LevelWidth.Five;


            if (LevelNumber % 3 == 0) //7th level
            {
                levelBlockType = LevelBlockType.Normal;
                levelFinishtype = LevelFinishtype.Length;
                isBoostAllowed = true;
            }
            else if (LevelNumber % 3 == 1) //8th level
            {
                levelBlockType = LevelBlockType.Mixed;
                levelFinishtype = LevelFinishtype.Distance;
                isBoostAllowed = false;
            }
            else if (LevelNumber % 3 == 2) //9th level
            {
                levelBlockType = LevelBlockType.Mixed;
                levelFinishtype = LevelFinishtype.Length;
                isBoostAllowed = true;
            }
        }

        return isBoostAllowed;
    }




    public bool IsEndingConditionSatisfied(float distanceBtwMonster, int PassedRoadLength)
    {
        bool didPassed = false;

        switch (levelFinishtype) 
        {
            case LevelFinishtype.Length:
                didPassed = (PassedRoadLength >= length) ? true : false;
                if (didPassed)
                    IncreaseLevelNumber();
                break;
            case LevelFinishtype.Distance:
                didPassed = (distanceBtwMonster >= distanceThreshold) ? true : false;
                if (didPassed)
                    IncreaseLevelNumber();
                break;
            case LevelFinishtype.Boost:
                didPassed = false;
                if (didPassed)
                    IncreaseLevelNumber();
                break;
            case LevelFinishtype.None: //endless sa hep devam ediyor
                didPassed = false;
                break;
            default:
                break;
        }

        return didPassed;
    }

    public void IncreaseLevelNumber()
    {
        LevelNumber += 1;
        Data.UpdateLevelData(LevelNumber);
    }
}
