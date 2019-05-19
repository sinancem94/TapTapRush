using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Arrange gameObject sizes according to devices screen size
public class PlatformSizeHandler : SizeHandler{
	
    public float ArrangeSize(Transform road,ref Vector2 blockScale,Transform runner, bool is5Line) 
    {
        //Arrange road scale according to screen
        float roadWidth = gameScreenWidth / (float)(5f/3f);
        float roadLength = roadWidth * 15;

        road.localScale = new Vector3(roadWidth, roadLength, 1f);

        //Arrange block scale according to roads scale. 1.8f for 3 lines , 3f for 5 lines
        float bScale = roadWidth / 1.8f;
        if(is5Line)
        {
            bScale = roadWidth / 3f;//1.8f; 
        }
        else
        {
            bScale = roadWidth / 1.8f; 
        }

        blockScale = new Vector2(bScale, bScale);

        //Arrange runner size
        runner.localScale = new Vector3(bScale * 2 / 4, bScale * 2 / 4, 1f);
        //return the distance between blocks. 3f for 3 lines, 5f for 5 lines
        if(is5Line){
            return roadWidth / 5f;//return distance btwn blocks
        }
        else{
            return roadWidth / 3f; //return distance btwn blocks
        }
	}
}
