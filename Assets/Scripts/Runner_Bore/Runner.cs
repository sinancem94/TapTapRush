using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sadece playeri hareket ettiriyor zamanla hızı artırıyor

public class Runner : MonoBehaviour
{
    public RunnerAnimation BoreAnimationController; //Used by runner for setting speed of bore animation. And by boost script to play boost animations.
    private float speed;

    public float CharacterSpeed;
    public GameObject eye;

    public List<GameObject> eyeList;

    private EyesScript eyesScript;

    void Start()
    {
        eyesScript = FindObjectOfType(typeof(EyesScript)) as EyesScript;
        BoreAnimationController = new RunnerAnimation(this);
        eyeList = ObjectPooler.instance.PooltheObjects(eye, 20);
        StartCoroutine(createEyesDuringPerfect());
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

    public void InitializeEyes()
    {
        GameObject eye = ObjectPooler.instance.GetPooledObject(eyeList);
        if (eye != null)
        {
            bool isEyePositionRight = (Random.value > 0.5f);                           //creates a random boolean 1 or 0
            float dummyVecx;
            float eyeScaler = Random.Range(-0.7f, -0.3f);

            if (isEyePositionRight)
            {
                dummyVecx = Random.Range(2.5f, 4f);
            }
            else                                                                      //y de 180 derece rotate etmesi lazım
            {
                eye.transform.Rotate(0, 180f, 0, Space.World);
                dummyVecx = Random.Range(-2.5f, -4f);
                //perfectText.transform.Rotate(0, 180f, 0, Space.World);
            }

            eye.transform.localScale += new Vector3(eyeScaler, eyeScaler, 0);
            float dummyVecy = Random.Range(1f, 7f);
            Vector3 dummyVec = new Vector3(dummyVecx, dummyVecy, 0);
            eye.transform.position = gameObject.transform.position + dummyVec;            //runner ın pozisyonunu alamıyo burda aq
            eye.SetActive(true);
        }
    }

    public IEnumerator createEyesDuringPerfect()
    {
        while (true)
        {
            InitializeEyes();
            yield return new WaitForSeconds(0.3f);
        }
    }
}