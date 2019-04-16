﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingChange : MonoBehaviour
{
    private ColorGrading colorGrading;
    private Vignette vignette;
    private Color vignetteOldColor;

    public float vignetteChange;
    public float vignetteIntensityTimer;
    public float colorGradingTimer;
    //public float tempandTintTimer;
    //public Color vignetteNewColor;


    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log("enter vignette");
        if (isEntering)
        {
            //color grading settings for starting boost
            colorGrading.temperature.value = 100f;
            colorGrading.tint.value = 60f;
            colorGrading.saturation.value = 10f;

            //vignette.color.value = vignetteNewColor;

            yield return new WaitForSeconds(colorGradingTimer);

            //color grading settings during boost
            colorGrading.temperature.value = 100f;
            colorGrading.tint.value = 100f;
            colorGrading.saturation.value = -100f;
            colorGrading.contrast.value = 100f;

            //vignette settings during boost
            while (vignette.intensity.value > 0f)
            {
                vignette.intensity.value -= vignetteChange;
                yield return new WaitForSeconds(vignetteIntensityTimer);
            }
        }
        else
        {
            //color grading settings after exiting from boost & initial vignette settings
            colorGrading.temperature.value = 0f;
            colorGrading.tint.value = 0f;
            colorGrading.saturation.value = 0f;

            //vignette settings after exiting from boost & initial vignette settings
            while (vignette.intensity.value < 0.25f)
            {
                vignette.intensity.value += vignetteChange;
                yield return new WaitForSeconds(vignetteIntensityTimer);
            }
        }
    }
}


// Change Post Processing via scripting

//temperature 100, tint 60

//vignette basta bu renk 680000

//badthingsparticlesystem radiusu 1 di onu boostta 10 yapabiliriz