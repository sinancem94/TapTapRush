using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrialButtons : MonoBehaviour
{
    public GameObject postProcessing;
    private InputField charSpeed;
    private InputField monsSpeed;

    private Text errorText;

    private void Start()
    {
        postProcessing = GameObject.FindGameObjectWithTag("PostProcessing");
        charSpeed = GameObject.FindGameObjectWithTag("ChangeCharSpeed").GetComponent<InputField>();
        monsSpeed = GameObject.FindGameObjectWithTag("ChangeMonsSpeed").GetComponent<InputField>();

        charSpeed.placeholder.GetComponent<Text>().text = "Change Bore's Speed Speed\n" + "Current is : " + Data.charSpeed.ToString("0.0");

        monsSpeed.placeholder.GetComponent<Text>().text = "Change Monster Speed Speed\n" + "Current is : " + Data.monsSpeed.ToString("0.0");

        errorText = GameObject.FindGameObjectWithTag("ErrorText").GetComponent<Text>();
        errorText.gameObject.SetActive(false);
        //if(!Data.isPostProcessing)
            //postProcessing.SetActive(false);
    }


    public void ChangeAngle() //for mode
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
    }


    public void ChangeView() //for postprocessing or not
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

    }

    public void ChangeMode() // for 5 line or not
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
    }

    public void ChangeCharacterSpeed()
    {
        float spd = float.Parse(charSpeed.text);

        if(spd < 1 || spd > 3)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Character speed must be between 1.0 and 3.0";
        }
        else
        {
            errorText.gameObject.SetActive(false);
            Data.charSpeed = float.Parse(charSpeed.text);
            Platform.instance.runner.GetComponent<Runner>().CharacterSpeed = Data.charSpeed;
            Debug.Log(Data.charSpeed);
        }
    }

    public void ChangeMonsterSpeed()
    {
        float spd = float.Parse(monsSpeed.text);

        if (spd < 0.5 || spd > 1.5)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Monster speed must be between 0.5 and 1.5";
        }
        else
        {
            errorText.gameObject.SetActive(false);
            Data.monsSpeed = float.Parse(monsSpeed.text);
            Platform.instance.Nightmare.GetComponent<BadThingParticleSystem>().monsterSpeed = Data.monsSpeed;
            Debug.Log(Data.monsSpeed);
        }
    }

}
