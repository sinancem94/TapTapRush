using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sadece playeri hareket ettiriyor zamanla hızı artırıyor

public class Runner : MonoBehaviour
{
    public RunnerAnimation BoreAnimationController; //Used by runner for setting speed of bore animation. And by boost script to play boost animations.
    private float speed;

    public float CharacterSpeed;

    void Start()
    {
        BoreAnimationController = new RunnerAnimation(this);
    }

    void Update()
    {
        if (Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning)
        {
            speed = Platform.instance.straightRoadLenght * CharacterSpeed;              //burası if statementın dışındaydı aşağı aldım boosttan sonra bore kosmaya baslayabilsin diye
            this.transform.Translate(0f, speed * Time.deltaTime, 0f, Space.World);

            BoreAnimationController.AnimationSpeed(speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Platform.instance.GameOver();
    }
}