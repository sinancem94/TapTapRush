 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

    Text point;
    GameObject StartingPanel;
    GameObject EndingPanel;
    public Button modeButton;
    Text infoText;
    //called from starting button.

    private void Start()
    {
        StartingPanel = GameObject.FindWithTag("StartingPanel"); //this.transform.GetChild(0).gameObject;
        EndingPanel = GameObject.FindWithTag("GameOverPanel");//this.transform.GetChild(1).gameObject;
        EndingPanel.SetActive(false);
        point = GameObject.FindWithTag("Point").GetComponent<Text>();//this.transform.GetChild(2).GetComponent<Text>();
        infoText = GameObject.FindWithTag("InfoText").GetComponent<Text>();
    }

    public void SetPoint(int pnt)
    {
        point.text = pnt.ToString();
    }


    public IEnumerator GiveInfo(float time,string message)
    {
        infoText.text = message;
        while(infoText.color.a < 1)
        {
            infoText.color = new Color(infoText.color.r,infoText.color.g,infoText.color.b,infoText.color.a + 0.1f);
            yield return new WaitForSeconds(.01f);
        }
        infoText.color = new Color(infoText.color.r, infoText.color.g, infoText.color.b, 1);
        yield return new WaitForSeconds(time);
        while (infoText.color.a > 0)
        {
            infoText.color = new Color(infoText.color.r, infoText.color.g, infoText.color.b, infoText.color.a - 0.1f);
            yield return new WaitForSeconds(.01f);
        }
        infoText.color = new Color(infoText.color.r, infoText.color.g, infoText.color.b, 0);

        StopCoroutine(GiveInfo(time, message));
    }

    public void StartGame()
    {
        Platform.instance.game.StartGame();
        StartingPanel.SetActive(false);
    }

    //called from platform.gamehandler and runner
    public void GameOver()
    {
        Debug.Log("game over");
        EndingPanel.SetActive(true);
    }

    public void Restart()
    {
        Data.isAngled = false;//for mode change kaldırılcak
        SceneManager.LoadScene("RunHelper",LoadSceneMode.Single);
    }
	
    public void ChangeMode() //for mode
    {
        if(!Data.isAngled)
        {
            Data.isAngled = true;
            modeButton.GetComponentInChildren<Text>().text = "açılı";
            Platform.instance.ChangeMode();
        }
        else
        {
            Data.isAngled = false;
            modeButton.GetComponentInChildren<Text>().text = "açısız";
            Platform.instance.ChangeMode();
        }

    }
}
