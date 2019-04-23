using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Vector3 offset;

    private DynamicCameraMovement dynamicCamera;
    private bool isCamSizeChanging;

    public float OrthographicUpperSize;
    public float OrthographicLowerSize;

    void Start()
    {
        dynamicCamera = new DynamicCameraMovement(this,Platform.instance.GetBoreSpeed());

        isCamSizeChanging = false;
        CalculateOffset(new Vector3(0f, 3f, -10f));

        //ıf not changed from editor change to this values
        if(Mathf.Approximately(OrthographicLowerSize,0))
            OrthographicLowerSize = 55f;
        if (Mathf.Approximately(OrthographicUpperSize, 0))
            OrthographicUpperSize = 80f;
    }


    void LateUpdate()
    {
        if (!isCamSizeChanging && Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning)
        {
            StartCamera();
        }
        else if(isCamSizeChanging && Platform.instance.game.GetGameState() == GameHandler.GameState.GameOver || Platform.instance.game.GetGameState() == GameHandler.GameState.LevelPassed)
        {
            StopCamera();
        }

        if (Platform.instance.GetBoostPhase() != BoostScript.BoostPhase.PlayerRunning)
            transform.position = Platform.instance.Runner.transform.position + offset;
        else
            dynamicCamera.CameraChase();

    }


    void CalculateOffset(Vector3 pos) //for mode
    {
        offset = pos;
        //offset = Platform.instance.runner.transform.position - pos;
    }

    void StartCamera()
    {
        isCamSizeChanging = true;
        dynamicCamera.StartDynamicSizeHandler(OrthographicUpperSize, OrthographicLowerSize);
        //StartCoroutine(dynamicCamera.DynamicCameraSizer(OrthographicUpperSize, OrthographicLowerSize));
    }

    void StopCamera()
    {
        isCamSizeChanging = false;
        dynamicCamera.StopDynamicSizeHandler();
        //StopCoroutine(dynamicCamera.DynamicCameraSizer(OrthographicUpperSize, OrthographicLowerSize));
    }


    /* IEnumerator DynamicCamMovement()
     {
        // if(Platform.instance.platfotmTiles[Platform.instance.blockToSlide].transform.position.y > )
         yield return null;
     }*/

}
