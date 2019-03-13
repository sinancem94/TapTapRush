using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static bool isAngled;
    public static bool isPostProcessing;
    public static bool is5Line;

    public static int AppLaunchCount;

    public static float charSpeed; //Bore speed
    public static float monsSpeed; //Character speed

    public static int HighScore;

    public static int MaxLevel;
    public static int Level;

    public static void SetData()
    {
        AppLaunchCount = PlayerPrefs.GetInt("TapTapRush");
        charSpeed = PlayerPrefs.GetFloat("BoreSpeed");
        monsSpeed = PlayerPrefs.GetFloat("MonsterSpeed");
        HighScore = PlayerPrefs.GetInt("HighScore");
        MaxLevel = PlayerPrefs.GetInt("MaxLevel");
        Level = PlayerPrefs.GetInt("Level");
    }
}
