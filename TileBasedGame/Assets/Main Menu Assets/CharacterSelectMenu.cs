using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectMenu : MonoBehaviour {

	enum Chars {
		tank,
		medic,
		assassin,
		tech
	}

	public CanvasGroup charMenu, talentMenu; //character selection screen, talent selection screen: grouped for easy enable/disable
	public Text charDescription;			 //character description shown to player before they choose to add to team/select talents
	string tankDescription, medicDescription, assassinDescription, techDescription; //individual class descriptions, pull from class factories instead?
	public Image charImage;					//current character description image
	public Button tankIcon, medicIcon, assassinIcon, techIcon, selectCharacter, beginGame;
	public Image tankImage, medicImage, assassinImage, techImage;

	List<Button> talentButtons;				 							//store the below panels for easy access
	public Button talent1, talent2, talent3, talent4, talent5, talent6;	//the panel buttons containing the talent images and descriptions

	List<Text> talentDescriptionsList;
	public Text talentDescription1, talentDescription2, talentDescription3, talentDescription4, talentDescription5, talentDescription6; 

	List<Image> talentImages;
	public Image talentImage1, talentImage2, talentImage3, talentImage4, talentImage5, talentImage6; 

	List<ClassFactory> teamMembers;			 			//maintain a list of the team members for display

	List<Image> teamMemberIcons;						//the images showing the current team selection
	public Image teamMember1, teamMember2, teamMember3; //the individual image elements, placed in teamMemberIcons

	Chars currentChar;									//maintains current character selection from the enum

	// Use this for initialization
	void Start () {
	
		charMenu = charMenu.GetComponent<CanvasGroup> ();
		charDescription = charDescription.GetComponent<Text> ();
		charImage = charImage.GetComponent<Image> ();

		tankIcon = tankIcon.GetComponent<Button> ();
		tankImage = tankImage.GetComponent<Image> ();
		tankImage.sprite = new TankFactory ().image;

		medicIcon = medicIcon.GetComponent<Button> ();
		medicImage = medicImage.GetComponent<Image> ();
		medicImage.sprite = new MedicFactory ().image;

		assassinIcon = assassinIcon.GetComponent<Button> ();
		assassinImage = assassinImage.GetComponent<Image> ();
		assassinImage.sprite = new AssassinFactory ().image;

		techIcon = techIcon.GetComponent<Button> ();
		techImage = techImage.GetComponent<Image> ();
		techImage.sprite = new TechFactory ().image;

		selectCharacter = selectCharacter.GetComponent<Button> ();
		beginGame = beginGame.GetComponent<Button> ();
		beginGame.enabled = false;

		teamMembers = new List<ClassFactory>();
		teamMemberIcons = new List<Image> ();

		teamMember1 = teamMember1.GetComponent<Image> ();
		teamMember2 = teamMember2.GetComponent<Image> ();
		teamMember3 = teamMember3.GetComponent<Image> ();

		teamMemberIcons.Add (teamMember1);
		teamMemberIcons.Add (teamMember2);
		teamMemberIcons.Add (teamMember3);

		talent1 = talent1.GetComponent<Button> ();
		talent2 = talent2.GetComponent<Button> ();
		talent3 = talent3.GetComponent<Button> ();
		talent4 = talent4.GetComponent<Button> ();
		talent5 = talent5.GetComponent<Button> ();
		talent6 = talent6.GetComponent<Button> ();

		talentButtons = new List<Button> ();

		talentButtons.Add (talent1);
		talentButtons.Add (talent2);
		talentButtons.Add (talent3);
		talentButtons.Add (talent4);
		talentButtons.Add (talent5);
		talentButtons.Add (talent6);

		talentDescription1 = talentDescription1.GetComponent<Text> ();
		talentDescription2 = talentDescription2.GetComponent<Text> ();
		talentDescription3 = talentDescription3.GetComponent<Text> ();
		talentDescription4 = talentDescription4.GetComponent<Text> ();
		talentDescription5 = talentDescription5.GetComponent<Text> ();
		talentDescription6 = talentDescription6.GetComponent<Text> ();

		talentDescriptionsList = new List<Text> ();

		talentDescriptionsList.Add (talentDescription1);
		talentDescriptionsList.Add (talentDescription2);
		talentDescriptionsList.Add (talentDescription3);
		talentDescriptionsList.Add (talentDescription4);
		talentDescriptionsList.Add (talentDescription5);
		talentDescriptionsList.Add (talentDescription6);

		talentImage1 = talentImage1.GetComponent<Image> ();
		talentImage2 = talentImage2.GetComponent<Image> ();
		talentImage3 = talentImage3.GetComponent<Image> ();
		talentImage4 = talentImage4.GetComponent<Image> ();
		talentImage5 = talentImage5.GetComponent<Image> ();
		talentImage6 = talentImage6.GetComponent<Image> ();

		talentImages = new List<Image>();

		talentImages.Add (talentImage1);
		talentImages.Add (talentImage2);
		talentImages.Add (talentImage3);
		talentImages.Add (talentImage4);
		talentImages.Add (talentImage5);
		talentImages.Add (talentImage6);


		currentChar = Chars.tank;

		tankDescription = "The Tank is a rough, tough melee brawler. He is capable of taking big hits as well as dishing them out.";
		medicDescription = "The Medic is";
		assassinDescription = "The Assassin is";
		techDescription = "The Tech is";
	}


	//not sure how to combine these into a single function with a switch depending on which button was pressed
	//would probably be a much cleaner implementation

	public void selectTalentOne(){
		teamMembers [teamMembers.Count - 1].talentOptions [0].toggle = true;
		teamMembers [teamMembers.Count - 1].talentOptions [1].toggle = false;

		talentButtons [0].enabled = false;
		talentButtons [1].enabled = true;
	}

	public void selectTalentTwo(){
		teamMembers [teamMembers.Count - 1].talentOptions [1].toggle = true;
		teamMembers [teamMembers.Count - 1].talentOptions [0].toggle = false;

		talentButtons [1].enabled = false;
		talentButtons [0].enabled = true;
	}

	public void selectTalentThree(){
		teamMembers [teamMembers.Count - 1].talentOptions [2].toggle = true;
		teamMembers [teamMembers.Count - 1].talentOptions [3].toggle = false;

		talentButtons [2].enabled = false;
		talentButtons [3].enabled = true;
	}
	
	public void selectTalentFour(){
		teamMembers [teamMembers.Count - 1].talentOptions [3].toggle = true;
		teamMembers [teamMembers.Count - 1].talentOptions [2].toggle = false;

		talentButtons [3].enabled = false;
		talentButtons [2].enabled = true;
	}

	public void selectTalentFive(){
		teamMembers [teamMembers.Count - 1].talentOptions [4].toggle = true;
		teamMembers [teamMembers.Count - 1].talentOptions [5].toggle = false;

		talentButtons [4].enabled = false;
		talentButtons [5].enabled = true;
	}
	
	public void selectTalentSix(){
		teamMembers [teamMembers.Count - 1].talentOptions [5].toggle = true;
		teamMembers [teamMembers.Count - 1].talentOptions [4].toggle = false;

		talentButtons [5].enabled = false;
		talentButtons [4].enabled = true;
	}


	public void Begin(){

		GlobalManager.instance.PartyFactories = teamMembers;

	}

	public void displayTank(){
		currentChar = Chars.tank;
	}

	public void displayMedic(){
		currentChar = Chars.medic;
	}

	public void displayAssassin(){
		currentChar = Chars.assassin;
	}

	public void displayTech(){
		currentChar = Chars.tech;
	}

	public void selectChar(){
		switch (currentChar) {
		case Chars.tank:
			teamMembers.Add (new TankFactory());
			break;
		case Chars.medic:
			teamMembers.Add (new MedicFactory());
			//charImage.mainTexture = medicImage; ??
			break;
		case Chars.assassin:
			teamMembers.Add (new AssassinFactory());
			//charImage.mainTexture = assassinImage;
			break;
		case Chars.tech:
			teamMembers.Add (new TechFactory());
			//charImage.mainTexture = techImage;
			break;
		default:
			break;
		}

		for( int i = 0; i < teamMembers[teamMembers.Count - 1].talentOptions.Count; ++i) {
			//get the talent name + description
			talentDescriptionsList[i].text = teamMembers[teamMembers.Count - 1].talentOptions[i].name + "\n" + 
				teamMembers[teamMembers.Count - 1].talentOptions[i].description;
			talentImages[i].sprite = teamMembers[teamMembers.Count - 1].talentOptions[i].icon;
			//initialize left column of talents to be selected, right column unselected
			if( i % 2 == 0)
				teamMembers[teamMembers.Count - 1].talentOptions[i].toggle = true;
			else
				teamMembers[teamMembers.Count - 1].talentOptions[i].toggle = false;


		}

	}

	public void confirmSelectChar(){
		//update the team member image icons here
		teamMemberIcons [teamMembers.Count - 1].sprite = teamMembers [teamMembers.Count - 1].image;
		//set their alpha in Update
	}

	public void cancelSelectChar(){
		teamMembers.RemoveAt(teamMembers.Count - 1);
	}

	public void removeTeamMember(){
		//the buttons for this dont exist yet

		//function should remove a character from the team
	}

	// Update is called once per frame
	void Update () {
		switch (currentChar) {
			case Chars.tank:
				charDescription.text = tankDescription;
				//charImage.mainTexture = tankImage;
				break;
			case Chars.medic:
				charDescription.text = medicDescription;
				//charImage.mainTexture = medicImage;
				break;
			case Chars.assassin:
				charDescription.text = assassinDescription;
				//charImage.mainTexture = assassinImage;
				break;
			case Chars.tech:
				charDescription.text = techDescription;
				//charImage.mainTexture = techImage;
				break;
			default:
				break;
		}

		//full team or not enough team members, toggle buttons
		if (teamMembers.Count == 3) {
			beginGame.enabled = true;
			selectCharacter.enabled = false;
		} else {
			beginGame.enabled = false;
			selectCharacter.enabled = true;
		}

	}
}
