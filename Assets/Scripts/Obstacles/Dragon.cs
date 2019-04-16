using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Obstacle
{
    public float shootInterval;
    private int fireBallCount;

    private GameObject fireBall;
    public List<GameObject> FireBalls;

    public override void SetParameters() 
    {
        shootInterval = 1.5f;
        fireBallCount = 3;
        fireBall = Resources.Load<GameObject>("Prefabs/FireBall");

        FireBalls = new List<GameObject>(fireBallCount);

        for (int i = 0; i < FireBalls.Capacity; i++)
        {
            FireBalls.Add(Instantiate(fireBall,transform.parent));
            FireBalls[i].GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder - 1;
            FireBalls[i].SetActive(false);
        }
    }


    public override IEnumerator ObstacleLoop()
    {
        bool shoot = true;

        while (shoot)
        {
            for (int i = 0; i < FireBalls.Capacity; i++)
            {
                if (!FireBalls[i].activeInHierarchy)
                {
                    FireBalls[i].transform.position = transform.position;
                    FireBalls[i].SetActive(true);
                    //PostProcessingChange.instance.Glow(0.07f);
                }
                yield return new WaitForSeconds(shootInterval);
            }

            if (Platform.instance.game.GetGameState() != GameHandler.GameState.GameRunning)
                shoot = false;
        }

        StopCoroutine(ObstacleLoop());
    }
}
