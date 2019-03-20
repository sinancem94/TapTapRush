using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Manages change between UI panels
public class UIHandler : MonoBehaviour {


    GameObject StartingPanel;
    GameObject GameOverPanel;
    GameObject OnGamePanel;

    UIGameHandler uIGameHandler;
    GameOverPage gameOverPage;
    //TrialButtons trialButtons;

    private void Start()
    {
        StartingPanel = GameObject.FindWithTag("StartingPanel"); //this.transform.GetChild(0).gameObject;
        StartingPanel.SetActive(false);
        GameOverPanel = GameObject.FindWithTag("GameOverPanel");//this.transform.GetChild(1).gameObject;
        GameOverPanel.SetActive(false);
        OnGamePanel = GameObject.FindWithTag("OnGamePanel");
        OnGamePanel.SetActive(false);

        //trialButtons = GetComponent<TrialButtons>();
        //trialButtons.enabled = false;
        uIGameHandler = OnGamePanel.GetComponent<UIGameHandler>();
        gameOverPage = GameOverPanel.GetComponent<GameOverPage>();
    }

    public UIGameHandler GetGamePanel()
    {
        return uIGameHandler;
    }

    //Opens Starting Panel
    //After platform setted game objects and parameters calls this method 
    public void OpenUIPanel()
    {
        StartingPanel.SetActive(true);
        //trialButtons.enabled = true;
    }

    //called from starting button.
    public void StartGame()
    {
        StartingPanel.SetActive(false);
        OnGamePanel.SetActive(true);
        Platform.instance.game.StartGame();
        //infoText.gameObject.SetActive(true);
    }

    //called from platform
    public void GameOver()
    {
        Debug.Log("Game over");
        OnGamePanel.SetActive(false);

        gameOverPage.SetPanel();
        GameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        //Data.isAngled = false;//for mode change kaldırılcak
        SceneManager.LoadScene("RunHelper");
    }
	
}
