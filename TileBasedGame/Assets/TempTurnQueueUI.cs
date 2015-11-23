using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TempTurnQueueUI : MonoBehaviour {

    public GameObject buttonPrefab;
    int lrSize = 4;
    int Size
    {
        get
        {
            return lrSize*2+ 1;  //lrSize*2+1
        }
    }

    public List<ButtonMorpher> buttons = new List<ButtonMorpher>();

    bool justMade = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Initialize(List<Unit> units)
    {
        for(int i = 0; i < Size; ++i)
        {
            int index = (i-lrSize) % units.Count;
            ButtonMorpher bm = MakeButton();
            if(i >= lrSize)
                bm.GetComponent<Button>().image.sprite = units[index].icon;
            bm.desiredPos = -Vector3.up*100 + Vector3.right * 100 * (i-lrSize);//Vector3.right*600 + Vector3.up*800 + Vector3.right * 100 * i;
            bm.desiredSize = i == lrSize ? 1 : 0.6f;
            bm.desiredAlpha = 1f - (float)Mathf.Abs(lrSize - i) / (lrSize);
            bm.transform.SetParent(transform);
            buttons.Add(bm);
        }
    }

    public void NewTurn(List<Unit> units)
    {
        if (justMade) {
            justMade = false;
            return;
        }
        Destroy(buttons[0].gameObject);
        buttons.RemoveAt(0);
        ButtonMorpher bm = MakeButton();
        int index = (lrSize) % units.Count;
        bm.GetComponent<Button>().image.sprite = units[index].icon;
        bm.transform.SetParent(transform);
        buttons.Add(bm);
        for(int i = 0; i < buttons.Count; ++i)
        {
            buttons[i].desiredPos = -Vector3.up * 100 + Vector3.right * 100 * (i - lrSize);
            buttons[i].desiredSize = i == lrSize  ? 1 : 0.6f;
            buttons[i].desiredAlpha = 1f - (float)Mathf.Abs(lrSize - i) / (lrSize);
        }
    }

    public ButtonMorpher MakeButton()
    {
        return ((GameObject)Instantiate(buttonPrefab,transform.up*800+transform.right*2000,Quaternion.identity)).GetComponent<ButtonMorpher>();
    }
}
