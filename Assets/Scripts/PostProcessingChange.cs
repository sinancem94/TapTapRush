using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingChange : MonoBehaviour
{
	private Vignette vignette;

    // Start is called before the first frame update
    void Start()
    {
		PostProcessVolume postProcessVolume = GetComponent<PostProcessVolume> ();  
		if (postProcessVolume.profile == null) {
			enabled = false;
			Debug.Log ("Cant load PostProcess volume");
			return;
		}

		bool foundEffectSettings = postProcessVolume.profile.TryGetSettings<Vignette>(out vignette);
		if(!foundEffectSettings) {
			enabled = false;
			Debug.Log("Cant load PitchTest settings");
			return;
		}

		vignette.roundness.value = 1; //change value of vignette
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


// Change Post Processing via scripting