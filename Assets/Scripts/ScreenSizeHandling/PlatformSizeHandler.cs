using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Arrange gameObject sizes according to devices screen size
public class PlatformSizeHandler : SizeHandler{
	
    public float ArrangeSize(Transform road,Transform lines,Transform block,Transform runner) 
    {
        //Arrange platform width convert screen width to world size in x
        //float height = Camera.main.orthographicSize * 2.0f;
        //float width = height * Camera.main.aspect;

        //Arrange road scale according to screen
        float roadWidth = gameScreenWidth / (float)(5f/3f);
        //Debug.Log(width + " " + roadWidth);
        road.localScale = new Vector3(roadWidth, 20f, 1f);

        //Arrange line positions according to roads size
        float linePosition = (/*Platform.instance.*/road.transform.localScale.x / 2);
        lines.GetChild(0).localPosition = new Vector3(linePosition, 0f, 0f);
        lines.GetChild(1).localPosition = new Vector3(linePosition * -1,0f,0f);

        //Arrange block scale according to roads scale. 1.8f for 3 lines , 3f for 5 lines
        float blockScale = roadWidth / 1.8f; // 3f;//1.8f; 
        block.localScale = new Vector3(blockScale,blockScale,1f);

        //Arrange runner size
        runner.localScale = new Vector3(blockScale * 2 / 4, blockScale * 2 / 4, 1f);
        //return the distance between blocks. 3f for 3 lines, 5f for 5 lines
        return roadWidth / 3f;//5f;//3f;
	}
}
