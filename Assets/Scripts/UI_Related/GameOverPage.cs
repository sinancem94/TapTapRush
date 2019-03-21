using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPage : MonoBehaviour
{
    private Button PlayButton;
    private Text GameEndedMessage;

    void OnEnable()
    {
        PlayButton = GameObject.FindWithTag("StartGameButton").GetComponent<Button>();
        GameEndedMessage = GameObject.FindWithTag("GameEndedMessage").GetComponent<Text>();
    }

    public void SetPanel() 
    {
        if (Platform.instance.game.GetGameState() == GameHandler.GameState.LevelPassed)
        {
            GameEndedMessage.text = "LEVEL PASSED!";
            PlayButton.GetComponentInChildren<Text>().text = "NEXT LEVEL";
        }
        else if (Platform.instance.game.GetGameState() == GameHandler.GameState.GameOver)
        {
            GameEndedMessage.text = "YOU DIED";
            GameEndedMessage.color = Color.red;
            PlayButton.GetComponentInChildren<Text>().text = "TRY AGAIN";
        }
    }
}
