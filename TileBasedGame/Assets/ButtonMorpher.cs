using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonMorpher : MonoBehaviour {

    public float desiredAlpha;
    public Vector2 desiredPos;
    public float desiredSize;

    float cursize;

    Button button;
    RectTransform rt;
    
	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        rt = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        button.image.color = new Color(button.image.color.r, button.image.color.g, button.image.color.b, Mathf.Lerp(button.image.color.a,desiredAlpha,Time.deltaTime));
        rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, desiredPos, Time.deltaTime);
        cursize = button.transform.localScale.x;
        cursize = Mathf.Lerp(cursize, desiredSize, Time.deltaTime);
        transform.localScale = Vector3.one * cursize;
	}
}
