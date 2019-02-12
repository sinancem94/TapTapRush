using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeHandler 
{
    //Arrange platform width convert screen width to world size in x
    public float gameScreenHeight = Camera.main.orthographicSize * 2.0f;
    public float gameScreenWidth = (Camera.main.orthographicSize * 2.0f) * Camera.main.aspect; //gameScreenHeight
}
