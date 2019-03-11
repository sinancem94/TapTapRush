using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeHandler
{

    public IEnumerator DynamicCameraMovement(float Upperlimit, float LowerLimit)
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

        yield return new WaitUntil(() => Platform.instance.game.state == GameHandler.GameState.GameRunning);

        while (Platform.instance.game.state == GameHandler.GameState.GameRunning)
        {
            //Debug.Log(Platform.instance.straightRoadLenght);
            if ((Platform.instance.straightRoadLenght) > roadMiddleReferencePoint) //genişlicekse
            {
                //Debug.Log("genişlicek bu kadar : " + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint))));

                // ne kadar genişlemesi gerektiği
                float newCamSize = camSize + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint)));

                //genişle
                if (Camera.main.fieldOfView < newCamSize && Camera.main.fieldOfView < Upperlimit)
                {
                    camSizeChange = ChangeSizeTo(0.1f, camSizeChange);
                    Camera.main.fieldOfView += camSizeChange;
                    //Camera.main.fieldOfView += 0.1f;
                }//daral
                else if (Camera.main.fieldOfView > newCamSize)
                {
                    camSizeChange = ChangeSizeTo(-0.1f, camSizeChange);
                    Camera.main.fieldOfView += camSizeChange;
                    //Camera.main.fieldOfView -= 0.1f;
                }

                //Camera.main.fieldOfView = camSize + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint)));
            }
            else if (Platform.instance.straightRoadLenght <= roadMiddleReferencePoint) // daralcaksa
            {
                //Debug.Log("daralcak  bu kadar : " + (camDiffLowBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint ) / (roadMiddleReferencePoint - roadLengthLowerLimit))));

                // ne kadar daralması gerektiği
                float newCamSize = camSize - (camDiffLowBounds * ((roadMiddleReferencePoint - Platform.instance.straightRoadLenght) / (roadMiddleReferencePoint - roadLengthLowerLimit)));

                //daral
                if (Camera.main.fieldOfView > newCamSize && Camera.main.fieldOfView > LowerLimit)
                {
                    camSizeChange = ChangeSizeTo(-0.1f, camSizeChange);
                    Camera.main.fieldOfView += camSizeChange;
                    //Camera.main.fieldOfView -= 0.1f;
                }//genişle
                else if (Camera.main.fieldOfView < newCamSize)
                {
                    camSizeChange = ChangeSizeTo(0.1f, camSizeChange);
                    Camera.main.fieldOfView += camSizeChange;
                    //Camera.main.fieldOfView += 0.1f;
                }
                //Camera.main.fieldOfView = camSize - (camDiffLowBounds * ((roadMiddleReferencePoint - Platform.instance.straightRoadLenght) / (roadMiddleReferencePoint - roadLengthLowerLimit)));
            }
            /*else // düz 60
            {
                Camera.main.fieldOfView = camSize;
            }*/

            yield return new WaitForSeconds(0.01f);
        }
    }

    //change camera size forward to
    float ChangeSizeTo(float direction, float changeSize)
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
    }

}