using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraMovement
{
    private Coroutine DynamicCameraSizer;

    private MonoBehaviour OwnerCamera;
    private Camera camera;

    private float CamChaseSpeed;

    float camSize = Camera.main.fieldOfView;

    public DynamicCameraMovement(MonoBehaviour owner,float camSpeed,float minCamSize, float maxCamSize)
    {
        OwnerCamera = owner;
        camera = owner.GetComponent<Camera>();
        CamChaseSpeed = camSpeed;

        LowerLimit = minCamSize;
        UpperLimit = maxCamSize;

        camDiffLowBounds = camSize - LowerLimit; // 60 - lower cam size
        camDiffUpperBounds = UpperLimit - camSize; // upper cam size - 60
    }


    #region CamSizeHandling

    float UpperLimit;
    float LowerLimit;

    //Şimdilik 9.5 la falan başlıyor ilk yolun mesafesi. ondan hardcoded değiştirilcek ama sonra
    float roadLenghtUpperLimit = Platform.instance.initialStraightRoadLenght + (Platform.instance.distBetweenBlock * 3);
    float roadLengthLowerLimit = 0f;
    float roadMiddleReferencePoint = Platform.instance.initialStraightRoadLenght;

    float camDiffLowBounds;
    float camDiffUpperBounds;


    public void CamSizeController()
    {
        if ((Platform.instance.straightRoadLenght) > roadMiddleReferencePoint) //genişlicekse
        {
            // ne kadar genişlemesi gerektiği
            float newCamSize = camSize + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint)));
            //Debug.Log("bu kadar olmalı: " + newCamSize + " ama bu kadar : " + camera.fieldOfView);    
            //genişle
            if (camera.fieldOfView < newCamSize - (Platform.instance.distBetweenBlock * 2) && camera.fieldOfView < UpperLimit)
            {
                camera.fieldOfView = MoveCamSizeTowardsNewSize(newCamSize, camera.fieldOfView);
            }
        }
        else if (Platform.instance.straightRoadLenght <= roadMiddleReferencePoint) // daralcaksa
        {
            // ne kadar daralması gerektiği
            float newCamSize = camSize - (camDiffLowBounds * ((roadMiddleReferencePoint - Platform.instance.straightRoadLenght) / (roadMiddleReferencePoint - roadLengthLowerLimit)));

            //daral
            if (camera.fieldOfView > newCamSize + (Platform.instance.distBetweenBlock * 2) && camera.fieldOfView > LowerLimit)
            {
                camera.fieldOfView = MoveCamSizeTowardsNewSize(newCamSize, camera.fieldOfView);
            }
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

        return currentCamSize;
    }
    #endregion

    #region CameraDynamicYAxis

    public Vector3 MoveCamTowardsPos(Vector3 current,Vector3 to)
    {
        float delta = (current.y < to.y) ? (to.y / current.y) * 0.7f : (current.y / to.y) * 0.7f;
        return new Vector3(current.x, Mathf.MoveTowards(current.y, to.y, delta),current.z);
    }

    private float CalculateDifference(float rPos)
    {
        Vector3 minCamPos = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.6f, camera.farClipPlane));
        float minCamY = minCamPos.y;
       //Debug.Log("min cam pos is : " + minCamPos + " Difference speed is : " + (rPos - minCamY) * 10);
        if (rPos > minCamY)
            return (rPos - minCamY) * 10;

        return 0f;
    }

    public float CameraChase(float runnerPos)
    {
        float delta = (int)(Platform.instance.boostTimer / 0.3f) / 12f;

        //Runner straight road lengthle çarpılıyor bu ise initialLengthle (yani, hiç değişmeyen bizim verdiğimiz bir değer camera size ve hızı için.) 
        //Eğer player hızla straight road length arttırsa geçebilir..
        float camSpd = Platform.instance.initialStraightRoadLenght * (CamChaseSpeed + delta + CalculateDifference(runnerPos));

        //Debug.Log("Camspd is " + camSpd);

        //Debug.Log(camSpd);

        OwnerCamera.transform.Translate(0f, camSpd * Time.deltaTime, 0f, Space.World);

        Vector3 maxCamPos = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.06f, camera.farClipPlane));

        /* if(runnerPos < maxCamPos.y)
         {
             Debug.Log("On Boost and stayed behind on Camera");
             return true;
         }

         return false;*/

        return runnerPos - maxCamPos.y;
    }

    #endregion
}