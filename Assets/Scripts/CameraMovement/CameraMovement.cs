using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 offset;

    private DynamicCameraMovement dynamicCamera;

    public float OrthographicLowerSize;
    public float OrthographicUpperSize;

    private bool isChasing;

    private float playerPassedTime;
    private bool didPassedTooMuch;

    private bool didPassedPlayer;

    void Start()
    {
        //ıf not changed from editor change to this values
        if (Mathf.Approximately(OrthographicLowerSize, 0) || Mathf.Approximately(OrthographicUpperSize, 0))
        {
            OrthographicLowerSize = 55f;
            OrthographicUpperSize = 85f;
            Debug.LogWarning("CameraSize parameters are null. Setting defaults. OrthographicLowerSize : " + OrthographicLowerSize + ". OrthographicUpperSize : " + OrthographicUpperSize);
        }
        dynamicCamera = new DynamicCameraMovement(this,Platform.instance.GetBoreSpeed(),OrthographicLowerSize,OrthographicUpperSize);
        CalculateOffset(new Vector3(0f, 3f, -10f));

        transform.position = Platform.instance.Runner.transform.position + offset;

        isChasing = false;
        didPassedPlayer = false;
        didPassedTooMuch = false;
        playerPassedTime = 0f;
    }


    void LateUpdate()
    {
        if (/*!isCamSizeChanging && */Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning)
        {
            dynamicCamera.CamSizeController();


            if (Platform.instance.GetBoostPhase() != BoostScript.BoostPhase.OnBoost || !Platform.instance.GetCamChase()) //Not on boost or camchase mode is closed
            {
                FollowBore();
            }
            else if (Platform.instance.GetBoostPhase() == BoostScript.BoostPhase.OnBoost) // if Chase mode open and OnBoost
            {
                ChaseBore();
                SendPlatformBoostParams();
            }
        }
    }
           
    void ChaseBore()
    {
        if(!isChasing)
        {
            isChasing = true;
            didPassedPlayer = false;
            didPassedTooMuch = false;
            playerPassedTime = 0f;
        }

        float diff = dynamicCamera.CameraChase(Platform.instance.Runner.transform.position.y);

        if(diff >= 0 && didPassedPlayer)
        {
            didPassedPlayer = false;
            playerPassedTime = 0f;
        }
        else if(diff < -Platform.instance.distBetweenBlock)
        {
            didPassedTooMuch = true;
            playerPassedTime += Time.deltaTime;
        }
        else if(diff < 0f)
        {
            if (!didPassedPlayer)
                didPassedPlayer = true;
            playerPassedTime += Time.deltaTime;
        }
    }


    void FollowBore()
    {
        if(isChasing)
        {
            isChasing = false;
        }

        transform.position = dynamicCamera.MoveCamTowardsPos(transform.position, Platform.instance.Runner.transform.position + offset);//Platform.instance.Runner.transform.position + offset;
    }


    void CalculateOffset(Vector3 pos) //for mode
    {
        offset = pos;
    }

    void SendPlatformBoostParams()
    {
        Platform.instance.PlayerBehindCamTime = playerPassedTime;
        Platform.instance.PlayerTooBehindCam = didPassedTooMuch;
    }
}
