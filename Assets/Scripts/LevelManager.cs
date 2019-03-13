using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager 
{
    public int LevelNumber;


    public void IncreaseLevelNumber()
    {
        PlayerPrefs.SetInt("MaxLevel", Data.MaxLevel + 1);
        Data.MaxLevel = PlayerPrefs.GetInt("MaxLevel");

        Data.Level = Data.MaxLevel;
        PlayerPrefs.SetInt("Level", Data.Level);
    }



}
