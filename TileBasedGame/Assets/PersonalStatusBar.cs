using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PersonalStatusBar : MonoBehaviour {

    Unit unit;

    public Image hpbar;
    public Image manabar;

	// Use this for initialization
	void Start () {
        unit = transform.parent.GetComponent<Unit>();
	}
	
	// Update is called once per frame
	void Update () {
        hpbar.rectTransform.localScale = new Vector2(unit.curHP / unit.maxHP,1);
        manabar.rectTransform.localScale = new Vector2(unit.curMP / unit.maxMP, 1);
    }
}
