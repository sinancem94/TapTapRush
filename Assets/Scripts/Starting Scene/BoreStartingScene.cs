using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoreStartingScene : MonoBehaviour
{
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position =    Camera.main.WorldToScreenPoint(image.transform.position) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
