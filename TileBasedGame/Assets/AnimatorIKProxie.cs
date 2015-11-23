using UnityEngine;
using System.Collections;

public class AnimatorIKProxie : MonoBehaviour {

    GameObject lookObject = null;
    Vector3 lookPos = Vector3.zero;
    bool looking = false;
    bool lookingObject = false;


    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (lookingObject && !lookObject)
            StopLooking();

    
	}

    public void StopLooking()
    {
        looking = lookingObject = false;
    }

    public void LookAt(Vector3 pos)
    {
        lookPos = pos;
        looking = true;
        lookingObject = false;
    }

    public void LookAt(GameObject go, Vector3 offset)
    {
        lookObject = go;
        lookPos = offset;
        looking = lookingObject = true;
    }

    public void LookAt(GameObject go)
    {
        LookAt(go, Vector3.zero);
    }

    void OnAnimatorIK()
    {
        if (lookObject)
            anim.SetLookAtPosition(lookObject.transform.position+lookPos);
        else
            anim.SetLookAtPosition(lookPos);
        anim.SetLookAtWeight(looking ? 0.5f : 0f);
    }
}
