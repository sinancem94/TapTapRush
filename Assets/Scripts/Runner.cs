using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sadece playeri hareket ettiriyor zamanla hızı artırıyor

public class Runner : MonoBehaviour {
    
    public float speed;
    private float timer;
    private float gameTime;

    private Vector3 toPos;

    public bool isStrike;

    private SpriteRenderer sprite;

    void Start () {
        gameTime = 0f;
        timer = 4f;
        toPos = Vector3.zero;
        //speed = 2f;
        isStrike = false;
        sprite = this.GetComponent<SpriteRenderer>();
       // this.transform.position = Platform.instance.platfotmTiles[5].transform.position; // start from 3rd tile // platform da yapılıyor ki aradaki fark hemen hesaplanabilsin
	}
	
	void Update () 
    {
        if(Platform.instance.game.state == GameHandler.GameState.GameRunning)
        {
            if(Platform.instance.straightRoadLenght >= Platform.instance.distBetweenBlock * 5 && !isStrike) //kombo var mı hesapla
            {
                isStrike = true;
                toPos = Platform.instance.platfotmTiles[Platform.instance.blockToSlide].transform.position;
                Platform.instance.gainedPoint += 1;
                Platform.instance.GiveMessage(1f, "Speed Up!!");
                Debug.Log("STRİKE!!");
            }

            //speed = Platform.instance.straightRoadLenght; 

            if (Platform.instance.straightRoadLenght >= 1f && !isStrike)// && !Mathf.Approximately(platform.transform.GetChild(platform.GetComponent<Platform>().blockToSlide).position.y,0))
            {
                this.transform.Translate(0f, speed * Time.deltaTime, 0f, Space.World);
            }
            else if(isStrike) // kombo varsa hızlan, zorlaştır
            {
                
                if(this.transform.position.y < toPos.y) 
                {
                    this.transform.Translate(0f,5  * Time.deltaTime, 0f, Space.World);
                }
                else
                {
                    isStrike = false;
                    //sprite.material.SetColor("_OutlineColor", Color.white);
                    Platform.instance.gainedPoint -= 1;
                    Platform.instance.GiveMessage(1f, "Speed Down");
                }
                    
                
            }
            //TODO: Add a speed changer to runner according to players tapping speed
          /*  gameTime += Time.deltaTime;
            if (gameTime > timer)
            {
                speed += .3f;
                timer += 4f;
            }*/
        }
	}
}
