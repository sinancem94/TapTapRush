using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingChange : MonoBehaviour
{
    public static PostProcessingChange instance;

    private ColorGrading colorGrading;
	private Vignette vignette;
	private Color vignetteOldColor;

	public float vignetteChange;
	public float vignetteIntensityTimer;
	public float vignetteColorTimer;
	public float tempandTintTimer;
	public Color vignetteNewColor;

    private bool suddenGlowRunning;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

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

        suddenGlowRunning = false;

        vignette.roundness.value = 1; //change value of vignette
		vignetteOldColor = vignette.color.value;
    }

    public void Glow(float rate)
    {
        if (!suddenGlowRunning)
            StartCoroutine(SuddenGlow(rate));
        else
            Debug.LogWarning("Cannor start Sudden Glow. IEnumerator is already running.");
    }

    private IEnumerator SuddenGlow(float glw)
    {
        suddenGlowRunning = true;

        float intens = vignette.intensity.value;
        float round = vignette.roundness.value;

        float limit = intens - glw;

        bool done = false;

        bool decreased = false;

        while(!done)
        {
            yield return new WaitForSeconds(0.01f);
            if (vignette.intensity.value >= limit && !decreased)
            {
                vignette.intensity.value -= 0.01f;
                decreased = (vignette.intensity.value <= limit) ? true : false;
            }
            else
            {
                vignette.intensity.value += 0.01f;
                done = (vignette.intensity.value >= intens) ? true : false;
            }
        }

        vignette.intensity.value = intens;

        suddenGlowRunning = false;
        StopCoroutine(SuddenGlow(glw));
    }


    public IEnumerator BoostVignetteSettings (bool isEntering)
    {
		if (isEntering) {
			colorGrading.temperature.value = 100f;
			colorGrading.tint.value = 60f;
            colorGrading.saturation.value = 10f;

			//vignette.color.value = vignetteNewColor;

			yield return new WaitForSeconds (vignetteColorTimer);

			colorGrading.temperature.value = 50f;
			colorGrading.tint.value = 25f;
			colorGrading.saturation.value = 50f;

			//vignette.color.value = vignetteOldColor;
			while (vignette.intensity.value < 0.35f) {
				vignette.intensity.value += vignetteChange;
				yield return new WaitForSeconds (vignetteIntensityTimer);
			}
		} else {
			colorGrading.temperature.value = 0f;
			colorGrading.tint.value = 0f;
			colorGrading.saturation.value = 0f;

			while (vignette.intensity.value > 0.25f) {
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
