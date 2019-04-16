using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraMovement
{
    public Coroutine DynamicCameraSizer;

    private MonoBehaviour MainCam;
    private Camera camera;

    public DynamicCameraMovement(MonoBehaviour owner)
    {
        MainCam = owner;
        camera = owner.GetComponent<Camera>();
    }

    public void StartDynamicSizeHandler(float upperCamSizeLimit,float lowerCamSizeLimit)
    {
        if(DynamicCameraSizer == null)
            DynamicCameraSizer = MainCam.StartCoroutine(CamSizeCorountine(upperCamSizeLimit, lowerCamSizeLimit));
    }

    public void StopDynamicSizeHandler()
    {
        MainCam.StopCoroutine(DynamicCameraSizer);
    }



    private IEnumerator CamSizeCorountine(float Upperlimit, float LowerLimit)
    {
        float camSize = Camera.main.fieldOfView;

        //Şimdilik 9.5 la falan başlıyor ilk yolun mesafesi. ondan hardcoded değiştirilcek ama sonra
        float roadLenghtUpperLimit = Platform.instance.initialStraightRoadLenght + (Platform.instance.distBetweenBlock * 3);
        float roadLengthLowerLimit = Platform.instance.initialStraightRoadLenght - (Platform.instance.distBetweenBlock * 2);
        //Debug.Log(Platform.instance.initialStraightRoadLenght);
        float roadMiddleReferencePoint = Platform.instance.initialStraightRoadLenght;

        //float distanceBtwLowBounds = roadMiddleReferencePoint - roadLengthLowerLimit;
        //float distanceBtwUpperBounds = roadLenghtUpperLimit - roadMiddleReferencePoint;

        float camDiffLowBounds = camSize - LowerLimit; // 60 - lower cam size
        float camDiffUpperBounds = Upperlimit - camSize; // upper cam size - 60
        //Debug.Log(camDiffUpperBounds);

        float camSizeChange = 0f;

        yield return new WaitUntil(() => Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning);

        while (Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning)
        {
            //Debug.Log(Platform.instance.straightRoadLenght);
            if ((Platform.instance.straightRoadLenght) > roadMiddleReferencePoint) //genişlicekse
            {
                //Debug.Log("genişlicek bu kadar : " + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint))));

                // ne kadar genişlemesi gerektiği
                float newCamSize = camSize + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint)));

                //genişle
                if (camera.fieldOfView < newCamSize && camera.fieldOfView < Upperlimit)
                {
                    camera.fieldOfView = MoveCamSizeTowardsNewSize(newCamSize, camera.fieldOfView);

                    //camSizeChange = ChangeSizeTo(0.1f, camSizeChange);

                    //camera.fieldOfView += camSizeChange;
                    //Camera.main.fieldOfView += 0.1f;
                }//daral
              /*  else if (Camera.main.fieldOfView > newCamSize)
                {
                    camSizeChange = ChangeSizeTo(-0.1f, camSizeChange);
                    Camera.main.fieldOfView += camSizeChange;
                    //Camera.main.fieldOfView -= 0.1f;
                } */

                //Camera.main.fieldOfView = camSize + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint)));
            }
            else if (Platform.instance.straightRoadLenght <= roadMiddleReferencePoint) // daralcaksa
            {
                //Debug.Log("daralcak  bu kadar : " + (camDiffLowBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint ) / (roadMiddleReferencePoint - roadLengthLowerLimit))));

                // ne kadar daralması gerektiği
                float newCamSize = camSize - (camDiffLowBounds * ((roadMiddleReferencePoint - Platform.instance.straightRoadLenght) / (roadMiddleReferencePoint - roadLengthLowerLimit)));

                //daral
                if (camera.fieldOfView > newCamSize && camera.fieldOfView > LowerLimit)
                {
                    camera.fieldOfView = MoveCamSizeTowardsNewSize(newCamSize, camera.fieldOfView);

                    //camSizeChange = ChangeSizeTo(-0.1f, camSizeChange);

                    //camera.fieldOfView += camSizeChange;
                    //Camera.main.fieldOfView -= 0.1f;
                }//genişle
             /*   else if (Camera.main.fieldOfView < newCamSize)
                {
                    camSizeChange = ChangeSizeTo(0.1f, camSizeChange);
                    Camera.main.fieldOfView += camSizeChange;
                    //Camera.main.fieldOfView += 0.1f;
                }*/
                //Camera.main.fieldOfView = camSize - (camDiffLowBounds * ((roadMiddleReferencePoint - Platform.instance.straightRoadLenght) / (roadMiddleReferencePoint - roadLengthLowerLimit)));
            }
            /*else // düz 60
            {
                Camera.main.fieldOfView = camSize;
            }*/

            yield return new WaitForSeconds(0.001f);
        }
    }

    //Moves camera size forward to desired size
    float MoveCamSizeTowardsNewSize(float newCamSize,float currentCamSize )
    {
        if(!Mathf.Approximately(currentCamSize, newCamSize))
        {
            float delta = (newCamSize > currentCamSize) ? (newCamSize / currentCamSize) * 0.1f : (currentCamSize / newCamSize) *  0.1f;

            return Mathf.MoveTowards(currentCamSize, newCamSize, delta);
        }

        return 0;
    }

   /* float ChangeSizeTo(float direction, float changeSize)
    {

        if (direction > 0)
        {
            changeSize = (changeSize < direction) ? changeSize + 0.02f : changeSize = 0.15f;
        }
        else if (direction < 0)
        {
            changeSize = (changeSize > direction) ? changeSize - 0.02f : changeSize = -0.15f;
        }

        return changeSize;
    }*/

}