using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraMovement
{
    private Coroutine DynamicCameraSizer;

    private MonoBehaviour MainCam;
    private Camera camera;

    private float CamChaseSpeed;

    public DynamicCameraMovement(MonoBehaviour owner,float camSpeed)
    {
        MainCam = owner;
        camera = owner.GetComponent<Camera>();
        CamChaseSpeed = camSpeed;
    }


    #region CamSizeHandling

    public void StartDynamicSizeHandler(float upperCamSizeLimit, float lowerCamSizeLimit)
    {
        //if (DynamicCameraSizer == null)
            //DynamicCameraSizer = MainCam.StartCoroutine(CamSizeCorountine(upperCamSizeLimit, lowerCamSizeLimit));
    }

    public void StopDynamicSizeHandler()
    {
        //MainCam.StopCoroutine(DynamicCameraSizer);
    }

    public void CamSizeCorountine(float Upperlimit, float LowerLimit)
    {
        float camSize = Camera.main.fieldOfView;

        //Şimdilik 9.5 la falan başlıyor ilk yolun mesafesi. ondan hardcoded değiştirilcek ama sonra
        float roadLenghtUpperLimit = Platform.instance.initialStraightRoadLenght + (Platform.instance.distBetweenBlock * 3);
        float roadLengthLowerLimit = 0f;//Platform.instance.initialStraightRoadLenght - (Platform.instance.distBetweenBlock * 2);
        //Debug.Log(Platform.instance.initialStraightRoadLenght);
        float roadMiddleReferencePoint = Platform.instance.initialStraightRoadLenght;


        float camDiffLowBounds = camSize - LowerLimit; // 60 - lower cam size
        float camDiffUpperBounds = Upperlimit - camSize; // upper cam size - 60
        //Debug.Log(camDiffUpperBounds);

        //yield return new WaitUntil(() => Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning);

      //  while (Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning)
        //{
            //Debug.Log(Platform.instance.straightRoadLenght);
            if ((Platform.instance.straightRoadLenght) > roadMiddleReferencePoint) //genişlicekse
            {
                //Debug.Log("genişlicek bu kadar : " + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint))));

                // ne kadar genişlemesi gerektiği
                float newCamSize = camSize + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint)));

                //genişle
                if (camera.fieldOfView < newCamSize - (Platform.instance.distBetweenBlock * 2) && camera.fieldOfView < Upperlimit)
                {
                    camera.fieldOfView = MoveCamSizeTowardsNewSize(newCamSize, camera.fieldOfView);
                }
            

                //Camera.main.fieldOfView = camSize + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint)));
            }
            else if (Platform.instance.straightRoadLenght <= roadMiddleReferencePoint) // daralcaksa
            {
                //Debug.Log("daralcak  bu kadar : " + (camDiffLowBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint ) / (roadMiddleReferencePoint - roadLengthLowerLimit))));

                // ne kadar daralması gerektiği
                float newCamSize = camSize - (camDiffLowBounds * ((roadMiddleReferencePoint - Platform.instance.straightRoadLenght) / (roadMiddleReferencePoint - roadLengthLowerLimit)));

                //daral
                if (camera.fieldOfView > newCamSize  + (Platform.instance.distBetweenBlock * 2 ) && camera.fieldOfView > LowerLimit)
                {
                    camera.fieldOfView = MoveCamSizeTowardsNewSize(newCamSize, camera.fieldOfView);
                }
             
                //Camera.main.fieldOfView = camSize - (camDiffLowBounds * ((roadMiddleReferencePoint - Platform.instance.straightRoadLenght) / (roadMiddleReferencePoint - roadLengthLowerLimit)));
            }

            //yield return new WaitForSeconds(0.0001f);
       // }
    }

    //Moves camera size forward to desired size
    float MoveCamSizeTowardsNewSize(float newCamSize,float currentCamSize )
    {
        if(!Mathf.Approximately(currentCamSize, newCamSize))
        {
            float delta = (newCamSize > currentCamSize) ? (newCamSize / currentCamSize) * 0.1f : (currentCamSize / newCamSize) *  0.1f;

            return Mathf.MoveTowards(currentCamSize, newCamSize, delta);
        }

        return currentCamSize;
    }
    #endregion

    #region CameraDynamicYAxis

    public float DynamicOffset(float exOffset) //Used when boost starts to make camera chase player
    {
        float dyOf;
        float delta;

        dyOf = Platform.instance.boostTimer;

        return dyOf;
    }

    public void CameraChase()
    {
        float delta = (int)(Platform.instance.boostTimer / 0.5f) / 12f;

        //Runner straight road lengthle çarpılıyor bu ise initialLengthle (yani, hiç değişmeyen bizim verdiğimiz bir değer camera size ve hızı için.) Eğer player hızla straight road length arttırsa geçebilir..
        float camSpd = Platform.instance.initialStraightRoadLenght * (CamChaseSpeed + delta);

        //Debug.Log(camSpd);

        MainCam.transform.Translate(0f, camSpd * Time.deltaTime, 0f, Space.World);


    }

    #endregion
}