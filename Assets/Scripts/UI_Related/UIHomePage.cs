using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHomePage : MonoBehaviour
{
    public GameObject postProcessing;
    private InputField charSpeed;
    private InputField monsSpeed;

    private Text errorText;

    private void Start()
    {
        postProcessing = GameObject.FindGameObjectWithTag("PostProcessing");

        if(GameObject.FindGameObjectWithTag("ChangeCharSpeed"))
            charSpeed = GameObject.FindGameObjectWithTag("ChangeCharSpeed").GetComponent<InputField>();
        if(GameObject.FindGameObjectWithTag("ChangeMonsSpeed"))
            monsSpeed = GameObject.FindGameObjectWithTag("ChangeMonsSpeed").GetComponent<InputField>();
        if(charSpeed)
            charSpeed.placeholder.GetComponent<Text>().text = "Change Bore's Speed \n" + "Current is : " + Data.charSpeed.ToString("0.0");
        if(monsSpeed)
            monsSpeed.placeholder.GetComponent<Text>().text = "Change Monster Speed \n" + "Current is : " + Data.monsSpeed.ToString("0.0");

        errorText = GameObject.FindGameObjectWithTag("ErrorText").GetComponent<Text>();

        SetLevelInfoText();
        //errorText.gameObject.SetActive(false);
        //if(!Data.isPostProcessing)
        //postProcessing.SetActive(false);
    }


  /*  public void ChangeAngle() //for mode
    {
        if (!Data.isAngled)
        {
            Data.isAngled = true;
            //modeButton.GetComponentInChildren<Text>().text = "açılı";
            Platform.instance.ChangeAngle();
        }
        else
        {
            Data.isAngled = false;
            //modeButton.GetComponentInChildren<Text>().text = "açısız";
            Platform.instance.ChangeAngle();
        }
    }*/


  /*  public void ChangeView() //for postprocessing or not
    {
        if(Data.isPostProcessing)
        {
            Data.isPostProcessing = false;
            postProcessing.SetActive(false);    
            //Camera.m
        }
        else
        {
            Data.isPostProcessing = true;
            postProcessing.SetActive(true);
        }

    }*/

   /* public void ChangeMode() // for 5 line or not
    {
        if(Data.is5Line)
        {
            Data.is5Line = false;
        }
        else
        {
            Data.is5Line = true;
        }
        Platform.instance.CreatePlatform();
    }*/

    public void ChangeCharacterSpeed()
    {
        float spd = float.Parse(charSpeed.text);

        if(spd < 1 || spd > 3)
        {
            //errorText.gameObject.SetActive(true);
            errorText.text = "Character speed must be between 1.0 and 3.0";
        }
        else
        {
            //errorText.gameObject.SetActive(false);
            SetLevelInfoText();
            PlayerPrefs.SetFloat("BoreSpeed", float.Parse(charSpeed.text));
            Data.charSpeed = PlayerPrefs.GetFloat("BoreSpeed");
            Platform.instance.SetSpeeds();
        }
    }

   /* public void ChangeMonsterSpeed()
    {
        float spd = float.Parse(monsSpeed.text);

        if (spd < 0.5 || spd > 1.5)
        {
            //errorText.gameObject.SetActive(true);
            errorText.text = "Monster speed must be between 0.5 and 1.5";
        }
        else
        {
            //errorText.gameObject.SetActive(false);
            SetLevelInfoText();
            PlayerPrefs.SetFloat("MonsterSpeed", float.Parse(monsSpeed.text));
            Data.monsSpeed = PlayerPrefs.GetFloat("MonsterSpeed");
            Platform.instance.SetSpeeds();
        }
    }*/

    public void SetLevelInfoText() 
    {
        //errorText.gameObject.SetActive(true);
        errorText.text = "Level is : " + (Data.GetLevel() + 1) + "\n";
        errorText.text += "Monster Speed is : " + Platform.instance.Nightmare.GetComponent<BadThingParticleSystem>().monsterSpeed + "\n\n";
       
        if(Platform.instance.levelManager.levelFinishtype == LevelManager.LevelFinishtype.Length) 
        {
            //errorText.text += "Level finishes with length\n";
            errorText.text += "Finish when " + Platform.instance.levelManager.length + " blocks passed.\n";
        }
        else if (Platform.instance.levelManager.levelFinishtype == LevelManager.LevelFinishtype.Distance)
        {
            //errorText.text += "Level finishes with distance\n";
            errorText.text += "Finish when distance between monster and player : " + Platform.instance.levelManager.distanceThreshold + "\n\n";
        }

        errorText.text += "Block type is : " + Platform.instance.levelManager.levelBlockType + "\n";
        errorText.text += "Road width is : " + Platform.instance.levelManager.levelWidth + "\n";

    }

}
