using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    private bool trackSwipe = true;

    public direction dirr = direction.none;
    public Queue<direction> directions = new Queue<direction>(); // ınputs are holded in a que

    public enum direction
    {
        right,
        left,
        none
    }

    public void directionGetter()
    {
        direction dir = direction.none;
#if UNITY_IOS && !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            Touch playerFinger = Input.touches[0];

            if (playerFinger.phase == TouchPhase.Began && trackSwipe)
            {
                trackSwipe = false;

                if(playerFinger.position.x < Screen.width / 2){
                    
                    dir = direction.left;
                    directions.Enqueue(dir);
                    Debug.Log(directions);
                }
                else if(playerFinger.position.x > Screen.width/2){
                    dir = direction.right;
                    directions.Enqueue(dir);
                    Debug.Log(directions);
                }
                trackSwipe = true;
             }
         }
            
#elif UNITY_EDITOR 
        if (Input.GetKeyDown("right"))
        {
            dir = direction.right;
            directions.Enqueue(dir);
        }
        else if (Input.GetKeyDown("left"))
        {
            dir = direction.left;
            directions.Enqueue(dir);
        }
        else
        {
            dir = direction.none;
        }
#endif
    }
}

