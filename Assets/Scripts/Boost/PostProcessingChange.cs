using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingChange : MonoBehaviour
{
    private ColorGrading colorGrading;
    private Vignette vignette;
    private Color vignetteOldColor;

    private float vignetteChange;
    private float postProcessingChangeTimer;
   
    //public float tempandTintTimer;
    //public Color vignetteNewColor;


    
    void Start()
    {
        vignetteChange = 0.01f;
        postProcessingChangeTimer = 0.05f;

        PostProcessVolume postProcessVolume = GetComponent<PostProcessVolume>();
        if (postProcessVolume.profile == null)
        {
            enabled = false;
            Debug.Log("Cant load PostProcess volume");
            return;
        }

        bool foundVignetteSettings = postProcessVolume.profile.TryGetSettings<Vignette>(out vignette);                      // vignette i sec
        bool foundColorGradingSettings = postProcessVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);         //color gradingi sec
        if (!foundVignetteSettings)
        {
            enabled = false;
            Debug.Log("Cant load PitchTest settings");
            return;
        }

        vignette.roundness.value = 1;                       //change value of vignette
        vignetteOldColor = vignette.color.value;            //vignette in eski colorunu bul

    }


    public IEnumerator BoostPostProcessingSettings(bool isEntering)
    {                   //boost a girerken ve çıkarkenki vignette ve color grading ayarlamaları burada yapılıyor
        //entering vignette
        if (isEntering)
        {
            /*//color grading settings for starting boost
            colorGrading.temperature.value = 0f;
            colorGrading.tint.value = 0;
            colorGrading.saturation.value = 10f;

            //vignette.color.value = vignetteNewColor;

            yield return new WaitForSeconds(colorGradingTimer);*/

            //color grading settings during boost
            colorGrading.temperature.value = 100f;
            colorGrading.tint.value = 100f;
            colorGrading.saturation.value = -100f;
            colorGrading.contrast.value = 100f;

            //vignette settings during boost
            while (vignette.intensity.value > 0f)
            {
                vignette.intensity.value -= vignetteChange;
                yield return new WaitForSeconds(postProcessingChangeTimer);
            }
        }
        else
        {
            bool isSaturationContinue;
            bool isContrastContinue;
            bool isVignetteContinue;

            //color grading settings after exiting from boost & initial vignette settings
            colorGrading.temperature.value = 0f;
            colorGrading.tint.value = 0f;


            isSaturationContinue = true;
            isContrastContinue = true;
            isVignetteContinue = true;

            while (isVignetteContinue || isSaturationContinue || isContrastContinue) {

                if (colorGrading.saturation.value < -54f)
                {
                    colorGrading.saturation.value += 1f;
                }
                else {isSaturationContinue = false;
                }

                if (colorGrading.contrast.value > 13f)
                {
                    colorGrading.contrast.value -= 1f;
                }
                else { isContrastContinue = false;
                }

                if (vignette.intensity.value < 0.25f) {
                    vignette.intensity.value += vignetteChange;
                }
                else { isVignetteContinue = false;
                }

                yield return new WaitForSeconds(postProcessingChangeTimer);
            }
        }
    }
}


// Change Post Processing via scripting

//temperature 100, tint 60

//vignette basta bu renk 680000

//badthingsparticlesystem radiusu 1 di onu boostta 10 yapabiliriz