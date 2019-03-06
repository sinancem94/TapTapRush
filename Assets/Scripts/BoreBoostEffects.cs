using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoreBoostEffects : MonoBehaviour
{

	private GameObject bore;
	private Vector3 scalerVec;

    // Start is called before the first frame update
    void Start()
    {
		scalerVec = new Vector3(0.5f,0.5f,0f);
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	public IEnumerator scaleBore(bool isScaleIncrease){
		if (isScaleIncrease) {
			while (gameObject.transform.localScale.x < 3f) {
				gameObject.transform.localScale += scalerVec;
				yield return new WaitForSeconds (.5f);
			}
		} else {
			Debug.Log ("enteredscalebore false");
			while (gameObject.transform.localScale.x > 1f) {
				gameObject.transform.localScale -= scalerVec;
				yield return new WaitForSeconds (.5f);
			}
		}
	}
}
