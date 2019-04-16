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
    UIHomePage uIHomePage;
    //TrialButtons trialButtons;

    //Attached to Canvas. 
    //Oyunun her aşamasındaki paneller arası geçiş yapmak kullanılıyor. Paneller in kendilerine ait scriptleri var
    //Her bir panel, canvas ın child ı. Ondan sırayla al
    //StartingPanel 0
    //GameOverPanel 1
    //OnGamelPanel//

    private void Start()
    {

        if (transform.GetChild(0))
        {
            StartingPanel = transform.GetChild(0).gameObject;
            StartingPanel.SetActive(false);
            uIHomePage = StartingPanel.GetComponent<UIHomePage>();
        }
        else
        {
            Debug.LogError("Cant find starting panel");
        }

        if (transform.GetChild(1))
        {
            GameOverPanel = transform.GetChild(1).gameObject;
            GameOverPanel.SetActive(false);
            gameOverPage = GameOverPanel.GetComponent<GameOverPage>();
        }
        else
        {
            Debug.LogError("Cant find starting panel");
        }

        if (transform.GetChild(2))
        {
            OnGamePanel = transform.GetChild(2).gameObject;
            OnGamePanel.SetActive(false);
            uIGameHandler = OnGamePanel.GetComponent<UIGameHandler>();
        }
        else
        {
            Debug.LogError("Cant find starting panel");
        }

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

        GameOverPanel.SetActive(true);
        gameOverPage.SetPanel();
    }

    public void Restart()
    {
        //Data.isAngled = false;//for mode change kaldırılcak
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name,LoadSceneMode.Single);
    }
	
}
