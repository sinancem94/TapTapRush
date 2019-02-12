using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeHandler : SizeHandler {

    public IEnumerator DynamicCameraMovement(float Upperlimit,float LowerLimit)
    {
        float camSize = Camera.main.fieldOfView;

        //Şimdilik 9.5 la falan başlıyor ilk yolun mesafesi. ondan hardcoded değiştirilcek ama sonra
        float roadLenghtUpperLimit = Platform.instance.initialStraightRoadLenght + 4f;
        float roadLengthLowerLimit = Platform.instance.initialStraightRoadLenght - 4f;

        //Debug.Log(Platform.instance.initialStraightRoadLenght);
        float roadMiddleReferencePoint = Platform.instance.initialStraightRoadLenght;

        //float distanceBtwLowBounds = roadMiddleReferencePoint - roadLengthLowerLimit;
        //float distanceBtwUpperBounds = roadLenghtUpperLimit - roadMiddleReferencePoint;

        float camDiffLowBounds = camSize - LowerLimit; // 60 - lower cam size
        float camDiffUpperBounds = Upperlimit - camSize; // upper cam size - 60
        //Debug.Log(camDiffUpperBounds);

        float camSizeChange = 0f;

        yield return new WaitUntil(() => Platform.instance.game.state == GameHandler.GameState.GameRunning);

        while(Platform.instance.game.state == GameHandler.GameState.GameRunning)
        {
            //Debug.Log(Platform.instance.straightRoadLenght);
            if ((Platform.instance.straightRoadLenght) > roadMiddleReferencePoint + 1f) //genişlicekse
            {
                //Debug.Log("genişlicek bu kadar : " + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint))));

                // ne kadar genişlemesi gerektiği
                float newCamSize  = camSize + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint)));

                //ona doğru git
                if(Camera.main.fieldOfView <  newCamSize)
                {
                    camSizeChange = ChangeSizeTo(0.1f, camSizeChange);
                    //Camera.main.fieldOfView += camSizeChange;
                    Camera.main.fieldOfView += 0.1f;
                }
                else if(Camera.main.fieldOfView > newCamSize + 0.5f)
                {
                    camSizeChange = ChangeSizeTo(-0.1f, camSizeChange);
                    //Camera.main.fieldOfView += camSizeChange;
                    Camera.main.fieldOfView -= 0.1f;
                }

                //Camera.main.fieldOfView = camSize + (camDiffUpperBounds * ((Platform.instance.straightRoadLenght - roadMiddleReferencePoint) / (roadLenghtUpperLimit - roadMiddleReferencePoint)));
            }
            else if (Platform.instance.straightRoadLenght <= roadMiddleReferencePoint - 1f) // daralcaksa
            {
                //Debug.Log("daralcak  bu kadar : " + (camDiffLowBounds * ((roadMiddleReferencePoint - Platform.instance.straightRoadLenght) / (roadMiddleReferencePoint - roadLengthLowerLimit))));

                // ne kadar daralması gerektiği
                float newCamSize = camSize - (camDiffLowBounds * ((roadMiddleReferencePoint - Platform.instance.straightRoadLenght) / (roadMiddleReferencePoint - roadLengthLowerLimit)));

                //ona doğru git
                if(Camera.main.fieldOfView > newCamSize)
                {
                    camSizeChange = ChangeSizeTo(-0.1f, camSizeChange);
                    //Camera.main.fieldOfView += camSizeChange;
                    Camera.main.fieldOfView -= 0.1f;
                }
                else if(Camera.main.fieldOfView < newCamSize -0.5f)
                {
                    camSizeChange = ChangeSizeTo(0.1f, camSizeChange);
                    //Camera.main.fieldOfView += camSizeChange;
                    Camera.main.fieldOfView += 0.1f;
                }

                //Camera.main.fieldOfView = camSize - (camDiffLowBounds * ((roadMiddleReferencePoint - Platform.instance.straightRoadLenght) / (roadMiddleReferencePoint - roadLengthLowerLimit)));
            }
            /*else // düz 60
            {
                Camera.main.fieldOfView = camSize;
            }*/

            yield return new WaitForSeconds(0.001f);
        }
    }

    // change camera size forward to
        float ChangeSizeTo(float direction, float changeSize)
        {


            if (direction > 0)
            {
                changeSize = (changeSize < direction) ? changeSize + 0.01f : changeSize = .01f;
            }
            else if(direction < 0)
            {
                changeSize = (changeSize > direction) ? changeSize - 0.01f : changeSize = -0.1f;
            }
            
            return changeSize;
        } 

}
