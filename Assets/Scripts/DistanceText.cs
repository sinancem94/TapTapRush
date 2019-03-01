using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceText : MonoBehaviour
{
    Text distanceText;
    
    void Start()
    {
        distanceText = GetComponent<Text>();
    }

    void LateUpdate()
    {
        distanceText.text = ((float)Platform.instance.distanceBtwRunner).ToString("0.0");
    }
}
