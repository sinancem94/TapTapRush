using System.Collections;
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
	public float vignetteColorTimer;
	public float tempandTintTimer;
	public Color vignetteNewColor;


    // Start is called before the first frame update
    void Start()
    {
		PostProcessVolume postProcessVolume = GetComponent<PostProcessVolume> ();  
		if (postProcessVolume.profile == null) {
			enabled = false;
			Debug.Log ("Cant load PostProcess volume");
			return;
		}

		bool foundVignetteSettings = postProcessVolume.profile.TryGetSettings<Vignette>(out vignette);
		bool foundColorGradingSettings = postProcessVolume.profile.TryGetSettings<ColorGrading> (out colorGrading);
		if(!foundVignetteSettings) {
			enabled = false;
			Debug.Log("Cant load PitchTest settings");
			return;
		}

		vignette.roundness.value = 1; //change value of vignette
		vignetteOldColor = vignette.color.value;

    }


	public IEnumerator BoostVignetteSettings (bool isEntering){
		Debug.Log ("enter vignette");
		if (isEntering) {
			colorGrading.temperature.value = 100f;
			colorGrading.tint.value = 60f;

			vignette.color.value = vignetteNewColor;

			yield return new WaitForSeconds (vignetteColorTimer);

			colorGrading.temperature.value = 50f;
			colorGrading.tint.value = 25f;
			colorGrading.saturation.value = 100f;

			vignette.color.value = vignetteOldColor;
			while (vignette.intensity.value < .3f) {
				vignette.intensity.value += vignetteChange;
				yield return new WaitForSeconds (vignetteIntensityTimer);
			}
		} else {
			colorGrading.temperature.value = 0f;
			colorGrading.tint.value = 0f;
			colorGrading.saturation.value = -54f;

			while (vignette.intensity.value > 0.2f) {
				vignette.intensity.value -= vignetteChange;
				yield return new WaitForSeconds (vignetteIntensityTimer);
			}
		}
	}
}


// Change Post Processing via scripting

//temperature 100, tint 60

//vignette basta bu renk 680000

//badthingsparticlesystem radiusu 1 di onu boostta 10 yapabiliriz
