using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerfectTextScript : MonoBehaviour
{
    private Text perfectText;
    private Color dummyColor;

    Quaternion rotation;


    private void Awake()
    {
        rotation = transform.rotation;
        perfectText = gameObject.GetComponent<Text>();
    }
    
    
    private void OnEnable()
    {
        StartCoroutine(perfectTextFadeandRise());
    }

    private void LateUpdate()
    {
        transform.rotation = rotation;
    }

    IEnumerator perfectTextFadeandRise()
    {
        for(int a = 100; a > 0; a= a - 10)
        {
            perfectText.CrossFadeAlpha(0,2.0f,false);
            perfectText.rectTransform.transform.Translate(0,0.3f,0);
            yield return new WaitForSeconds(0.07f);
        }
    }
}
