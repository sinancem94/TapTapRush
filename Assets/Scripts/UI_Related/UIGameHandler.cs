using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameHandler : MonoBehaviour
{
    Text infoText;
    Text point;
    //Start is called before the first frame update
    void Start()
    {
        point = GameObject.FindWithTag("Point").GetComponent<Text>();//this.transform.GetChild(2).GetComponent<Text>();
        infoText = GameObject.FindWithTag("InfoText").GetComponent<Text>();
        //this.gameObject.SetActive(false);
    }

    public void SetPoint(int pnt)
    {
        point.text = pnt.ToString();
    }

    public IEnumerator GiveInfo(float time, string message)
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

        StopCoroutine(GiveInfo(time, message));
    }
}