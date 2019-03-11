using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoreBoostEffects : MonoBehaviour
{
	private Vector3 scalerVec;
    private float boreScale;

    // Start is called before the first frame update
    void Start()
    {
        boreScale = this.transform.localScale.x;
		scalerVec = new Vector3(0.5f,0.5f,0f);
    }

	public IEnumerator scaleBore(bool isScaleIncrease){


		if (isScaleIncrease) {
            while (gameObject.transform.localScale.x < boreScale + 2f) {
				gameObject.transform.localScale += scalerVec;
				yield return new WaitForSeconds (.5f);
			}
		} else {
			Debug.Log ("enteredscalebore false");
            while (gameObject.transform.localScale.x > boreScale) {
				gameObject.transform.localScale -= scalerVec;
				yield return new WaitForSeconds (.5f);
			}
		}
	}
}
