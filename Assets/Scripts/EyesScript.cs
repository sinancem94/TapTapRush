using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyesScript : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer eye_renderer;
    float borePosDiff;
    Quaternion quaternion;
    public Text perfectText;  //bunu da normal getcomponentinchildrenla almak lazım...

    private void Awake()
    {
        perfectText = gameObject.GetComponentInChildren<Text>();
        quaternion = transform.rotation;
    }

    private void OnEnable()
    {
        InitializeEye();
    }

    void Start()            //eğer buraya yeni bişey eklemek gerekirse onenabled yapmak lazım...
    {
        // initializeEyes();
        eye_renderer = GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        animator.SetFloat("BorePosDiff", Platform.instance.distanceBtwRunner);          //burada sıkıntı var sanırım doğru alamıyor distancebtwrunner ı;;;

        if (!eye_renderer.isVisible)
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        OnBecameInvisible();
    }

    private void OnBecameInvisible()
    {
        transform.rotation = quaternion;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void InitializeEye()
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
            this.transform.Rotate(0, 180f, 0, Space.World);
            dummyVecx = Random.Range(-2.5f, -4f);
            //perfectText.transform.Rotate(0, 180f, 0, Space.World);
        }

        this.transform.localScale += new Vector3(eyeScaler, eyeScaler, 0);
        float dummyVecy = Random.Range(1f, 7f);
        Vector3 dummyVec = new Vector3(dummyVecx, dummyVecy, 0);
        this.transform.position = gameObject.transform.position + dummyVec;            //runner ın pozisyonunu alamıyo burda aq
        this.gameObject.SetActive(true);

    }
}