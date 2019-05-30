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
    private UIOnGamePage uIGame;
    private BoostScript Boost;
    private ObjectManager objectManager;

    //GameObjects
    public GameObject Runner; //koşan arkadaş artık neyse
    public GameObject Nightmare;

    //public GameObject Road; //Block, RoadSprite ve lines ın Parent ı olan GameObject.
    public GameObject RoadSprite;//Yol sprite ı. Karanlık World için gri düz bir kare. Road un ilk çocuğu 
    public GameObject Background; //rengi değişen arkaPlan. Runner ın child ı.

    public List<GameObject> platfotmTiles; //blokları barındıran liste
    public List<Block> blockScripts;
    public GameObject[] Shooters;

    //Parameters
    bool inputLock; //Used when boost is finished and animation begins && maybe in other various states
    bool levelLock;
    float CheckPointPos;
    private float CurrentBlockPos; //bi sonraki bloğun gelceği y mesafesi.  habire artıyor
    public float distBetweenBlock; //bloklar arası x ve y mesafesi
    public Vector2 blockScale;

    float[] BlockPos; // blokların oluşabilceği pozisyonlar

    public int blockToSlide; // o sırada kaydırılcak blok
    public int blockOnTail; //En arkada kalan blok yani o sırada sıranın en sonuna atılcak blok. 

    public int level_p;
    public int passedBlockNumber;

    private int point; //Total point
    public int gainedPoint; //Point that player gains under circumtances

    public float distanceBtwRunner;

    private bool boostLock; //since isBoost is changed in BoostScript, boostLock is used for locking update statement where boost started. Bir kere girsin diye

    private BoostScript.BoostPhase boostPhase; //Used for setting boost phases since behaviour will be different in each phase need more than just a bool
    private bool isBoostAllowed;

    public float boostTime;
    public float boostTimer;
    private float boostLimit;

    public float PlayerBehindCamTime; // Using when cam chase bore on boost. If that timer bigger than some value or player is too behind from cam stop Boost
    public bool PlayerTooBehindCam; // Player is too behind camera stop boost
    private bool wrongPressedOnBoost;

    private bool is5Line;

    public float straightRoadLenght;
    public float initialStraightRoadLenght; // for refence point of starting size

    public bool cameraChase;

    private Vector3 offsetRunnerBtwRoadSprite;

    public static Platform instance;

    public ObjectManager Manager1 { get => objectManager; set => objectManager = value; }

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Platform already exist!");
            Destroy(this);
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

        objectManager = new ObjectManager();

        Boost = (BoostScript)FindObjectOfType(typeof(BoostScript));
        uI = (UIHandler)FindObjectOfType(typeof(UIHandler));
        uIGame = SetUIGameHandler();



        //////////////
        //If gameobjects not setted from editor find with tag.
        //if (!Road)
        //  Road = GameObject.FindWithTag("Road");

        if (!Runner)
            Runner = GameObject.FindWithTag("Runner");

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
       
        CreatePlatformAccordingToLevel();

        //CreatePlatform();
        SetBoostPhase(BoostScript.BoostPhase.None);

        SetCamChase(true); //used when boost
        uI.OpenHomePage(); //After arranging everything open uı panel for starting game
    }



    private void LateUpdate()
    {
        if (game.GetGameState() == GameHandler.GameState.GameRunning)
        {

            RoadSprite.transform.position = Runner.transform.position + offsetRunnerBtwRoadSprite;

            //runnerla en arkada kalan blok arasındaki mesafe 15 bloğu geçerse giriyor buraya. Eğer son atılan blok check pointten önceki sonuncuysa yeni level e geçiyor platform.
            if (Runner.transform.position.y >= platfotmTiles[blockOnTail].transform.position.y + (15 * distBetweenBlock))
            {
                if (!levelLock && !PushTailBlockToHead())
                {
                    levelLock = true;
                    MoveCheckPoint();
                }

            }

            //en sondaki engel arkada kaldıysa tüm engellerin yerini tekrar hesaplayıp ileri at. 
            //TODO: Şu anda tüm obstacleları aynı anda atıyor. Platform dizilişine karar vercek ayrı bir sınıf olmalı orda tüm bunları yapmalıyız.Engel ve Blok pozisyonlandırma, blok tip karar verme etc.
            if (Shooters[Shooters.Length - 1].transform.position.y < Runner.transform.position.y)
                PlaceObstacles();

        }
    }

    // kaycak bloğa karar ver, input al, input varsa ona göre haraket et, yol hesapla, boost moda giriyor mu ona bak
    private void Update()
    {
        if (game.GetGameState() == GameHandler.GameState.GameRunning)
        {
            if (!inputLock)
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
                Debug.Log("Level Passed !!! Checkpoint reached.");
                LevelPassed();
            }
            else if (distanceBtwRunner < 0.8f)
            {
                GameOver();
            }

            //////////////////////////////
            //Calculate if entered or exited boost

            if (/*distanceBtwRunner > boostLimit*/Input.GetKeyDown("space")   && !boostLock && isBoostAllowed && GetBoostPhase() == BoostScript.BoostPhase.None)
            {
                boostLock = true; 
                InitiateBoost();
            }
            else if (boostLock)
            {
                boostTimer += Time.deltaTime;

                if (IsBoostEnded())
                {
                    boostLock = false;
                    inputLock = true; // Set to false when boost animation finishes

                    LostBoost();
                }
            }
        }
        else if(game.GetGameState() == GameHandler.GameState.LevelPassed)
        {
            straightRoadLenght = platfotmTiles[blockToSlide].transform.position.y - Runner.transform.position.y - distBetweenBlock;

            ınput.directionGetter();

            if (ınput.directions.Count != 0)
            {
                ınput.dirr = ınput.directions.Dequeue();
                MoveTile((int)ınput.dirr);
            
                game.LevelStarted();
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

            if (GetBoostPhase() == BoostScript.BoostPhase.None || GetCamChase())
                toPos = platfotmTiles[blockToSlide].transform.position.x + (direction * distBetweenBlock); // nereye gitcek onu hesapla
            else
                toPos = 0; //block will go to zero in either direction


            if (toPos < BlockPos[0] || toPos > BlockPos[BlockPos.Length - 1]) // eğer gitceği yer blockpos sınırları içinde değilse oyunu sonlandır
            {
                if (GetBoostPhase() == BoostScript.BoostPhase.None)
                {
                    blockScripts[blockToSlide].Fall(new Vector2(direction, 0), false);
                    GameOver();
                }
                else
                {
                    wrongPressedOnBoost = true;
                    blockScripts[blockToSlide].Fall(new Vector2(direction, 0), wrongPressedOnBoost); //Block will rollback from fall animation
                }
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

    bool PushTailBlockToHead()
    {
        if(CurrentBlockPos - distBetweenBlock >= CheckPointPos + (levelManager.length * distBetweenBlock) )
        {
            return false;
        }

        Vector2 newBlockPos = new Vector2(BlockPos[objectManager.BlockPosition(BlockPos.Length, is5Line)], CurrentBlockPos);
        platfotmTiles[blockOnTail].transform.position = newBlockPos;
        blockScripts[blockOnTail].SetBlock(levelManager.levelBlockType);

        CurrentBlockPos += distBetweenBlock;

        blockOnTail = (blockOnTail + 1 < platfotmTiles.Count) ? blockOnTail += 1 : blockOnTail = 0;

        return true;
    }


    #region RoadCreating

    public void CreatePlatformAccordingToLevel()
    {
        level_p = GetCurrentLevel();
        levelManager.SetParametersForLevel(level_p, ref isBoostAllowed);

        SetPlatformVaraibles();
        CreatePlatform();
    }

    void SetPlatformVaraibles(float headPos = 0,int p = 0)
    {
        //Sizes are changed according to Screen 
        is5Line = (levelManager.levelWidth == LevelManager.LevelWidth.Five) ? true : false;
        distBetweenBlock = sizeHandler.ArrangeSize(RoadSprite.transform, ref blockScale, Runner.transform, is5Line);

        //For 5 line mode
        if (is5Line)
        {
            BlockPos = new float[] { -2 * distBetweenBlock, -1 * distBetweenBlock, distBetweenBlock, 2 * distBetweenBlock };
        }
        else
        {
            BlockPos = new float[] { -1 * distBetweenBlock, distBetweenBlock };
        }

        initialStraightRoadLenght = 2 * distBetweenBlock;
        passedBlockNumber = 0;

        point = p;
        gainedPoint = 1;

        blockOnTail = 0;
        blockToSlide = 0;
        CurrentBlockPos = headPos;

        inputLock = false; //Using for locking input to game

        if (GetCamChase())
            boostTime = 1f;
        else
            boostTime = 8f;

        boostLimit = 12f;
        wrongPressedOnBoost = false;

    }

    void CreatePlatform() //Set initial Road and parameters
    {
        //Clear block scripts for initiliazing them again
        blockScripts.Clear();

        CheckPointPos = objectManager.SetCheckPoint(CurrentBlockPos - (5 * distBetweenBlock), distBetweenBlock, blockScale);

        platfotmTiles = objectManager.SetBlocks(distBetweenBlock, BlockPos, is5Line, ref CurrentBlockPos);

        blockScripts = objectManager.InitiliazeBlockScripts(blockScale, levelManager.levelBlockType);

        Runner.transform.position = new Vector2(0f, CheckPointPos); //Runner düz sıranın en sonunda başlıyor
        offsetRunnerBtwRoadSprite = RoadSprite.transform.position - Runner.transform.position;

        SetMonsterPosition();

        SetBoreSpeed(); //Set player speed from playerprefs.
    }

    void MoveCheckPoint()
    {
        CheckPointPos = objectManager.MoveCheckPoint(CurrentBlockPos + distBetweenBlock, distBetweenBlock);
    }

    bool CreateNewLevel()
    {
        level_p = GetCurrentLevel();
        levelManager.SetParametersForLevel(level_p, ref isBoostAllowed);

        //platfotmTiles.Clear();
        //platfotmTiles = null;
        //platfotmTiles = new List<GameObject>();

        CurrentBlockPos = CheckPointPos;

        //platfotmTiles = objectManager.SetBlocks(distBetweenBlock, BlockPos, is5Line, ref CurrentBlockPos);

        platfotmTiles = objectManager.SetNewLevelBlocks(distBetweenBlock, BlockPos, is5Line, ref CurrentBlockPos, blockScale);

        //blockScripts = null;

        blockScripts = objectManager.InitiliazeBlockScripts(blockScale, levelManager.levelBlockType);

        SetPlatformVaraibles(CurrentBlockPos,point);

        return true;
    }

    public void PlaceObstacles()
    {
        int lowerYLimit = 10;
        int upperYLimit = 20;

        foreach (GameObject dragon in Shooters)
        {
            if (dragon.transform.position.y < Runner.transform.position.y)
            {
                int r = (Random.Range(0, 3) == 1) ? -1 : 1;

                float x = sizeHandler.GetWallPosition() * r;
                float y = platfotmTiles[blockToSlide].transform.position.y + (Random.Range(lowerYLimit, upperYLimit) * distBetweenBlock);

                Vector3 dragonPos = new Vector3(x, y, 0f);

                dragon.transform.position = dragonPos;
                dragon.transform.rotation = Quaternion.Euler(0f, 0f, 90 * r);

                if (!dragon.activeInHierarchy)
                    dragon.SetActive(true);

                lowerYLimit += 15;
                upperYLimit += 15;
            }
        }
    }

    #endregion


    #region Boost

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

    void InitiateBoost()
    {
        boostTimer = 0f;
        gainedPoint += 1;

        SetBoostPhase(BoostScript.BoostPhase.OnBoost);
        Boost.StartBoost();

        uIGame.GiveMessage(2f, "RUN!!!");
        Debug.LogWarning("BOOST !! at : " + Time.unscaledTime);
        Debug.LogWarning("Distance is : " + distanceBtwRunner);
    }

    void LostBoost()
    {
        SetBoostPhase(BoostScript.BoostPhase.AnimationSlideDown);
        Boost.BoostFinish();

        if (wrongPressedOnBoost)
        {
            wrongPressedOnBoost = false;
        }

        uIGame.GiveMessage(2f, "SLOWDOWN");
        Debug.LogWarning("Boost finished... at : " + Time.unscaledTime);
    }

    bool IsBoostEnded()
    {
        if ((PlayerBehindCamTime > boostTime || PlayerTooBehindCam || wrongPressedOnBoost) && GetCamChase())
        {
            return true;
        }
        else if (boostTimer >= boostTime && !GetCamChase())
            return true;


        return false;
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

    public void SetMonsterSpeed(float to)
    {
        Nightmare.GetComponent<BadThingParticleSystem>().monsterSpeed = to;
    }

    public float GetMonsterSpeed()
    {
        return Nightmare.GetComponent<BadThingParticleSystem>().monsterSpeed;
    }

    public void GameOver()
    {
        game.GameOver();
        uI.GameOver();
    }

    public void LevelPassed()
    {
        game.LevelPassed();
        CreateNewLevel();
        levelLock = false;
        //uI.GameOver();
    }

    private void SetMonsterPosition()
    {
        float distanceBetweenRunner = (!is5Line) ? 8f : 11f;

        Nightmare.transform.position = new Vector3(Runner.transform.position.x, Runner.transform.position.y - distanceBetweenRunner, 0f);
    }

    private int GetCurrentLevel()
    {
        return GameData.GetLevel(); // or Data.GetLevel();
    }

    public bool GetCamChase()
    {
        return cameraChase;
    }

    public bool SetCamChase(bool to)
    {
        return cameraChase = to;
    }

    //This method is used for getting game uı panel. 
    //Since this script attached to OnGamePanel which owned by UI handler, UI handler returns this script from OnGamePanel
    //Can only get uıGameHandler this way because gamobject is disabled from uı handler.
    private UIOnGamePage SetUIGameHandler()
    {
        return uI.GetGamePanel();
    }

    #endregion
}