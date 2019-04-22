using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Used by obstacles in game. Dragon,blades etc. 
//Starting and disabling handled in this script.

//Each obstacle creates and sets its own parameters in Create parameters .
//Obstacles Implements loop ObstacleLoop which is different for each obstacle

public abstract class Obstacle : MonoBehaviour
{
    public bool EnteredStart;
    public bool isObstacleLooping;

    private Coroutine loop;

    private SpriteRenderer s_renderer;

    void Start()
    {
        EnteredStart = true;
        isObstacleLooping = false;
        s_renderer = GetComponent<SpriteRenderer>();

        //Debug.Log("Starta girdi : " + Time.unscaledTime);

        SetParameters();
    }

    private void LateUpdate()
    {

        if (!isObstacleLooping && s_renderer.isVisible && Platform.instance.GetBoostPhase() == BoostScript.BoostPhase.None)
        {
            Debug.Log("Started shooting fireball");
            isObstacleLooping = true;
            loop = StartCoroutine(ObstacleLoop());
        }
        else if ((isObstacleLooping && !s_renderer.isVisible) || (isObstacleLooping && Platform.instance.GetBoostPhase() != BoostScript.BoostPhase.None))
        {
            Debug.Log("Stopped shooting fireball");
            isObstacleLooping = false;
            StopCoroutine(loop);
            this.gameObject.SetActive(false);
        }
    }

    /*private void OnEnable()
    {
        Debug.Log("OnEnable a girdi. is shooting is : " + isObstacleLooping + " is started is . : " + EnteredStart);
        if (EnteredStart && !isObstacleLooping)
        {
            Debug.Log("Started shooting fireball");
            isObstacleLooping = true;
            StartCoroutine(ObstacleLoop());
        }
    }*/

    private void OnDisable()
    {
        if (isObstacleLooping)
        {
            Debug.Log("Obstacle stopped looping");
            isObstacleLooping = false;
            if(loop != null)
                StopCoroutine(loop);
        }
    }

    public abstract void SetParameters();

    public abstract IEnumerator ObstacleLoop();
   
}
