using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static int AppLaunchCount;

    public static float charSpeed; //Bore speed
    public static float monsSpeed; //Character speed

    public static int HighScore;

    public static int World;

    public static int MaxLevel;
    public static int Level; //Level starts from 0 and increases. because 0 is usefull while arranging levels

    public static Vector2 PlayerProgress; // x represents world and y is level. Used for getting latest position of world

    public static bool IsDebug = false;
    //PlayerPrefs are setted in initilizationScript and than mapped into Data
    public static void SetData()
    {
        AppLaunchCount = PlayerPrefs.GetInt("TapTapRush");
        charSpeed = PlayerPrefs.GetFloat("BoreSpeed");
        monsSpeed = PlayerPrefs.GetFloat("MonsterSpeed");
        HighScore = PlayerPrefs.GetInt("HighScore");
        MaxLevel = PlayerPrefs.GetInt("MaxLevel");
        Level = PlayerPrefs.GetInt("Level");
        World = PlayerPrefs.GetInt("World");

        PlayerProgress = SetProgressData();
    }

    static Vector2 SetProgressData()
    {
        return PlayerProgress = new Vector2(World,Level);
    }

    public static void UpdateLevelData(int currlevel)
    {
        Level = currlevel;

        if (Level > MaxLevel)
        {
            PlayerPrefs.SetInt("MaxLevel", Level);
            MaxLevel = Level;
        }
        PlayerPrefs.SetInt("Level", Level);
    }

    public static int GetMaxLevel()
    {
        return MaxLevel;
    }

    public static int GetLevel()
    {
        return Level;
    }

    public static float GetInitialMonsterSpeed()
    {
        return monsSpeed;
    }

    public static void ChangeLevel(int currlevel)
    {
        // if(currlevel <= MaxLevel) 
        //{
        Level = currlevel;
        PlayerPrefs.SetInt("Level", Level);
        //}
    }
}
