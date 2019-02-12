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

    public GameObject block; //kırmızı bloklar
    public GameObject runner; //koşan arkadaş artık neyse
    public GameObject lines;
	public GameObject road;//düz yol 
	//public GameObject background; //rengi değişen bok

    public List<GameObject> platfotmTiles; //blokları barındıran liste

    private float distance; //bi sonraki bloğun gelceği y mesafesi.  habire artıyor
    public float distBetweenBlock; //bloklar arası x mesafesi

    public float[] BlockPos; // blokların oluşailceği pozisyonlar

    private int blockNum; // kaç tane blok olcağı

    public int blockToSlide; // o sırada kaydırılcak blok

    private int exRand = 3;
    private int sameLine = 0;

    private int point;
    public int gainedPoint;

    public int pushBlockForward; // sıranın en sonuna atılcak blok. en arkada kalan blok

    public float straightRoadLenght;
    public float initialStraightRoadLenght; // for refence point of starting size

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

        BlockPos = new float[] {/*-2 * distBetweenBlock,*/-1 * distBetweenBlock, distBetweenBlock/*,2 * distBetweenBlock*/};
        distance = -5f; // Start from -5

        int levelStartStraightLine = 7; // start from third block to give full road

        platfotmTiles[platfotmTiles.Count - 1].transform.position = new Vector2(0f, distance);


        for (int i = 0; i < levelStartStraightLine; i++)
        {
            distance += distBetweenBlock;
            platfotmTiles.Add((GameObject)Instantiate(block, this.transform));
            platfotmTiles[platfotmTiles.Count - 1].transform.position = new Vector2(0f, distance);

        }

        blockNum = 20;

        for (int i = 0; i < blockNum; i++)
        {
            platfotmTiles.Add((GameObject)Instantiate(block, this.transform));
            platfotmTiles[platfotmTiles.Count - 1].transform.position = BlockPositioner(distBetweenBlock);
        }

       /* if (!Data.isAngled)
        {
            foreach (GameObject g in platfotmTiles)
            {
                g.transform.GetChild(0).gameObject.SetActive(false);
            }
        }*/

        runner.transform.position = instance.platfotmTiles[4].transform.position; //Runner starts from 4rd tile

        blockToSlide = levelStartStraightLine + 1;
        pushBlockForward = 0;
        initialStraightRoadLenght = platfotmTiles[blockToSlide].transform.position.y - runner.transform.position.y; // camera ve kombo için uzaklık hesapla
        straightRoadLenght = initialStraightRoadLenght; // camera ve kombo için uzaklık hesapla

        Debug.Log("Initial length is : " + initialStraightRoadLenght);

        point = 0;
        gainedPoint = 1;
    }

    //runner bloktan öndeyse bloğu ileri at + lines ı bir ileri taşı
    private void LateUpdate()
    {
        if(runner.transform.position.y >= platfotmTiles[pushBlockForward].transform.position.y + (10 * distBetweenBlock))
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

            straightRoadLenght = platfotmTiles[blockToSlide].transform.position.y - runner.transform.position.y; // camera ve kombo için uzaklık hesapla

            //Debug.Log(straightRoadLenght);

            /*if (Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, 0f)) //kaycak bloğa karar veriyor. MoveTile de kayar kaymaz yapılıyor artık
            {
                blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
            }*/
        }
    }



    private void MoveTile()
    {
        if (platfotmTiles[blockToSlide].GetComponent<Block>().type == BlockData.blockType.normal)
        {
            if (ınput.dirr == InputManager.direction.right)
            {
                if (platfotmTiles[blockToSlide].transform.position.x > 0 )//Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, BlockPos[1])) // if pressed right and next tile is on right
                {
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(0));
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
                    point += gainedPoint;
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
                if (platfotmTiles[blockToSlide].transform.position.x < 0 )//Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, BlockPos[0])) // if pressed left and tile is on left
                {
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(0));
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
                    point += gainedPoint;
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
                if (platfotmTiles[blockToSlide].transform.position.x < 0 )//Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, BlockPos[0])) // if pressed right tile is on left
                {
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(0));
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
                    point += gainedPoint;
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
            else if (ınput.dirr == InputManager.direction.left) 
            {
                if (platfotmTiles[blockToSlide].transform.position.x > 0)//Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, BlockPos[1])) // if pressed left tile is on right
                {
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(0));
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
                    point += gainedPoint;
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
        exRand = RandomPos.RandomPosition(exRand, sameLine,BlockPos.Length);
        sameLine = (tempEx == exRand) ? sameLine += 1 : sameLine = 0;
        distance += rate;

        return new Vector2(BlockPos[exRand], distance);
    }

    public void ChangeMode() //for mode //new Vector3(0f,0f,-10f)new Vector3(0f, -6f, -10f)
    {
        if(!Data.isAngled)
        {
            foreach(GameObject g in platfotmTiles)
            {
            g.transform.GetChild(0).gameObject.SetActive(false);
            }
            Camera.main.gameObject.transform.eulerAngles = Vector3.zero;
            Camera.main.gameObject.GetComponent<CameraMovement>().CalculateOffset(runner.transform.position + new Vector3(0f, 3f, -10f));
            //Camera.main.gameObject.transform.position = Vector3.zero;
        }
        else
        {
            foreach (GameObject g in platfotmTiles)
            {
                g.transform.GetChild(0).gameObject.SetActive(true);
            }
            Camera.main.gameObject.transform.eulerAngles = new Vector3(-30f, 0f, 0f);
            Camera.main.gameObject.GetComponent<CameraMovement>().CalculateOffset(runner.transform.position + new Vector3(0f, -3f, -10f));
            //Camera.main.gameObject.transform.position = new Vector3(0f, 6f, 0f);
        }

    }


    public void GiveMessage(float time,string message)
    {
        StartCoroutine(uI.GiveInfo(time,message));
    }

}
