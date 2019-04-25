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
    public LevelManager levelManager; //Could be private but public for displaying level parameters for now.
    public PlatformSizeHandler sizeHandler;

    private GetData GameData; 
    private InputManager ınput;
    private UIHandler uI;
    private UIGameHandler uIGame;
    private BoostScript Boost;


    //GameObjects
    public GameObject Blocks; //Pooled Blocks Parent
    public GameObject block; //kırmızı bloklar
    public GameObject Runner; //koşan arkadaş artık neyse
   // public GameObject Lines;
    public GameObject Road; //Block, RoadSprite ve lines ın Parent ı olan GameObject.
    public GameObject RoadSprite;//Yol sprite ı. Karanlık World için gri düz bir kare. Road un ilk çocuğu 
    public GameObject Background; //rengi değişen arkaPlan. Runner ın child ı.
    public GameObject Nightmare;
    public GameObject Shooter;

    public List<GameObject> platfotmTiles; //blokları barındıran liste
    public List<Block> blockScripts;
    public GameObject[] Shooters;

    //Parameters
    public bool inputLock; //Used when boost is finished and animation begins && maybe in other various states

    private float distance; //bi sonraki bloğun gelceği y mesafesi.  habire artıyor
    public float distBetweenBlock; //bloklar arası x ve y mesafesi
    public Vector2 blockScale;

    public float[] BlockPos; // blokların oluşabilceği pozisyonlar

    private int BlockNumberInPlatformTiles; //Platform tiles listesinde kaç blok olacağı

    public int blockToSlide; // o sırada kaydırılcak blok

    public int level_p;
    public int passedBlockNumber;

    //For block positioning
    private int exRand = 3;
    private int sameLine = 0;

    private int point; //Total point
    public int gainedPoint; //Point that player gains under circumtances

    public float distanceBtwRunner;

    private bool boostLock; //since isBoost is changed in BoostScript, boostLock is used for locking update statement where boost started. Bir kere girsin diye

    private BoostScript.BoostPhase boostPhase; //Used for setting boost phases since behaviour will be different in each phase need more than just a bool
    private bool isBoostAllowed;

    public float boostTime;
    public float boostTimer;
    private float boostLimit;

    public int pushBlockForward; //En arkada kalan blok yani o sırada sıranın en sonuna atılcak blok. 

    private bool is5Line;

    public float straightRoadLenght;
    public float initialStraightRoadLenght; // for refence point of starting size

    private Vector3 offsetRunnerBtwRoadSprite;

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
        sizeHandler = new PlatformSizeHandler();

        level_p = GetCurrentLevel();
        levelManager = new LevelManager(level_p);

        Boost = (BoostScript)FindObjectOfType(typeof(BoostScript));
        uI = (UIHandler)FindObjectOfType(typeof(UIHandler));
        uIGame = SetUIGameHandler();

        //////////////
        //If gameobjects not setted from editor find with tag.
        if (!Road)
            Road = GameObject.FindWithTag("Road");

        if (!Blocks)
            Blocks = GameObject.FindWithTag("Blocks");

        if (!block)
            block = Blocks.transform.GetChild(0).gameObject;

        if(!Runner)
            Runner = GameObject.FindWithTag("Runner");

        //if (!Lines)
          //  Lines = GameObject.FindWithTag("Lines");

        if (!RoadSprite)
            RoadSprite = GameObject.FindWithTag("RoadSprite");

        if (!Background)
            Background = GameObject.FindWithTag("Background");

        if (!Nightmare)
            Nightmare = GameObject.FindWithTag("Nightmare");
     

        Shooters = GameObject.FindGameObjectsWithTag("Shooter");

        foreach (GameObject shooter in Shooters)
        {
            shooter.gameObject.SetActive(false);
            shooter.transform.position = new Vector2(0f, -5f);
        }

        //background.transform.position = new Vector2(0f, runner.transform.position.y + 5);

        platfotmTiles = new List<GameObject>();
        blockScripts = new List<Block>();
        //platfotmTiles.Add(block);
        passedBlockNumber = 0;

        point = 0;
        gainedPoint = 1;

        pushBlockForward = 0;
        BlockNumberInPlatformTiles = 30;

        boostTime = 8f;
        boostLimit = 12f;

        CreatePlatformAccordingToLevel();

        SetBoreSpeed(); //Set player speed from playerprefs.
        //CreatePlatform();
        SetBoostPhase(BoostScript.BoostPhase.None);

        uI.OpenHomePage(); //After arranging everything open uı panel for starting game

        inputLock = false;
    }



    private void LateUpdate()
    {
        //Lines.transform.position = Runner.transform.position + offsetRunnerBtwRoadSprite;
        RoadSprite.transform.position = Runner.transform.position + offsetRunnerBtwRoadSprite;

        //runnerla en arkada kalan blok arasındaki mesafe 10 bloğu geçerse giriyor buraya.
        //Onları ilerletmişken road ve lines da ileri atılıyor.
        if (Runner.transform.position.y >= platfotmTiles[pushBlockForward].transform.position.y + (10 * distBetweenBlock))
        {
            platfotmTiles[pushBlockForward].transform.position = BlockPositioner(distBetweenBlock);
            blockScripts[pushBlockForward].SetBlock(levelManager.levelBlockType);
            pushBlockForward = (pushBlockForward + 1 < platfotmTiles.Count) ? pushBlockForward += 1 : pushBlockForward = 0;
        }

        //en sondaki engel arkada kaldıysa tüm engellerin yerini tekrar hesaplayıp ileri at.
        if(Shooters[Shooters.Length -1].transform.position.y < Runner.transform.position.y)
            PlaceObstacles();
    }

    // kaycak bloğa karar ver, input al, input varsa ona göre haraket et, yol hesapla, boost moda giriyor mu ona bak
    private void Update()
    {
        if (game.GetGameState() == GameHandler.GameState.GameRunning)
        {
            if(!inputLock)
                ınput.directionGetter();

            if (ınput.directions.Count != 0)
            {
                ınput.dirr = ınput.directions.Dequeue();
                MoveTile((int)ınput.dirr);
            }

            straightRoadLenght = platfotmTiles[blockToSlide].transform.position.y - Runner.transform.position.y - distBetweenBlock; // camera orthogonic size ve runner hızı için uzaklık hesapla

            distanceBtwRunner = Runner.transform.position.y - Nightmare.transform.position.y;

            //////////////////////////////
            //Calculate if level passed or ended.
            //
            if (levelManager.IsEndingConditionSatisfied(distanceBtwRunner, passedBlockNumber))
            {
                LevelPassed();
            }
            else if(distanceBtwRunner < 0.8f) 
            {
                GameOver();
            }

            //////////////////////////////
            //Calculate if entered or exited boost
            if (distanceBtwRunner > 100 && !boostLock && isBoostAllowed && GetBoostPhase() == BoostScript.BoostPhase.None)
            {
                boostLock = true;

                boostTimer = 0f;
                gainedPoint += 1;

                Boost.StartBoost();

                GiveMessage(2f, "RUN!!!");
                Debug.LogWarning("BOOST !! at : " + Time.unscaledTime);
                Debug.LogWarning("Distance is : " + distanceBtwRunner);
            }
            else if(boostLock)
            {
                boostTimer += Time.deltaTime;
                if(boostTimer >= boostTime)
                {
                    boostLock = false;
                    inputLock = true;
                    Boost.BoostFinish();

                    GiveMessage(2f, "SLOWDOWN");
                    Debug.LogWarning("Boost finished... at : " + Time.unscaledTime);
                }
            }

        }
    }



    private void MoveTile(int direction)
    {
        float toPos = 0;

        if (!Mathf.Approximately(platfotmTiles[blockToSlide].transform.position.x, 0f)) // eğer zaten ortada değilse
        {
            if (blockScripts[blockToSlide].type == BlockData.blockType.reverse) // eğer ters bloksa -1 le çarp ki ters yöne doğru gitsin
                direction *= -1;
            
            if (GetBoostPhase() == BoostScript.BoostPhase.None)
                toPos = platfotmTiles[blockToSlide].transform.position.x + (direction * distBetweenBlock); // nereye gitcek onu hesapla
            else
                toPos = 0; //block will go to zero in either direction


            if(toPos < BlockPos[0] || toPos > BlockPos[BlockPos.Length - 1]) // eğer gitceği yer blockpos sınırları içinde değilse oyunu sonlandır
            {
                blockScripts[blockToSlide].Fall(new Vector2(direction, 0));
                GameOver();
            }
            else // eğer blockPos sınırları içindeyse bloğu haraket ettir
            {
                blockScripts[blockToSlide].MoveTile(toPos);

                if (Mathf.Approximately(toPos, 0)) // eğer 0 a geliyorsa bi sonraki bloğa geç
                {
                    blockToSlide = (blockToSlide + 1 < platfotmTiles.Count) ? blockToSlide += 1 : blockToSlide = 0;
                    passedBlockNumber++;
                    point += gainedPoint;
                    uIGame.SetPoint(point);
                }
            }
        }
    }

#region RoadCreating

    // blokları konumlandıran fonksiyon
    private Vector2 BlockPositioner(float rate)
    {
        int tempEx = exRand;

        exRand = MathCalculation.RandomPosition(exRand, sameLine, BlockPos.Length,is5Line);
        sameLine = (tempEx == exRand) ? sameLine += 1 : sameLine = 0;
        distance += rate;

        return new Vector2(BlockPos[exRand], distance);
    }


    public void CreatePlatform() //Set initial Road and parameters
    {
        //Sizes are changed according to Screen 
        is5Line = (levelManager.levelWidth == LevelManager.LevelWidth.Five) ? true : false;
        distBetweenBlock = sizeHandler.ArrangeSize(RoadSprite.transform, block.transform, Runner.transform,is5Line);
        blockScale = block.transform.localScale;

        //For 5 line mode
        if (is5Line)
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
        blockScripts.Clear();

        //First add initial block that can be seen in editor.
        platfotmTiles.Add(block);
        blockScripts.Add(block.GetComponent<Block>());

        platfotmTiles[platfotmTiles.Count - 1].transform.position = new Vector2(0f, distance);
        blockScripts[blockScripts.Count - 1].enabled = true;
        blockScripts[blockScripts.Count - 1].SetBlock(levelManager.levelBlockType);

        //platfotmTiles[0].GetComponent<Block>().SetBlock();

        int levelStartStraightLine = 5; // start from third block to give full road at the beginning at level

        for (int i = 0; i < levelStartStraightLine; i++)
        {
            distance += distBetweenBlock;
            platfotmTiles.Add((GameObject)Instantiate(block, Blocks.transform));
            platfotmTiles[platfotmTiles.Count - 1].transform.position = new Vector2(0f, distance);

            blockScripts.Add(platfotmTiles[platfotmTiles.Count - 1].GetComponent<Block>());
            blockScripts[blockScripts.Count - 1].enabled = true;
            blockScripts[blockScripts.Count - 1].SetBlock(levelManager.levelBlockType);
        }

        //Platform tiles da buluncak toplam blok sayısından ilk baştaki düz blokları çıkar 
        int remainingBlock = BlockNumberInPlatformTiles - levelStartStraightLine; 

        for (int i = 0; i < remainingBlock; i++)
        {
            platfotmTiles.Add((GameObject)Instantiate(block, Blocks.transform));
            platfotmTiles[platfotmTiles.Count - 1].transform.position = BlockPositioner(distBetweenBlock);

            blockScripts.Add(platfotmTiles[platfotmTiles.Count - 1].GetComponent<Block>());
            blockScripts[blockScripts.Count - 1].enabled = true;
            blockScripts[blockScripts.Count - 1].SetBlock(levelManager.levelBlockType);
        }

        Runner.transform.position = instance.platfotmTiles[levelStartStraightLine].transform.position; //Runner düz sıranın en sonunda başlıyor
        blockToSlide = levelStartStraightLine + 1;

        initialStraightRoadLenght = 2 * distBetweenBlock;//platfotmTiles[blockToSlide].transform.position.y - runner.transform.position.y; // camera ve kombo için uzaklık hesapla
        straightRoadLenght = platfotmTiles[blockToSlide].transform.position.y - Runner.transform.position.y;//initialStraightRoadLenght; // camera ve kombo için uzaklık hesapla

        offsetRunnerBtwRoadSprite = RoadSprite.transform.position - Runner.transform.position;
    }


    public void PlaceObstacles()
    {
        int lowerYLimit = 10;
        int upperYLimit = 20;

        foreach(GameObject dragon in Shooters) 
        {
            if(dragon.transform.position.y < Runner.transform.position.y)
            {
                int r = (Random.Range(0, 3) == 1) ? -1 : 1;

                float x = sizeHandler.GetWallPosition() * r;
                float y = platfotmTiles[blockToSlide].transform.position.y + (Random.Range(lowerYLimit, upperYLimit) * distBetweenBlock);

                Vector3 dragonPos = new Vector3(x,y,0f);

                dragon.transform.position = dragonPos;
                dragon.transform.rotation = Quaternion.Euler(0f, 0f, 90 * r);

                if(!dragon.activeInHierarchy)
                    dragon.SetActive(true);

                lowerYLimit += 15;
                upperYLimit += 15;
            }
        }      
    }

    #endregion


#region SimpleMethods

    public void SetBoreSpeed(int i = 0) //Set speed for bore if default called withour any parameters setted to playerprefs speed. İf called with animation setted to zero
    {
        if (i == 0)
            Runner.GetComponent<Runner>().CharacterSpeed = GameData.GetBoreSpeed();
        else if (i == (int)BoostScript.BoostPhase.AnimationSlideDown)
            Runner.GetComponent<Runner>().CharacterSpeed = 0f;
        else
            Debug.LogError("Wrong SetBoreSpeed parameter");
    }

    public float GetBoreSpeed()
    {
        return Runner.GetComponent<Runner>().CharacterSpeed;
    }

    public void CreatePlatformAccordingToLevel() 
    {
        level_p = GetCurrentLevel();
        levelManager.SetParametersForLevel(level_p,ref Nightmare.GetComponent<BadThingParticleSystem>().monsterSpeed,ref isBoostAllowed);
        foreach(Block b in blockScripts) 
        {
            b.SetBlock(levelManager.levelBlockType);
        }
        CreatePlatform();
        SetMonsterPosition();
    }

    public void GameOver() 
    {
        game.GameOver();
        uI.GameOver();
    }

    public void LevelPassed()
    {
        game.LevelPassed();
        uI.GameOver();
    }

    private void SetMonsterPosition() 
    {
        float  distanceBetweenRunner = (!is5Line) ? 8f : 11f;

        Nightmare.transform.position = new Vector3(Runner.transform.position.x, Runner.transform.position.y - distanceBetweenRunner, 0f);
    }

    private int GetCurrentLevel()
    {
        return GameData.GetLevel(); // or Data.GetLevel();
    }

    //This method is used for getting game uı panel. 
    //Since this script attached to OnGamePanel which owned by UI handler, UI handler returns this script from OnGamePanel
    //Can only get uıGameHandler this way because gamobject is disabled from uı handler.
    private UIGameHandler SetUIGameHandler()
    {
        return uI.GetGamePanel();
    }

    public BoostScript.BoostPhase GetBoostPhase()
    {
        return boostPhase;
    }

    public BoostScript.BoostPhase SetBoostPhase(BoostScript.BoostPhase to)
    {
        if (to == BoostScript.BoostPhase.None && inputLock)
            inputLock = false;

        return boostPhase = to;
    }

    private void GiveMessage(float time, string message)
    {
        StartCoroutine(uIGame.GiveInfo(time, message));
    }
  #endregion
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




//Gereksiz function ama camera da ilk başta bununla setliyor şimdilik ondan duruyor
//TODO: Bu değişcek

/* public void ChangeAngle() //for mode //new Vector3(0f,0f,-10f)new Vector3(0f, -6f, -10f)
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
 }*/
