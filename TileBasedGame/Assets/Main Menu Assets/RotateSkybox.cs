using UnityEngine;
using System.Collections;

public class RotateSkybox : MonoBehaviour {

	public float speed = 1.5f;

	void Update () {	
		transform.Rotate (Vector3.up, speed * 0.75f * Time.deltaTime);
		transform.Rotate (Vector3.left, speed * Time.deltaTime);
	}
}
