using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerParticleSystemScript : MonoBehaviour
{
    private ParticleSystem runnerParticleSystem;

    private int levelofRunnerParticle;

    private float distance;

    private Color redColor;
    private Color startingDarkBlueColor;
    private Color startingDarkGreyColor;
    private Color blueColor;
    

    // Start is called before the first frame update
    void Start()
    {
        
        runnerParticleSystem = gameObject.GetComponent<ParticleSystem>();
        startingDarkBlueColor = new Color(0, 0.03921569f, 0.294118f);
        startingDarkGreyColor = new Color(0.1698113f, 0.1698113f, 0.1698113f);
        redColor = new Color(0.2597014f, 0, 0.3294118f);
        blueColor = new Color(0.02029152f, 0, 0.3294118f);
                                         
    }

    // Update is called once per frame
    void Update()
    {
        distance = Platform.instance.distanceBtwRunner;
        levelSelector();
    }

    private void levelSelector()
    {
        if (3f < distance && distance < 5f)
            level1RunnerParticle();
        else if (5f < distance && distance < 7f)
            level2RunnerParticle();
        else if (7f < distance && distance < 9f)
            level3RunnerParticle();
        else if (9f < distance && distance < 11f)
            level4RunnerParticle();
        else if (11f < distance)
            level5RunnerParticle();

    }

    #region runnerParticleSystemLevelFeatures
    public void level1RunnerParticle()
    {
        var main = runnerParticleSystem.main;
        var emission = runnerParticleSystem.emission;
        var colorOverLifetime = runnerParticleSystem.colorOverLifetime;

        main.startSpeed = 0.5f;
        emission.rateOverTime = 5f;
        main.startLifetime = 2f;

        Gradient gradient = new Gradient();
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(startingDarkBlueColor, 0.0f), new GradientColorKey(startingDarkGreyColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) });
        colorOverLifetime.color = gradient;
    }

    public void level2RunnerParticle()
    {
        var main = runnerParticleSystem.main;
        var emission = runnerParticleSystem.emission;
        var colorOverLifetime = runnerParticleSystem.colorOverLifetime;

        main.startSpeed = 0.6f;
        emission.rateOverTime = 30f;
        main.startLifetime = 2f;

        Gradient gradient = new Gradient();
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(startingDarkBlueColor, 0.0f), new GradientColorKey(startingDarkGreyColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) });
        colorOverLifetime.color = gradient;
    }

    public void level3RunnerParticle()
    {
        var main = runnerParticleSystem.main;
        var emission = runnerParticleSystem.emission;
        var colorOverLifetime = runnerParticleSystem.colorOverLifetime;

        main.startSpeed = 0.7f;
        emission.rateOverTime = 100f;
        main.startLifetime = 3f;

        Gradient gradient = new Gradient();
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(blueColor, 0.0f), new GradientColorKey(startingDarkGreyColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) });
        colorOverLifetime.color = gradient;
    }

    public void level4RunnerParticle()
    {
        var main = runnerParticleSystem.main;
        var emission = runnerParticleSystem.emission;
        var colorOverLifetime = runnerParticleSystem.colorOverLifetime;

        main.startSpeed = 0.8f;
        emission.rateOverTime = 200f;
        main.startLifetime = 2f;

        Gradient gradient = new Gradient();
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(redColor, 0.0f), new GradientColorKey(startingDarkGreyColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) });
        colorOverLifetime.color = gradient;
    }

    public void level5RunnerParticle()
    {
        var main = runnerParticleSystem.main;
        var emission = runnerParticleSystem.emission;
        var colorOverLifetime = runnerParticleSystem.colorOverLifetime;

        main.startSpeed = 5f;
        emission.rateOverTime = 500f;
        main.startLifetime = 0.5f;

        Gradient gradient = new Gradient();
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(redColor, 0.0f), new GradientColorKey(startingDarkGreyColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) });
        colorOverLifetime.color = gradient;
    }
    #endregion

    
}



// B21D1A       