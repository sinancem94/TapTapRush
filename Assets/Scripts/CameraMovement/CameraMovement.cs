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

        cameraSizeHandler = new CameraSizeHandler();
        isChanging = false;

        CalculateOffset(new Vector3(0f, 3f, -10f));

        //ıf not changed from editor change to this values
        if(Mathf.Approximately(OrthographicLowerSize,0))
            OrthographicLowerSize = 55f;
        if (Mathf.Approximately(OrthographicUpperSize, 0))
            OrthographicUpperSize = 80f;
    }


    void LateUpdate()
    {
        if (!isChanging && Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning)
        {
            StartCamera();
        }
        else if(isChanging && Platform.instance.game.GetGameState() == GameHandler.GameState.GameOver || Platform.instance.game.GetGameState() == GameHandler.GameState.LevelPassed)
        {
            StopCamera();
        }
        //if (!Platform.instance.runner.GetComponent<Runner>().isStrike)
            transform.position = Platform.instance.runner.transform.position + offset;
    }


    public void CalculateOffset(Vector3 pos) //for mode
    {
        offset = pos;
        //offset = Platform.instance.runner.transform.position - pos;
    }

    void StartCamera()
    {
        isChanging = true;
        StartCoroutine(cameraSizeHandler.DynamicCameraMovement(OrthographicUpperSize, OrthographicLowerSize));
    }

    void StopCamera()
    {
        isChanging = false;
        StopCoroutine(cameraSizeHandler.DynamicCameraMovement(OrthographicUpperSize, OrthographicLowerSize));
    }


    /* IEnumerator DynamicCamMovement()
     {
        // if(Platform.instance.platfotmTiles[Platform.instance.blockToSlide].transform.position.y > )
         yield return null;
     }*/

}
