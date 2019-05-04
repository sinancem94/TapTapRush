using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesScript : MonoBehaviour
{
    private Animator animator;
    float borePosDiff;
    public GameObject bore;                 // bunu platformla değiştirince sıkıntı cıkıyo

    void Start()
    {
        initializeEyes();
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {

        borePosDiff = bore.transform.position.y - gameObject.transform.position.y;
        animator.SetFloat("BorePosDiff", borePosDiff);
    }


    public void initializeEyes()
    {
        bool isEyePositionRight = (Random.value > 0.5f);                           //creates a random boolean 1 or 0
        float dummyVecx;
        float eyeScaler = Random.Range(-0.2f, 0.1f);

        if (isEyePositionRight)
        {
            dummyVecx = Random.Range(2.5f, 4f);
        }
        else                                                                      //y de 180 derece rotate etmesi lazım
        {
            gameObject.transform.Rotate(0, 180f, 0, Space.World);
            dummyVecx = Random.Range(-2.5f, -4f);
        }

        transform.localScale += new Vector3(eyeScaler, eyeScaler, 0);
        float dummyVecy = Random.Range(1f, 7f);
        Vector3 dummyVec = new Vector3(dummyVecx, dummyVecy, 0);
        gameObject.transform.position = bore.transform.position + dummyVec;
    }
}