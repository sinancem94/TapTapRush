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
    public Animator animator;

    public float CharacterSpeed;

   // private float boostCounter;
   // private float boostLimit;

    void Start()
    {
        //gameTime = 0f;
        //timer = 4f;
        //toPos = Vector3.zero;
       // isBoost = false;

        animator = this.GetComponent<Animator>();

        //CharacterSpeed = Data.charSpeed;

        //  boostLimit = 5f;
        //  boostCounter = 0f;
        //this.transform.position = Platform.instance.platfotmTiles[5].transform.position; // start from 3rd tile // platform da yapılıyor ki aradaki fark hemen hesaplanabilsin
    }

    void Update()
    {
        if (Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning)
        {
            if (Platform.instance.straightRoadLenght >= 1f )// && !Mathf.Approximately(platform.transform.GetChild(platform.GetComponent<Platform>().blockToSlide).position.y,0))
            {
                speed = Platform.instance.straightRoadLenght * CharacterSpeed;              //burası if statementın dışındaydı aşağı aldım boosttan sonra bore kosmaya baslayabilsin diye
                this.transform.Translate(0f, speed * Time.deltaTime, 0f, Space.World);
				animator.SetFloat ("speed", speed);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Platform.instance.GameOver();
    }
}