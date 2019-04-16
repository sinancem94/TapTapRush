using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour {

	SpriteRenderer bgSpriteRenderer;
	Color bgColor;
	public float changeAmount;
	public float waitTimer;
	 
	void Start () 
    {
        if (Mathf.Approximately(changeAmount, 0))
            changeAmount = 0.01f;
        if (Mathf.Approximately(waitTimer, 0))
            waitTimer = 0.1f;

		bgSpriteRenderer = GetComponent<SpriteRenderer>();
		bgColor = bgSpriteRenderer.color;
		StartCoroutine (WaitAndChangeColor (changeAmount, waitTimer));
	}

	private Color hueChanger(Color colortoChange, float hueIncrease)
    {
		float H, S, V;

		Color.RGBToHSV (colortoChange, out H, out S, out V);
		H = H + hueIncrease;
		return Color.HSVToRGB (H, S, V);
	}

	private  IEnumerator WaitAndChangeColor(float hueIncreaseEnum, float waitTime)
    {
        yield return new WaitUntil(() => Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning);
        while (Platform.instance.game.GetGameState() == GameHandler.GameState.GameRunning) 
        {
			Color newColor;
			bgColor = bgSpriteRenderer.color;
			newColor = hueChanger (bgColor, hueIncreaseEnum);
			yield return new WaitForSeconds (waitTime);
			bgSpriteRenderer.color = newColor;
		}
	}
}
