using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Vector3 offset;

    private Vector3 initPos; //for mode

    private CameraSizeHandler cameraSizeHandler;
    private bool isChanging;

    public float OrthographicUpperSize;
    public float OrthographicLowerSize;

    void Start()
    {
        initPos = Platform.instance.runner.transform.position; //for mode
        offset = initPos - new Vector3(0f, Platform.instance.runner.transform.position.y - 3f, 10f); //transform.position - initPos;
        cameraSizeHandler = new CameraSizeHandler();
        isChanging = false;
        Platform.instance.ChangeAngle(); //set cam and blocks for angle. for mode
                                         //StartCoroutine(cameraSizeHandler.DynamicCameraMovement(OrthographicUpperSize,OrthographicLowerSize));
    }


    void LateUpdate()
    {
        if (!isChanging && Platform.instance.game.state == GameHandler.GameState.GameRunning)
        {
            StartCamera();
        }

        //if (!Platform.instance.runner.GetComponent<Runner>().isStrike)
        transform.position = Platform.instance.runner.transform.position + offset;
    }

    public void CalculateOffset(Vector3 pos) //for mode
    {
        offset = -pos + Platform.instance.runner.transform.position;
    }

    public Vector3 GetOffset()
    {
        return offset;
    }

    void StartCamera()
    {
        isChanging = true;
        StartCoroutine(cameraSizeHandler.DynamicCameraMovement(OrthographicUpperSize, OrthographicLowerSize));
    }


    /* IEnumerator DynamicCamMovement()
     {
        // if(Platform.instance.platfotmTiles[Platform.instance.blockToSlide].transform.position.y > )
         yield return null;
     }*/

}
