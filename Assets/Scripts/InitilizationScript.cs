﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitilizationScript : MonoBehaviour
{
    /*
     * All initilization done here, Game speed, level adjustment and other data of the player will be setted and getted in here
     * Plugins and integrations will be done in here 
     * 
     */
    void Awake()
    {
       
        if(!PlayerPrefs.HasKey("TapTapRush")) //New User just opened the game
        {
            Debug.Log("New User. Setting parameters..");

            PlayerPrefs.SetInt("TapTapRush", 1);

            PlayerPrefs.SetFloat("BoreSpeed", 1);
            PlayerPrefs.SetFloat("MonsterSpeed", 0.5f);

            PlayerPrefs.SetInt("HighScore", 0);

            PlayerPrefs.SetInt("MaxLevel", 1);
            PlayerPrefs.SetInt("Level", 1);
        }
        else if(PlayerPrefs.HasKey("TapTapRush")) //Count Game Activity
        {
            int GameInitCount = PlayerPrefs.GetInt("TapTapRush");
            PlayerPrefs.SetInt("TapTapRush", GameInitCount + 1);
        }
    
    }


    void Start()
    {
        SetParameters();
    }

    private void SetParameters()
    {
        Data.charSpeed = PlayerPrefs.GetInt("BoreSpeed");
        Data.monsSpeed = PlayerPrefs.GetInt("MonsterSpeed");
        Data.HighScore = PlayerPrefs.GetInt("HighScore");
        Data.MaxLevel = PlayerPrefs.GetInt("MaxLevel");
        Data.Level = PlayerPrefs.GetInt("Level");
    }


}