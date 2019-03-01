using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sadece playeri hareket ettiriyor zamanla hızı artırıyor

public class Runner : MonoBehaviour
{

    private float speed;
   // private float timer;
   // private float gameTime;

    //private Vector3 toPos;

   // private bool isBoost;

    private SpriteRenderer sprite;

    public float CharacterSpeed;

   // private float boostCounter;
   // private float boostLimit;

    void Start()
    {
        //gameTime = 0f;
        //timer = 4f;
        //toPos = Vector3.zero;
       // isBoost = false;
        sprite = this.GetComponent<SpriteRenderer>();


      //  boostLimit = 5f;
      //  boostCounter = 0f;
        // this.transform.position = Platform.instance.platfotmTiles[5].transform.position; // start from 3rd tile // platform da yapılıyor ki aradaki fark hemen hesaplanabilsin
    }

    void Update()
    {
        if (Platform.instance.game.state == GameHandler.GameState.GameRunning)
        {
            speed = Platform.instance.straightRoadLenght * CharacterSpeed; 

            if (Platform.instance.straightRoadLenght >= 1f)// && !Mathf.Approximately(platform.transform.GetChild(platform.GetComponent<Platform>().blockToSlide).position.y,0))
            {
                this.transform.Translate(0f, speed * Time.deltaTime, 0f, Space.World);
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