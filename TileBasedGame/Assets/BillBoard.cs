using UnityEngine;
using System.Collections;

public class BillBoard : MonoBehaviour {

    Transform cam;
	// Use this for initialization
	void Start () {
        cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.LookRotation(cam.position - transform.position);
	}
}
