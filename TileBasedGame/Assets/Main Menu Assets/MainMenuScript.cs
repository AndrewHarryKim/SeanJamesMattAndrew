using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	enum Menu{
		main,
		characterSelect,
		options,
		talentSelect
	}


	public CanvasGroup mainMenu;
	public CanvasGroup optionsMenu;
	public CanvasGroup talentsMenu;
	public Button optionsButton;
	public CanvasGroup quitMenu;
	public Button quitButton;
	public Button startButton;
	public Button selectCharacter;
	public CanvasGroup charSelect;
	public Camera UICamera;
	public Button beginGameButton;
	Menu menuPosition;
	public float menuTransitionSpeed = 4.0f;
	
	// Use this for initialization
	void Start () {
	
		mainMenu = mainMenu.GetComponent<CanvasGroup> ();
		optionsMenu = optionsMenu.GetComponent<CanvasGroup> ();
		quitMenu = quitMenu.GetComponent<CanvasGroup> ();
		charSelect = charSelect.GetComponent<CanvasGroup> ();
		talentsMenu = talentsMenu.GetComponent<CanvasGroup> ();

		startButton = startButton.GetComponent<Button> ();
		optionsButton = optionsButton.GetComponent<Button> ();
		quitButton = quitButton.GetComponent<Button> ();
		selectCharacter = selectCharacter.GetComponent<Button> ();

		UICamera = UICamera.GetComponent<Camera> ();
		beginGameButton = beginGameButton.GetComponent<Button> ();

		menuPosition = Menu.main;

	}

	public void StartPress() {

		mainMenu.alpha = 1;
		charSelect.alpha = 1;
		startButton.enabled = false;
		optionsButton.enabled = false;
		quitButton.enabled = false;

		menuPosition = Menu.characterSelect;


	}

	public void SelectCharacterPress(){

		menuPosition = Menu.talentSelect;
	
	}


	public void ReturnToMainMenu() {
	
		mainMenu.alpha = 1;
		charSelect.alpha = 1;
		startButton.enabled = true;
		optionsButton.enabled = true;
		quitButton.enabled = true;

		menuPosition = Menu.main;
	}

	public void ReturnToCharacterSelect(){
	
	
		menuPosition = Menu.characterSelect;
	
	}

	public void StartGame () {
	
		Application.LoadLevel (1);
	
	}


	public void OptionsPress () {
	
		optionsMenu.alpha = 1;
		startButton.enabled = false;
		optionsButton.enabled = false;
		quitButton.enabled = false;

		menuPosition = Menu.options;
	
	}

	public void OptionsExit () {

		optionsMenu.alpha = 1;
		startButton.enabled = true;
		optionsButton.enabled = true;
		quitButton.enabled = true;

		menuPosition = Menu.main;

	}


	public void QuitPress () {
		
		quitMenu.alpha = 1;
		startButton.enabled = false;
		optionsButton.enabled = false;
		quitButton.enabled = false;

	}

	public void CancelQuit () {
	
		quitMenu.alpha = 0;
		startButton.enabled = true;
		optionsButton.enabled = true;
		quitButton.enabled = true;	

	}

	public void ConfirmQuit () {
	
		Application.Quit ();
	
	}

	void FixedUpdate() {
		Vector3 relativePos;

		switch (menuPosition) {
		case Menu.main:
			relativePos = (mainMenu.transform.position - UICamera.transform.position);
			break;
		case Menu.characterSelect:
			relativePos = (charSelect.transform.position - UICamera.transform.position);
			break;
		case Menu.options:
			relativePos = (optionsMenu.transform.position - UICamera.transform.position);
			break;
		case Menu.talentSelect:
			relativePos = (talentsMenu.transform.position - UICamera.transform.position);
			break;
		default:
			relativePos = (mainMenu.transform.position - UICamera.transform.position);
			break;
		}

		Quaternion rotation = Quaternion.LookRotation(relativePos);
		
		Quaternion current = UICamera.transform.localRotation;
		
		UICamera.transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime * menuTransitionSpeed);


	}


	// Update is called once per frame
	void Update () {
	
	}
}
