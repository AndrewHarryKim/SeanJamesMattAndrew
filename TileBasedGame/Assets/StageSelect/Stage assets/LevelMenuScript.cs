using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMenuScript : MonoBehaviour {

	enum Level{
		one, 
		two,
		three
	}
	public string level1Name = "Level 1";
	public string level1Info = "DOOOOOM!!";	
	public string level2Name = "Level 2";
	public string level2Info = "DOOOOOM!!";	
	public string level3Name = "Level 3";
	public string level3Info = "DOOOOOM!!";

	public Canvas infoMenu;
	public Button startBtn;
	public Button level1Btn;
	public Button level2Btn;
	public Button level3Btn;
	public Text levelSelectText;
	public Text levelInfoText;
	Level currLevel;


	// Use this for initialization
	void Start () {
		infoMenu = infoMenu.GetComponent<Canvas>();
		startBtn = startBtn.GetComponent<Button>();
		level1Btn = level1Btn.GetComponent<Button>();
		level2Btn = level2Btn.GetComponent<Button>();
		level3Btn = level3Btn.GetComponent<Button>();
		levelSelectText = levelSelectText.GetComponent<Text>();
		levelInfoText = levelInfoText.GetComponent<Text>();
		currLevel = Level.one;
		Level1Press ();
		infoMenu.enabled = true;
		startBtn.enabled = true;
	}

	public void Level1Press(){
		level1Btn.enabled = false;
		level2Btn.enabled = true;
		level3Btn.enabled = true;
		levelSelectText.text = level1Name;
		levelInfoText.text = level1Info;
	}

	public void Level2Press(){
		level1Btn.enabled = true;
		level2Btn.enabled = false;
		level3Btn.enabled = true;
		levelSelectText.text = level2Name;
		levelInfoText.text = level2Info;
	}

	public void Level3Press(){
		level1Btn.enabled = true;
		level2Btn.enabled = true;
		level3Btn.enabled = false;
		levelSelectText.text = level3Name;
		levelInfoText.text = level3Info;
	}

	public void startLevel(){
		Debug.Log ("Start Game!");
		Application.LoadLevel (1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
