using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public float speed;
    public float Boundary;

    // Use this for initialization
    private void Start()
    {
        if (Platform.instance)
            Boundary = Platform.instance.sizeHandler.GetWallPosition() + 0.1f;
    }

    void OnEnable()
    {
        //transform.position = transform.parent.position;
        speed = -4f;

        if (transform.position.x < 0)
        {  
            transform.rotation = Quaternion.Euler(0,0,180);
        }
    }

    private void LateUpdate()
    {
        if (transform.position.x < Boundary && transform.position.x > -Boundary) 
        {
            transform.Translate(speed * Time.deltaTime, 0f, 0f);
        }
        else
        {
            gameObject.SetActive(false);
        }
       
    }
}
