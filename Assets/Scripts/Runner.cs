using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sadece playeri hareket ettiriyor zamanla hızı artırıyor

public class Runner : MonoBehaviour {
    
    public float speed;
    private float timer;
    private float gameTime;

    void Start () {
        gameTime = 0f;
        timer = 4f;
        speed = 4f;
        this.transform.position = Platform.instance.platfotmTiles[2].transform.position; // start from 3rd tile
	}
	
	void Update () 
    {
        if(Platform.instance.game.state == GameHandler.GameState.GameRunning)
        {
            if (this.transform.position.y <= Platform.instance.platfotmTiles[Platform.instance.blockToSlide].transform.position.y - 1f)// && !Mathf.Approximately(platform.transform.GetChild(platform.GetComponent<Platform>().blockToSlide).position.y,0))
            {
                this.transform.Translate(0f, speed * Time.deltaTime, 0f, Space.World);
            }
            //TODO: Add a speed changer to runner according to players tapping speed
            gameTime += Time.deltaTime;
            if (gameTime > timer)
            {
                speed += .5f;
                timer += 4f;
            }
        }
	}
}
