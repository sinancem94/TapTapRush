using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLevelSelectionButtons : MonoBehaviour
{
    private GameObject levelSelectionButton;
    private List<GameObject> levelButtons;

    private int LevelCount;

    void Start()
    {
        Rect panelRect = this.GetComponent<RectTransform>().rect;
        panelRect.center = Vector2.zero;
        panelRect.height = Screen.height;
        panelRect.width = Screen.width;

        ///////////////
        //-----Arrange scale of buttons according to screen size
        GridLayoutGroup levels = GetComponent<GridLayoutGroup>();
        levels.cellSize = new Vector2((Screen.width * 4) / 30, (Screen.width * 4) / 30);
        levels.spacing = new Vector2(Screen.width / 15, Screen.width / 15);
        ///////////////

        LevelCount = 12;
        levelSelectionButton = transform.GetChild(0).gameObject;
        levelButtons = new List<GameObject>(LevelCount);
        levelButtons.Add(levelSelectionButton);

        for(int i = 1; i < LevelCount; i++) 
        {
            levelButtons.Add(Instantiate(levelSelectionButton, transform));

            if (levelButtons[i].GetComponentInChildren<Text>())
                levelButtons[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
            else
                Debug.LogError("ERROR when arranging level buttons. Button " + (i + 1) + " does not have a text component in child");
        }

    }


}
