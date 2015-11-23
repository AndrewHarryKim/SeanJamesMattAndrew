using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class TempActionBarUI : MonoBehaviour {

    public Unit unit;
    public Button[] buttons;
    public Sprite sprite;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void LoadUnit(Unit unit)
    {
        this.unit = unit;
        //button.image.sprite = sprite;
        int i = 0;
        for (; i < unit.skillContainers.Count; ++i)
        {
            SetButton(i, unit.skillContainers[i]);
        }
        for (; i < buttons.Length; ++i)
        {
            BlackOut(i);
        }

    }

    void SetButton(int index, SkillContainer sc)
    {
        if (index >= buttons.Length)
            return;
        buttons[index].enabled = sc.IsCastable;
        buttons[index].image.color = sc.IsCastable ? Color.white : Color.grey;
        buttons[index].image.sprite = sc.skill.icon;
        buttons[index].transform.GetChild(0).GetComponent<Text>().text = sc.skill.name+"\n"+sc.skill.manaCost(unit);
        buttons[index].onClick.RemoveAllListeners();
        buttons[index].onClick.AddListener(() => {
            unit.StopAimingSkill();
            unit.SelectSkill(index);
            buttons[index].image.color = Color.white;
        });

        //set cooldown here
        //sc.cooldownproportion or whatever i called it gives you the amount to fill.
        buttons[index].transform.GetChild(1).GetComponent<Image>().fillAmount = 1 - sc.CooldownProportion;
        //buttons[index].transform.GetChild(0).GetComponent<Text>().color = sc.skill.CanCast(sc.user) ? Color.white : Color.red;
    }

    void BlackOut(int index)
    {
        if (index >= buttons.Length)
            return;
        buttons[index].enabled = false;
        buttons[index].image.color = Color.black;
        buttons[index].image.sprite = null;
        buttons[index].transform.GetChild(0).GetComponent<Text>().text = "";
        buttons[index].onClick.RemoveAllListeners();
        buttons[index].transform.GetChild(1).GetComponent<Image>().fillAmount = 0;
    }
}
