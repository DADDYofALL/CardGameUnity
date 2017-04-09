using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class GameControl : MonoBehaviour {

	public Button TurnEndo;
	private GameObject deck;
	private GameObject panel;
	GameObject[] players;
	bool[] isDead = new bool[4];
	public int currentPlayerIndex;
	public int currentDirection;
	int point;
	public Text Points;
	public Text[] EnermyText;
	public Transform canvasWin;
	public Transform canvasLose;


	// Use this for initialization
	void Start () {

		GameObject player1 = GameObject.Find("PlayerPanel");
		GameObject player2 = GameObject.Find("EnemyPanel1");
		GameObject player3 = GameObject.Find("EnemyPanel2");
		GameObject player4 = GameObject.Find("EnemyPanel3");

		players = new GameObject[]{ player1, player2, player3, player4};
		bool[] isDead = {false, false, false, false};
		//players = new GameObject[]{ player1, player2};
		deck = GameObject.Find("DECK");
		panel = GameObject.Find("Panel");
		//set playerindex 0=player, 1=enemy
		currentPlayerIndex = 0;
		currentDirection = 1;
		//create deck and shuffle
		deck.GetComponent<DeckScript>().createDeck();
		deck.GetComponent<DeckScript>().shuffle();

		//draw card to All player
		for (int j = 0; j < 4; j++) {
			for (int i = 0; i < 4; i++) {
				GameObject card = deck.GetComponent<DeckScript> ().draw ();
				card.GetComponent<CardScript> ().setOwner (j);
				players [j].GetComponent<PlayerScript> ().addCard (card);
			}
		}/*
		//draw card to enemy
		for (int i = 0; i < 4; i++) {
			GameObject card = deck.GetComponent<DeckScript>().draw();
			card.GetComponent<CardScript>().setOwner(1);
			players[1].GetComponent<PlayerScript>().addCard(card);
		}*/

		//draw initial card 
		while (true){
			GameObject Icard = deck.GetComponent<DeckScript>().draw();
			Calculate(Icard.GetComponent<CardScript>().getCard());
			if (point > 0) {
				panel.GetComponent<PanelControl>().addCard(Icard);
				break;
			}else {
				point = 0;
				Destroy (Icard);
			}
		}

		Points.text = "POINTS\n" + point;
	}

	//Do when click Button and AI
	bool DebugButton;
	public void ButtonDown() {

		//StopAllCoroutines();

		DebugButton = false;
		if (currentPlayerIndex == 0) {
			int index = -1;

			if (isDead [1] == true && isDead [2] == true && isDead [3] == true) {
				canvasWin.gameObject.SetActive (true);
				Points.text = "YOU WIN\n";
			}

			GameObject Pcard = new GameObject ();
			foreach (GameObject card in players[0].GetComponent<PlayerScript>()	.myCard) {
				if (card != null) {
					//play card that selected
					if (card.GetComponent<CardScript> ().selected) {
						DebugButton = true;
						//get card index
						index = players [0].GetComponent<PlayerScript> ().getCardIndex (card);
						//add card to panel
						card.GetComponent<CardScript> ().setOwner (99);
						panel.GetComponent<PanelControl> ().addCard (card);
						Debug.Log ("Player : " + card.GetComponent<CardScript> ().getCard ().value);
						//calculate point
						Pcard = card;
					}
				}
			}
			if (DebugButton) {
				//button uninteractable
				TurnEndo.interactable = false;
			
				players [0].GetComponent<CardSelect> ().setSel (false);
				//Remove void in list
				if (index != -1) {
					Debug.Log ("INDEX : " + index);
					players [0].GetComponent<PlayerScript> ().myCard.RemoveAt (index);
				}




				if (Calculate (Pcard.GetComponent<CardScript> ().getCard ())) {
					Debug.Log ("Player Dead");
					Points.text = "Player Dead\n";
					canvasLose.gameObject.SetActive(true);
				} else {
					//check deck is empty?
					if (!(deck.GetComponent<DeckScript> ().isEmpty ())) {
						//if not
						//draw new card
						GameObject newcard = deck.GetComponent<DeckScript> ().draw ();
						newcard.GetComponent<CardScript> ().setOwner (0);
						players [0].GetComponent<PlayerScript> ().addCard (newcard);
					}
					//next player
					//currentPlayerIndex = (currentPlayerIndex + 1) % 2;
					NextPlayer ();
					Debug.Log ("Next Player : " + currentPlayerIndex);
					//Check player win
					if (players [0].GetComponent<PlayerScript> ().myCard.Count == 0) {
						Points.text = "YOU WIN\n";
						canvasWin.gameObject.SetActive(true);
					}
					else
						ButtonDown ();
				}
			}
		} else {
			
			//AI
			//CHECK DEAD
			if (isDead [currentPlayerIndex]) {
				//next player
				NextPlayer ();
				if (currentPlayerIndex != 0)
					ButtonDown ();
				else
					TurnEndo.interactable = true;
				if (isDead [1] == true && isDead [2] == true && isDead [3] == true) {
					canvasWin.gameObject.SetActive (true);
					Points.text = "YOU WIN\n";
				}
			
			} else {

				StartCoroutine( Stop () );
				
			}
		}
	}

	void AI (){
		//Discard first card
		GameObject card = players [currentPlayerIndex].GetComponent<PlayerScript> ().myCard [0];
		players [currentPlayerIndex].GetComponent<PlayerScript> ().myCard.RemoveAt (0);
		//Destroy Back card on screen
		Destroy (players [currentPlayerIndex].GetComponent<PlayerScript> ().backCard [0]);
		players [currentPlayerIndex].GetComponent<PlayerScript> ().backCard.RemoveAt (0);
		//display card that discard on table panel
		card.GetComponent<CardScript> ().setOwner (99);
		panel.GetComponent<PanelControl> ().addCard (card);
		Debug.Log ("AI : " + currentPlayerIndex + " " + card.GetComponent<CardScript> ().getCard ().value);



		//calculate point
		if (Calculate (card.GetComponent<CardScript> ().getCard ())) {
			Debug.Log ("Enemy " + currentPlayerIndex + " is Dead");
			EnermyText [currentPlayerIndex].text = "Dead\n";
			isDead [currentPlayerIndex] = true;
		} else {
			//check deck is empty?
			if (!(deck.GetComponent<DeckScript> ().isEmpty ())) {
				//if not
				//Draw new card
				GameObject newcard = deck.GetComponent<DeckScript> ().draw ();
				newcard.GetComponent<CardScript> ().setOwner (currentPlayerIndex);
				players [currentPlayerIndex].GetComponent<PlayerScript> ().addCard (newcard);

			}
		}
		//Check Win
		if (players [currentPlayerIndex].GetComponent<PlayerScript> ().myCard.Count == 0) {
			Points.text = "Enemy " + currentPlayerIndex + " win";
			canvasLose.gameObject.SetActive(true);
		}
		if (isDead [1] == true && isDead [2] == true && isDead [3] == true) {
			canvasWin.gameObject.SetActive (true);
			Points.text = "YOU WIN\n";
		}
		//next player
		NextPlayer ();	

		//if not Next player play
		if (currentPlayerIndex != 0)
			ButtonDown ();
		else
			TurnEndo.interactable = true;
	}


	//int[] soundtime = { };
	//int soundindex;
	public AudioSource[] XXX;
	IEnumerator Stop()
	{
		Debug.Log ( "start" + Time.time);
		//

		yield return new WaitForSeconds (5);
		AI ();
		Debug.Log ( "End" + Time.time);
	}

	void NextPlayer(){
		currentPlayerIndex += currentDirection;
		if (currentPlayerIndex > 3)
			currentPlayerIndex = 0;
		if (currentPlayerIndex < 0)
			currentPlayerIndex = 3;
	}

	public bool Calculate(Card card){
		int temp = point;
		switch(card.value ){
		case Card.Value.ACE:
			point +=1;
			XXX[0].Play ();
			break;
		case Card.Value.TWO: 
			point +=2;
			XXX[1].Play ();
			break;
		case Card.Value.THREE:	
			point = 99;
			XXX[2].Play ();
			break;
		case Card.Value.FOUR:
			XXX[3].Play ();
			break;	
		case Card.Value.FIVE:
			point +=5;
			XXX[4].Play ();
			break;
		case Card.Value.SIX:
			point += 6;
			XXX[5].Play ();
			break;
		case Card.Value.SEVEN:
			point += 7;
			XXX[6].Play ();
			break;
		case Card.Value.EIGHT:
			point += 8;
			XXX[7].Play ();
			break;
		case Card.Value.NINE:
			point -= 9;
			XXX[8].Play ();
			break;
		case Card.Value.TEN:
			point -= 10;
			XXX[9].Play ();
			break;
		case Card.Value.JACK:
			currentDirection *= -1;
			XXX[10].Play ();
			break;
		case Card.Value.QUEEN:
			point += 10;
			XXX[11].Play ();
			break;
		case Card.Value.KING:
			point += 10;
			XXX[12].Play ();
			break;
		default: break;
		}
		if (point > 99) {
			point = temp;
			UI ();
			return true;	
		}
		if (point < 0)
			point = 0;
		UI ();
		return false;

	}

	void UI(){
		Points.text = "POINTS\n" + point;
	}
}				