using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    private int levelofButton;

    //private GameObject scrollViewObj;
    private int maxLevel;
    //private Button thisButton;


    void Start()
    {
        maxLevel = Data.GetMaxLevel();

        //PlayerPrefs de level ve maxlevel 0 dan başlayarak tutuluyor. Bundan dolayı Text te yazan değerin 1 eksiğini alınıyor.
        levelofButton = int.Parse(gameObject.GetComponentInChildren<Text>().text) - 1; 

        if (maxLevel < levelofButton)
        {
            //gameObject.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<Image>().color = Color.HSVToRGB(0,0,50);
        }
    }

    public void SelectLevel()
    {
        Data.ChangeLevel(levelofButton);
        Platform.instance.CreatePlatformAccordingToLevel();
        Debug.Log("Level changed to " + (levelofButton + 1));
        this.transform.parent.gameObject.SetActive(false);
    }
}

