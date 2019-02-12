using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inputa sahip yani gelen inputları o alıyor ve oyunu yönetiyor, GameHandler a sahip yani game state i yönetiyor
/*
Blokları yaratıyor, konumlandırıyor input alarak kaydırıp yanlış input gelirse oyunu sonlandırıyor
*/

public class Platform : MonoBehaviour
{
	

    private InputManager ınput;
    public GameHandler game;
    private PlatformSizeHandler platformSizeHandler;
    private UIHandler uI;
	private ExplosionParticleSystem explosionParticleSystem;

    public GameObject block; //kırmızı bloklar
    public GameObject runner; //koşan arkadaş artık neyse
    public GameObject lines;
	public GameObject road;//düz yol 
	//public GameObject background; //rengi değişen bok

    public List<GameObject> platfotmTiles; //blokları barındıran liste

    private float distance; //bi sonraki bloğun gelceği y mesafesi
    private float distBetweenBlock;

    public float[] BlockPos;

    private int blockNum; // kaç tane blok olcağı

    public int blockToSlide; // o sırada kaydırılcak blok

    private int exRand = 3;
    private int sameLine = 0;

    private int point;

    public int pushBlockForward; // sıranın en sonuna atılcak blok

    public static Platform instance;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        ınput = new InputManager();

        game = new GameHandler(GameHandler.GameState.BeginingPage);

        platformSizeHandler = new PlatformSizeHandler();

        uI = (UIHandler)FindObjectOfType(typeof(UIHandler));

		explosionParticleSystem = (ExplosionParticleSystem)FindObjectOfType (typeof(ExplosionParticleSystem));

		block = GameObject.FindWithTag("Block");
        runner = GameObject.FindWithTag("Runner");
		lines = GameObject.FindWithTag("Lines");
		road = GameObject.FindWithTag("Road");
		//background = GameObject.FindWithTag("Background");

        distBetweenBlock = platformSizeHandler.ArrangeSize(road.transform,lines.transform,block.transform,runner.transform);

        lines.transform.position = new Vector2(0f, runner.transform.position.y + (5 * distBetweenBlock));
        road.transform.position = new Vector2(0f, runner.transform.position.y + (5 * distBetweenBlock));
		//background.transform.position = new Vector2(0f, runner.transform.position.y + 5);

        platfotmTiles = new List<GameObject>();
        platfotmTiles.Add(block);

        BlockPos = new float[] {-distBetweenBlock,distBetweenBlock};
        distance = -5f; // Start from -5

        int levelStartStraightLine = 7; // start from third block to give full road

        platfotmTiles[platfotmTiles.Count - 1].transform.position = new Vector2(0f, distance);


        for (int i = 0; i < levelStartStraightLine;i++)
        {
            distance += distBetweenBlock;
            platfotmTiles.Add((GameObject)Instantiate(block, this.transform));
            platfotmTiles[platfotmTiles.Count - 1].transform.position = new Vector2(0f, distance);

        }

        blockNum = 10;

        for (int i = 0; i < blockNum; i++)
        {
            platfotmTiles.Add((GameObject)Instantiate(block, this.transform));
            platfotmTiles[platfotmTiles.Count - 1].transform.position = BlockPositioner(distBetweenBlock);
        }
        blockToSlide = 0;
        pushBlockForward = 0;

        point = 0;
    }

    //runner bloktan öndeyse bloğu ileri at + lines ı bir ileri taşı
    private void LateUpdate()
    {
        if(runner.transform.position.y >= platfotmTiles[pushBlockForward].transform.position.y + (3 * distBetweenBlock))
        {
            lines.transform.position = new Vector2(0f, runner.transform.position.y + 3);
			road.transform.position = new Vector2(0f, runner.transform.position.y + 3);
			//background.transform.position = new Vector2(0f, runner.transform.position.y + 3);

            platfotmTiles[pushBlockForward].transform.position = BlockPositioner(distBetweenBlock);

            platfotmTiles[pushBlockForward].GetComponent<Block>().SetBlock();

            pushBlockForward = (pushBlockForward + 1 < platfotmTiles.Count) ? pushBlockForward += 1 : pushBlockForward = 0;
        }
    }

    // kaycak bloğa karar ver, input al, input varsa ona göre haraket et
    private void Update()
    {
        if(game.state == GameHandler.GameState.GameRunning)
        {
            ınput.directionGetter();

            if (ınput.directions.Count != 0)
            {
                ınput.dirr = ınput.directions.Dequeue();
                MoveTile();
            }

            if (Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, 0f))
            {
                blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
            }
        }
    }


    private void MoveTile()
    {
        if (platfotmTiles[blockToSlide].GetComponent<Block>().type == BlockData.blockType.normal)
        {
            if (ınput.dirr == InputManager.direction.right)
            {
                if (Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, BlockPos[1])) // if pressed right and next tile is on right
                {
					explosionParticleSystem.Explode (platfotmTiles[blockToSlide].transform.position);// xplosion
					platfotmTiles [blockToSlide].gameObject.SetActive (false); // blow up the block

                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(0));
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
                    point++;
                    uI.SetPoint(point);


                    //platfotmTiles[blockToSlide].transform.position = new Vector2(platfotmTiles[blockToSlide].transform.position.x - distBetweenBlock, platfotmTiles[blockToSlide].transform.position.y);
                }
                else // if pressed Right but tile is on left
                {
                    game.GameOver();
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().Fall(new Vector2(-1f, 0)));

                    uI.GameOver();
                }
            }
            else if (ınput.dirr == InputManager.direction.left)
            {
                if (Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, BlockPos[0])) // if pressed left and tile is on left
                {
					explosionParticleSystem.Explode (platfotmTiles[blockToSlide].transform.position); //xplosion
					platfotmTiles [blockToSlide ].gameObject.SetActive (false); // blow up the block

                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(0));
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
                    point++;
                    uI.SetPoint(point);


                    //platfotmTiles[blockToSlide].transform.position = new Vector2(platfotmTiles[blockToSlide].transform.position.x + distBetweenBlock, platfotmTiles[blockToSlide].transform.position.y);
                }
                else // if pressed left but tile is on right
                {
                    game.GameOver();
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().Fall(new Vector2(1f, 0)));
                   
                    uI.GameOver();
                }
            }
        }
        else // if block is reverse 
        {
            if (ınput.dirr == InputManager.direction.right)
            {
                if (Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, BlockPos[0])) // if pressed right tile is on left
                {
					explosionParticleSystem.Explode (platfotmTiles[blockToSlide].transform.position); //xplosion
					platfotmTiles [blockToSlide].gameObject.SetActive (false); // blow up the block

                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(0));
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
                    point++;
                    uI.SetPoint(point);


                    //platfotmTiles[blockToSlide].transform.position = new Vector2(platfotmTiles[blockToSlide].transform.position.x + distBetweenBlock, platfotmTiles[blockToSlide].transform.position.y);
                }
                else // if pressed right but reverse worng
                {
                    game.GameOver();
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().Fall(new Vector2(1f, 0)));
                   
                    uI.GameOver();
                }
            }
            else if (ınput.dirr == InputManager.direction.left) // if pressed left tile is on right
            {
                if (Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, BlockPos[1]))
                {
					explosionParticleSystem.Explode (platfotmTiles[blockToSlide].transform.position);// xplosion
					platfotmTiles [blockToSlide].gameObject.SetActive (false); // blow up the block

                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(0));
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
                    point++;
                    uI.SetPoint(point);


                    //platfotmTiles[blockToSlide].transform.position = new Vector2(platfotmTiles[blockToSlide].transform.position.x - distBetweenBlock, platfotmTiles[blockToSlide].transform.position.y);
                }
                else // if pressed left but reverse worng
                {
                    game.GameOver();
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().Fall(new Vector2(-1f, 0)));

                    uI.GameOver();
                }
            }
        }

    }

    // blokları konumlandıran fonksiyon
    private Vector2 BlockPositioner(float rate)
    {
        int tempEx = exRand;
        exRand = RandomPos.RandomPosition(exRand, sameLine);
        sameLine = (tempEx == exRand) ? sameLine += 1 : sameLine = 0;
        distance += rate;

        return new Vector2(BlockPos[exRand], distance);
    }

}
