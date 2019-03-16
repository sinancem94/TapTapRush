using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inputa sahip yani gelen inputları o alıyor ve oyunu yönetiyor, GameHandler a sahip yani game state i yönetiyor
/*
Blokları yaratıyor, konumlandırıyor input alarak kaydırıp yanlış input gelirse oyunu sonlandırıyor
*/

public class Platform : MonoBehaviour
{
    //Scripts
    public GameHandler game;
    private GetData GameData; 
    private InputManager ınput;
    private PlatformSizeHandler platformSizeHandler;
    private UIHandler uI;
    private UIGameHandler uIGame;
    private BoostScript Boost;
    private LevelManager levelManager;


    //GameObjects
    public GameObject block; //kırmızı bloklar
    public GameObject runner; //koşan arkadaş artık neyse
    public GameObject lines;
    public GameObject road;//düz yol 
    public GameObject background; //rengi değişen bok
    public GameObject Nightmare;

    public List<GameObject> platfotmTiles; //blokları barındıran liste

    //Parameters
    private float distance; //bi sonraki bloğun gelceği y mesafesi.  habire artıyor
    public float distBetweenBlock; //bloklar arası x mesafesi

    public float[] BlockPos; // blokların oluşailceği pozisyonlar

    private int BlockNumberInPlatformTiles; //Platform tiles listesinde kaç blok olacağı

    public int blockToSlide; // o sırada kaydırılcak blok

    public int level_p;
    public int TotalBlockInLevel;

    private int exRand = 3;
    private int sameLine = 0;

    private int point;
    public int gainedPoint;

    public float distanceBtwRunner;

    private bool boostLock; //since isBoost is changed in BoostScript, boostLock is used for locking update statement where boost started. Bir kere girsin diye

    public bool isBoost; // when boost mode activates set to true otherwise false
    private float boostTimer;
    private float boostLimit;

    public int pushBlockForward; // sıranın en sonuna atılcak blok. en arkada kalan blok

    public float straightRoadLenght;
    public float initialStraightRoadLenght; // for refence point of starting size

    public static Platform instance;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        game = new GameHandler(GameHandler.GameState.BeginingPage);

        GameData = new GetData();

        ınput = new InputManager();

        platformSizeHandler = new PlatformSizeHandler();

        level_p = GetCurrentLevel();
        levelManager = new LevelManager(level_p);

        Boost = (BoostScript)FindObjectOfType(typeof(BoostScript));
        uI = (UIHandler)FindObjectOfType(typeof(UIHandler));
        uIGame = SetUIGameHandler(); 
        //uIGame = (UIGameHandler)FindObjectOfType(typeof(UIGameHandler));

        block = GameObject.FindWithTag("Block");
        runner = GameObject.FindWithTag("Runner");
        lines = GameObject.FindWithTag("Lines");
        road = GameObject.FindWithTag("Road");
        background = GameObject.FindWithTag("Background");
        Nightmare = GameObject.FindWithTag("Nightmare");

        if (background)
            background.transform.position = new Vector3(0f, 6.5f, 0f);
        else
            Debug.LogError("Could not find GameObject Background");
        //road.transform.position = new Vector3(road.transform.position.x, road.transform.position.y + (road.transform.localScale.y / 3), 0f);
        //road.transform.position = new Vector3(road.transform.position.x, road.transform.position.y + (road.transform.localScale.y / 3), 0f);
        if (lines)
            lines.transform.position = new Vector2(0f, runner.transform.position.y + (lines.transform.GetChild(0).localScale.y / 3));//(5 * distBetweenBlock));
        else
            Debug.LogError("Could not find GameObject Background");
        
        road.transform.position = new Vector2(0f, runner.transform.position.y + (road.transform.localScale.y / 3));//(5 * distBetweenBlock));
              
        //background.transform.position = new Vector2(0f, runner.transform.position.y + 5);

        platfotmTiles = new List<GameObject>();
        //platfotmTiles.Add(block);

        point = 0;
        gainedPoint = 1;

        pushBlockForward = 0;
        BlockNumberInPlatformTiles = 30;

        boostTimer = 0f;
        boostLimit = 10f;

        levelManager.SetParametersForLevel(ref Nightmare.GetComponent<BadThingParticleSystem>().monsterSpeed);

        CreatePlatform();

        SetSpeed(); //Set player speed from playerprefs.
        SetBoost(false);

        uI.OpenUIPanel(); //After arranging everything open uı panel for starting game
    }

    //runner bloktan öndeyse bloğu ileri at + lines ı bir ileri taşı
    private void LateUpdate()
    {
        if (runner.transform.position.y >= platfotmTiles[pushBlockForward].transform.position.y + (10 * distBetweenBlock))
        {
            lines.transform.position = new Vector2(0f, runner.transform.position.y + (10 * distBetweenBlock));
            road.transform.position = new Vector2(0f, runner.transform.position.y + (10 * distBetweenBlock));
            //background.transform.position = new Vector2(0f, runner.transform.position.y + 3);

            platfotmTiles[pushBlockForward].transform.position = BlockPositioner(distBetweenBlock);

            platfotmTiles[pushBlockForward].GetComponent<Block>().SetBlock();

            pushBlockForward = (pushBlockForward + 1 < platfotmTiles.Count) ? pushBlockForward += 1 : pushBlockForward = 0;
        }
    }

    // kaycak bloğa karar ver, input al, input varsa ona göre haraket et, yol hesapla, boost moda giriyor mu ona bak
    private void Update()
    {
        if (game.state == GameHandler.GameState.GameRunning)
        {
            ınput.directionGetter();

            if (ınput.directions.Count != 0)
            {
                ınput.dirr = ınput.directions.Dequeue();
                MoveTile((int)ınput.dirr);
            }

            straightRoadLenght = platfotmTiles[blockToSlide].transform.position.y - runner.transform.position.y; // camera ve kombo için uzaklık hesapla

            //Debug.Log(straightRoadLenght);

            distanceBtwRunner = runner.transform.position.y - Nightmare.transform.position.y;

            if (distanceBtwRunner > 12f && !boostLock) //kombo var mı hesapla
            {
                boostLock = true;

                boostTimer = 0f;
                gainedPoint += 1;

                Boost.StartBoost(20f);

                GiveMessage(2f, "RUN!!!");
                Debug.LogWarning("BOOST !! at : " + Time.unscaledTime);
            }
            else if(boostLock)
            {
                boostTimer += Time.deltaTime;
                if(boostTimer >= boostLimit)
                {
                    boostLock = false;
                    Boost.StopBoost(5f);
                    GiveMessage(2f, "SLOWDOWN");
                    Debug.LogWarning("Boost finished... at : " + Time.unscaledTime);
                }
            }

            /*if (Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, 0f)) //kaycak bloğa karar veriyor. MoveTile de kayar kaymaz yapılıyor artık
            {
                blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
            }*/
        }
    }

    private void MoveTile(int direction)
    {
        float toPos = 0;

        if (!Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, 0f)) // eğer zaten ortada değilse
        {
            
            if (!isBoost)
                toPos = platfotmTiles[blockToSlide].transform.position.x + (direction * distBetweenBlock); // nereye gitcek onu hesapla
            else
                toPos = 0; //block will go to zero in either direction


            if(toPos < BlockPos[0] || toPos > BlockPos[BlockPos.Length - 1])
            {
                game.GameOver();
                platfotmTiles[blockToSlide].GetComponent<Block>().Fall(new Vector2(direction, 0));
                //StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().Fall(new Vector2(direction, 0)));

                uI.GameOver();
            }
            else
            {
                platfotmTiles[blockToSlide].GetComponent<Block>().MoveTile(toPos);

                if (Mathf.Approximately(toPos, 0)) // eğer 0 a geliyorsa bi sonraki bloğa geç
                {
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
                    point += gainedPoint;
                    uIGame.SetPoint(point);
                }
            }

         /*   if (toPos < BlockPos[0] || toPos > BlockPos[BlockPos.Length - 1]) //en uçta yanlış yöne basıldıysa düş
            {
                game.GameOver();
                platfotmTiles[blockToSlide].GetComponent<Block>().Fall(new Vector2(direction, 0));
                //StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().Fall(new Vector2(direction, 0)));
                uI.GameOver();
            }
            else //yoksa o yöne doğru git
            {
                platfotmTiles[blockToSlide].GetComponent<Block>().MoveTile(toPos);
                if (Mathf.Approximately(toPos, 0)) // eğer 0 a geliyorsa bi sonraki bloğa geç
                {
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
                    point += gainedPoint;
                    uI.SetPoint(point);
                }
            }*/
        }
    }

#region RoadCreating

    // blokları konumlandıran fonksiyon
    private Vector2 BlockPositioner(float rate)
    {
        int tempEx = exRand;

        exRand = MathCalculation.RandomPosition(exRand, sameLine, BlockPos.Length);
        sameLine = (tempEx == exRand) ? sameLine += 1 : sameLine = 0;
        distance += rate;

        return new Vector2(BlockPos[exRand], distance);
    }


    public void CreatePlatform() //Set initial Road and parameters
    {
        //Sizes are changed according to Screen 
        //When First Inıt script written Call this and put sizes on PlayerPrefs
        distBetweenBlock = platformSizeHandler.ArrangeSize(road.transform, lines.transform, block.transform, runner.transform);
        //For 5 line mode
        if (Data.is5Line)
        {
            BlockPos = new float[] { -2 * distBetweenBlock, -1 * distBetweenBlock, distBetweenBlock, 2 * distBetweenBlock };
        }
        else
        {
            BlockPos = new float[] { -1 * distBetweenBlock, distBetweenBlock };
        }

        distance = -5f; // Start from -5

        for (int i = 1; i < platfotmTiles.Count; i++)
        {
            Destroy(platfotmTiles[i]);
        }
        platfotmTiles.Clear();
        platfotmTiles.Add(block);
        //platfotmTiles[0].GetComponent<Block>().SetBlock();

        int levelStartStraightLine = 5; // start from third block to give full road

        platfotmTiles[platfotmTiles.Count - 1].transform.position = new Vector2(0f, distance);


        for (int i = 0; i < levelStartStraightLine; i++)
        {
            distance += distBetweenBlock;
            platfotmTiles.Add((GameObject)Instantiate(block, this.transform));
            platfotmTiles[platfotmTiles.Count - 1].transform.position = new Vector2(0f, distance);

        }

        //Platform tiles da buluncak toplam blok sayısından ilk baştaki düz blokları çıkar 
        int remainingBlock = BlockNumberInPlatformTiles - levelStartStraightLine; 

        for (int i = 0; i < remainingBlock; i++)
        {
            platfotmTiles.Add((GameObject)Instantiate(block, this.transform));
            platfotmTiles[platfotmTiles.Count - 1].transform.position = BlockPositioner(distBetweenBlock);
        }

        runner.transform.position = instance.platfotmTiles[levelStartStraightLine].transform.position; //Runner düz sıranın en sonunda başlıyor
        blockToSlide = levelStartStraightLine + 1;

        initialStraightRoadLenght = 3 * distBetweenBlock;//platfotmTiles[blockToSlide].transform.position.y - runner.transform.position.y; // camera ve kombo için uzaklık hesapla
        straightRoadLenght = platfotmTiles[blockToSlide].transform.position.y - runner.transform.position.y;//initialStraightRoadLenght; // camera ve kombo için uzaklık hesapla
        //initialStraightRoadLenght = straightRoadLenght;

        //Debug.Log("Length  length is : " + initialStraightRoadLenght);
    }

#endregion


#region SimpleSetMethods
    public void SetSpeed() //Set speed for bore and monster
    {
        runner.GetComponent<Runner>().CharacterSpeed = GameData.GetBoreSpeed();
        //Nightmare.GetComponent<BadThingParticleSystem>().monsterSpeed = GameData.GetMonsterSpeed();
    }

    private int GetCurrentLevel()
    {
        return GameData.GetLevel();
    }

    //This method is used for getting game uı panel. 
    //Since this script attached to OnGamePanel which owned by UI handler, UI handler returns this script from OnGamePanel
    private UIGameHandler SetUIGameHandler()
    {
        return uI.GetGamePanel();
    }

    public bool SetBoost(bool to)
    {
        return isBoost = to;
    }

    private void GiveMessage(float time, string message)
    {
        StartCoroutine(uIGame.GiveInfo(time, message));
    }
#endregion

   


    //Gereksiz function ama camera da ilk başta bbunla setliyor şimdilik ondan duruyor
    //TODO: Bu değişcek

    public void ChangeAngle() //for mode //new Vector3(0f,0f,-10f)new Vector3(0f, -6f, -10f)
    {
        if (!Data.isAngled)
        {
            foreach (GameObject g in platfotmTiles)
            {
                g.transform.GetChild(0).gameObject.SetActive(false);
            }
            Camera.main.gameObject.transform.eulerAngles = Vector3.zero;
            Camera.main.gameObject.GetComponent<CameraMovement>().CalculateOffset(runner.transform.position + new Vector3(0f, -3f, 10f));
            if (Data.is5Line)
            {
                Camera.main.gameObject.GetComponent<CameraMovement>().OrthographicLowerSize = 55f;
                Camera.main.gameObject.GetComponent<CameraMovement>().OrthographicUpperSize = 80f;
            }
            else
            {
                Camera.main.gameObject.GetComponent<CameraMovement>().OrthographicLowerSize = 55f;
                Camera.main.gameObject.GetComponent<CameraMovement>().OrthographicUpperSize = 90f;
            }

            //Camera.main.gameObject.transform.position = Vector3.zero;
        }
        else
        {
            foreach (GameObject g in platfotmTiles)
            {
                g.transform.GetChild(0).gameObject.SetActive(true);
            }
            Camera.main.gameObject.transform.eulerAngles = new Vector3(-30f, 0f, 0f);
            Camera.main.gameObject.GetComponent<CameraMovement>().CalculateOffset(runner.transform.position + new Vector3(0f, 3f, 10f));
            if (Data.is5Line)
            {
                Camera.main.gameObject.GetComponent<CameraMovement>().OrthographicLowerSize = 55f;
                Camera.main.gameObject.GetComponent<CameraMovement>().OrthographicUpperSize = 70f;
            }
            else
            {
                Camera.main.gameObject.GetComponent<CameraMovement>().OrthographicLowerSize = 55f;
                Camera.main.gameObject.GetComponent<CameraMovement>().OrthographicUpperSize = 75f;
            }
            //Camera.main.gameObject.transform.position = new Vector3(0f, 6f, 0f);
        }
    }
}


/* private void MoveTile(int direction)
    {
        float toPos = 0;
        if (platfotmTiles[blockToSlide].GetComponent<Block>().type == BlockData.blockType.normal) //if block is normal
        {
            if (ınput.dirr == InputManager.direction.right)
            {
                if (platfotmTiles[blockToSlide].transform.position.x > 0 ) // if pressed right and next tile is on right
                {
                    if(explosionParticleSystem != null)
                    {
                        explosionParticleSystem.Explode(platfotmTiles[blockToSlide].transform.position);// xplosion
                    }
                    toPos = platfotmTiles[blockToSlide].transform.position.x - distBetweenBlock;//(!Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, 0)) ? platfotmTiles[blockToSlide].transform.position.x - distBetweenBlock : 0;
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(toPos));
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count  && Mathf.Approximately(toPos,0)) ? blockToSlide += 1 : blockToSlide;
                    point += gainedPoint;
                    uI.SetPoint(point);
                    //platfotmTiles[blockToSlide].transform.position = new Vector2(platfotmTiles[blockToSlide].transform.position.x - distBetweenBlock, platfotmTiles[blockToSlide].transform.position.y);
                }
                else if(Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x,BlockPos[1])) //if pressed Right but tile is on left
                {
                    toPos = platfotmTiles[blockToSlide].transform.position.x - distBetweenBlock;
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(toPos));
                }
                else // if pressed Right but tile is on leftmost
                {
                    game.GameOver();
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().Fall(new Vector2(-1f, 0)));
                    uI.GameOver();
                }
            }
            else if (ınput.dirr == InputManager.direction.left)
            {
                if (platfotmTiles[blockToSlide].transform.position.x < 0 ) // if pressed left and tile is on left
                {
                    if (explosionParticleSystem != null)
                    {
                        explosionParticleSystem.Explode(platfotmTiles[blockToSlide].transform.position);// xplosion
                    }
                    toPos = platfotmTiles[blockToSlide].transform.position.x + distBetweenBlock;//(!Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, 0)) ? platfotmTiles[blockToSlide].transform.position.x + distBetweenBlock : 0;
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(toPos));
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count && Mathf.Approximately(toPos,0)) ? blockToSlide += 1 : blockToSlide;
                    point += gainedPoint;
                    uI.SetPoint(point);
                    //platfotmTiles[blockToSlide].transform.position = new Vector2(platfotmTiles[blockToSlide].transform.position.x + distBetweenBlock, platfotmTiles[blockToSlide].transform.position.y);
                }
                else if (Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, BlockPos[2])) // if pressed left but tile is on right most
                {
                    toPos = platfotmTiles[blockToSlide].transform.position.x + distBetweenBlock;
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(toPos));
                }
                else // if pressed left but tile is on right most
                {
                    game.GameOver();
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().Fall(new Vector2(1f, 0)));
                   
                    uI.GameOver();
                }
            }
        }
        else // if block is reverse NOT usigin DELETE!!!!
        {
            if (ınput.dirr == InputManager.direction.right)
            {
                if (platfotmTiles[blockToSlide].transform.position.x < 0 )//Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, BlockPos[0])) // if pressed right tile is on left
                {
                    if (explosionParticleSystem != null)
                    {
                        explosionParticleSystem.Explode(platfotmTiles[blockToSlide].transform.position);// xplosion
                    }
                    toPos = (!Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, 0)) ? platfotmTiles[blockToSlide].transform.position.x + distBetweenBlock : 0;
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(toPos));
                    //blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
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
                    if (explosionParticleSystem != null)
                    {
                        explosionParticleSystem.Explode(platfotmTiles[blockToSlide].transform.position);// xplosion
                    }
                    toPos = (!Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, 0)) ? platfotmTiles[blockToSlide].transform.position.x - distBetweenBlock : 0;
                    StartCoroutine(platfotmTiles[blockToSlide].GetComponent<BlockAnimation>().MoveTile(toPos));
                    //blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
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
        Debug.Log(toPos);
    }*/



//OLD START 


/* distBetweenBlock = platformSizeHandler.ArrangeSize(road.transform, lines.transform, block.transform, runner.transform);
        if (Data.is5Line)
            BlockPos = new float[] { -2 * distBetweenBlock, -1 * distBetweenBlock, distBetweenBlock, 2 * distBetweenBlock };
        else
            BlockPos = new float[] { -1 * distBetweenBlock, distBetweenBlock };
        distance = -5f; // Start from -5
        int levelStartStraightLine = 5; // first straight line
        platfotmTiles[platfotmTiles.Count - 1].transform.position = new Vector2(0f, distance);
        for (int i = 0; i < levelStartStraightLine; i++)
        {
            distance += distBetweenBlock;
            platfotmTiles.Add((GameObject)Instantiate(block, this.transform));
            platfotmTiles[platfotmTiles.Count - 1].transform.position = new Vector2(0f, distance);
        }
        blockNum = 28; //total block number is = levelStartghtLine + block num
        for (int i = 0; i < blockNum; i++)
        {
            platfotmTiles.Add((GameObject)Instantiate(block, this.transform));
            platfotmTiles[platfotmTiles.Count - 1].transform.position = BlockPositioner(distBetweenBlock);
        }
        runner.transform.position = instance.platfotmTiles[levelStartStraightLine].transform.position; //Runner starts from 4rd tile
        blockToSlide = levelStartStraightLine + 1;
        initialStraightRoadLenght = 3 * distBetweenBlock;//platfotmTiles[blockToSlide].transform.position.y - runner.transform.position.y; // camera ve kombo için uzaklık hesapla
        straightRoadLenght = platfotmTiles[blockToSlide].transform.position.y - runner.transform.position.y;//initialStraightRoadLenght; // camera ve kombo için uzaklık hesapla
        Debug.Log("Initial length is : " + initialStraightRoadLenght);*/

