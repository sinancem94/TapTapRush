using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

    Text point;
    GameObject StartingPanel;
    GameObject EndingPanel;
    //called from starting button.


    public void SetPoint(int pnt)
    {
        point.text = pnt.ToString();
    }

    public void StartGame()
    {
        Platform.instance.game.StartGame();
        StartingPanel = this.transform.GetChild(0).gameObject;
        StartingPanel.SetActive(false);
        EndingPanel = this.transform.GetChild(1).gameObject;
        EndingPanel.SetActive(false);

        point = this.transform.GetChild(2).GetComponent<Text>();
    }

    //called from platform.gamehandler and runner
    public void GameOver()
    {
        Debug.Log("game over");
        EndingPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("RunHelper",LoadSceneMode.Single);
    }
	
}
