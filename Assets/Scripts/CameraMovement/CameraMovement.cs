using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	private Vector3 offset;

	void Start () 
	{
		offset = transform.position - Platform.instance.runner.transform.position;
	}


	void LateUpdate () 
	{
		// Camera.main.orthographicSize += .1f;
		transform.position = Platform.instance.runner.transform.position + offset;
	}

	IEnumerator DynamicCamMovement()
	{
		// if(Platform.instance.platfotmTiles[Platform.instance.blockToSlide].transform.position.y > )

		yield return null;
	}

}