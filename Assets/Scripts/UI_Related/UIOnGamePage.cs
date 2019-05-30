using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOnGamePage : MonoBehaviour
{
    Text infoText;
    Text point;
    Coroutine Message;
    Text distance;
    //Start is called before the first frame update
    void Start()
    {
        point = GameObject.FindWithTag("Point").GetComponent<Text>();//this.transform.GetChild(2).GetComponent<Text>();
        infoText = GameObject.FindWithTag("InfoText").GetComponent<Text>();
        distance = GameObject.FindWithTag("DistanceText").GetComponent<Text>();
        //this.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        //SetDistance()
    }

    public void SetDistance(float dist)
    {
        distance.text = dist.ToString("0");
    }

    public void SetPoint(int pnt)
    {
        point.text = pnt.ToString();
    }

    public void GiveMessage(float time,string msg)
    {
        Message = StartCoroutine(GiveInfo(time, msg));
    }

    IEnumerator GiveInfo(float time, string message)
    {
        infoText.text = message;
        while (infoText.color.a < 1)
        {
            infoText.color = new Color(infoText.color.r, infoText.color.g, infoText.color.b, infoText.color.a + 0.1f);
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

        StopCoroutine(Message);
    }
}