using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelBtns : MonoBehaviour {

	public string level1Name = "Level 1";
	public string level1Info = "DOOOOOM!!";

	public string level2Name = "Level 2";
	public string level2Info = "DOOOOOM!!";

	public string level3Name = "Level 3";
	public string level3Info = "DOOOOOM!!";

	
	public Button level1;
	public Button level2;
	public Button level3;

	public Text levelNameText = null;
	public Text levelInfoText = null;

	enum Level{
		one,
		two, 
		three
	}

	Level levelPosition;

	void Start () {
		levelNameText = levelNameText.GetComponent<Text> ();
		levelInfoText = levelInfoText.GetComponent<Text> ();

		level1 = level1.GetComponent<Button> ();
		level2 = level2.GetComponent<Button> ();
		level3 = level3.GetComponent<Button> ();

		level1.enabled = true;
		level2.enabled = true;
		level3.enabled = true;
	}

	public void level1Pressed() {
		Debug.Log ("button 1 press caught");

		levelPosition = Level.one;
		level1.enabled = false;
		level2.enabled = true;
		level3.enabled = true;
	}

	public void level2Pressed() {
		Debug.Log ("button 2 press caught");

		levelPosition = Level.two;
		level2.enabled = false;
		level1.enabled = true;
		level3.enabled = true;
	}

	public void level3Pressed() {
		Debug.Log ("button 3 press caught");

		levelPosition = Level.three;
		level3.enabled = false;
		level2.enabled = true;
		level1.enabled = true;
	}

	void Update() {
		Debug.Log (levelPosition);
		switch (levelPosition) {
		case Level.one:
				levelNameText.text = level1Name;
				levelInfoText.text = level1Info;
				break;
		case Level.two:
				levelNameText.text = level2Name;
				levelInfoText.text = level2Info;
				break;
		case Level.three:
				levelNameText.text = level3Name;
				levelInfoText.text = level3Info;
				break;
		}
	}
}
