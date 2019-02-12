using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    
    private Vector3 offset;

    private Vector3 initPos; //for mode

    private CameraSizeHandler cameraSizeHandler;

    public float OrthographicUpperSize;
    public float OrthographicLowerSize;

    void Start ()
    {
        initPos = Platform.instance.runner.transform.position; //for mode
        offset = initPos - new Vector3(0f, Platform.instance.runner.transform.position.y - 3f,  10f); //transform.position - initPos;

        cameraSizeHandler = new CameraSizeHandler();

        StartCoroutine(cameraSizeHandler.DynamicCameraMovement(OrthographicUpperSize,OrthographicLowerSize));
	}
	

	void LateUpdate () 
    {
       // Camera.main.orthographicSize += .1f;
        transform.position = Platform.instance.runner.transform.position + offset;
	}

    public void CalculateOffset(Vector3 pos) //for mode
    {
        offset = pos - initPos;
    }

   /* IEnumerator DynamicCamMovement()
    {
       // if(Platform.instance.platfotmTiles[Platform.instance.blockToSlide].transform.position.y > )

        yield return null;
    }*/

}
