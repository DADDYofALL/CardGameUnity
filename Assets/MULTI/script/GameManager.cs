using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;
using System;

public class GameManager : MonoBehaviour {

	DatabaseReference myDatabaseRef;

	public Button TurnEndo;
	public Text scoreboard, TurnText;

    private bool _gameReady;
    private bool _isMyTurn;
	private bool[] isDead;
	private string[] cardName;

	private int _PlayerNo, _NextTurn, _Score, Direction;
	float playtime;

	public List <GameObject> myHand;
	public List <GameObject> Junk;

	GameObject player1,table,Username;
	public GameObject Win, Lose, Gameplay;

	void CreateCardNameAndSetDead(){
		string[] Rank = {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10","J","Q","K" };
		for (int i = 0; i < 52; i++) {
			string name;
			if(i < 13)
				name = "Clubs_" + Rank [i%13];
			else if (i < 26)
				name = "Diamonds_" + Rank [i%13];
			else if (i < 39)
				name = "Hearts_" + Rank [i%13];
			else
				name = "Spades_" + Rank [i%13];
			cardName [i] = name;
		}
		for (int i = 0; i < 4; i++)
			isDead [i] = false;
	}

    void Awake()
    {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://card-e7bea.firebaseio.com/");
		myDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;

		_gameReady = false;
		_isMyTurn = true;
		isDead = new bool[4];
		cardName = new string[52];
		CreateCardNameAndSetDead ();
		_Score = 0;
		_PlayerNo = 0;
		_NextTurn = 0;
		Direction = 1;

		player1 = GameObject.Find("Player");
		table = GameObject.Find("Table");
		Username = GameObject.Find ("User");

		//Debug.Log (Username.GetComponent<UserScriupt> ().getUsername());

        refreshButton();
        NetworkController.Instance.GameManager = this;
		playtime = Time.time;
		Debug.Log ("Time"+playtime);
    }

	public void writeNewPlayer(string playerName, int win, int lose) {
		FirebaseDatabase.DefaultInstance
			.GetReference("Player").Child(playerName)
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					Debug.Log("Error");
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					if(snapshot.Value != null){
						var stat = snapshot.Value as Dictionary<string, object>;
						foreach (var item in stat)
						{
							if(item.Key == "win")
								win += int.Parse(item.Value.ToString());
							if(item.Key == "lose")
								lose += int.Parse(item.Value.ToString());
						}
					}
				}
				Stat playerStat = new Stat(win, lose);
				string json = JsonUtility.ToJson(playerStat);

				myDatabaseRef.Child("Player").Child(playerName).SetRawJsonValueAsync(json);
			});
	}

    void Start()
    {
        StartCoroutine(ConnectCoroutine());
    }

    void Update()
    {
        NetworkController.Instance.ProcessEvents();
    }

    private IEnumerator ConnectCoroutine()
    {
        NetworkController controller = NetworkController.Instance;
        controller.Connect();
        controller.ProcessEvents();
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 10; i++)
        {
            if (controller.Connected())
            {
                break;
            }
            if (controller.ConnectFailed())
            {
                break;
            }

            controller.ProcessEvents();
            yield return new WaitForSeconds(i * 0.1f);
        }

        if (controller.Connected())
        {
            Debug.Log("Connected!");
            _gameReady = true;
        }
        else
        {
            yield return new WaitForSeconds(5f);
            Debug.Log("Cannot connect");
        }

        yield break;
    }

    public void OnDisconnect()
    {
        _gameReady = false;
        OnEndMatch(false);
    }

	public void OnStartMatch(int matchID, int PlayerNo, int Card1, int Card2, int Card3, int Card4)
    {
        //Show Match ID
		Debug.Log("Match ID : "+matchID);
		//set Player No.
		_PlayerNo = PlayerNo;
		Debug.Log ("Player No : "+PlayerNo);
		//set Hand[]
		Card1 A = MapCard(Card1);
		Card1 B = MapCard(Card2);
		Card1 C = MapCard(Card3);
		Card1 D = MapCard(Card4);

		//Show Card on screen
		myHand.Add( (GameObject)Instantiate (Resources.Load (cardName[Card1]), player1.transform.position, Quaternion.identity));
		myHand.Last ().GetComponent<CardScript1> ().setCard (new Card1 (A.value, A.suite));
		myHand.Last ().transform.SetParent (player1.transform);
		myHand.Add( (GameObject)Instantiate (Resources.Load (cardName[Card2]), player1.transform.position, Quaternion.identity));
		myHand.Last ().GetComponent<CardScript1> ().setCard (new Card1 (B.value, B.suite));
		myHand.Last ().transform.SetParent (player1.transform);
		myHand.Add( (GameObject)Instantiate (Resources.Load (cardName[Card3]), player1.transform.position, Quaternion.identity));
		myHand.Last ().GetComponent<CardScript1> ().setCard (new Card1 (C.value, C.suite));
		myHand.Last ().transform.SetParent (player1.transform);
		myHand.Add( (GameObject)Instantiate (Resources.Load (cardName[Card4]), player1.transform.position, Quaternion.identity));
		myHand.Last ().GetComponent<CardScript1> ().setCard (new Card1 (D.value, D.suite));
		myHand.Last ().transform.SetParent (player1.transform);

		Debug.Log ("Card On Hand After Start:"+myHand.Count());
		//Check Player No
		if(PlayerNo == 0){
			//set Turn
			_isMyTurn = true;
			refreshButton();
			//set play time
			playtime = Time.time;
		}
		if (_isMyTurn) {
			TurnText.text = "Ore no Turn!";
		} else {
			TurnText.text = "Waiting Player " + (_NextTurn+1);
		} 	
    }

    public void OnEndMatch(bool win)
    {
		//Show Win or Loose
		if (win)
			Debug.Log ("WIN");
    }

	public void OnDrawCard(int C)
	{
		//Add card to hand
		Card1 Draw = MapCard(C);
		myHand.Add( (GameObject)Instantiate (Resources.Load (cardName[C]), player1.transform.position, Quaternion.identity));
		myHand.Last ().GetComponent<CardScript1> ().setCard (new Card1 (Draw.value, Draw.suite));
		myHand.Last ().transform.SetParent (player1.transform);
		Debug.Log("Played and Draw "+C);
	}

	public void OnOppSelect(int C)
    {
		if (!isDead [_PlayerNo]) {
			Debug.Log ("Opp Play:" + C);
			//opp play card
			Junk.Add ((GameObject)Instantiate (Resources.Load (cardName [C]), table.transform.position, Quaternion.identity));
			Junk.Last ().transform.SetParent (table.transform);
			//Calculate Score
			CalculateScore (MapCard (C));

			if (CheckDead3 ()) {
				Debug.Log ("End");
				Debug.Log ("Win");
				Win.gameObject.SetActive (true);
				Gameplay.gameObject.SetActive (false);
				writeNewPlayer(Username.GetComponent<UserScriupt>().getUsername(),1,0);
				//Win Screen
			}
		//Calculate Nextturn
		else {
				CalculateNext ();
			}
			if (_NextTurn == _PlayerNo) {
				//set myTurn to true
				_isMyTurn = true;
				refreshButton ();
				//Start count time
				playtime = Time.time;
			}
		}

    }

	private void checkHand(){
		Debug.Log ("Card On Hand After play:"+myHand.Count());
		if (myHand.Count () == 0) {
			Debug.Log ("Win");
			Win.gameObject.SetActive(true);
			Gameplay.gameObject.SetActive (false);
			writeNewPlayer(Username.GetComponent<UserScriupt> ().getUsername(),1,0);
		}
	}

	private bool CheckDead3(){
		int Check = 0;
		for (int i = 0; i < 4; i++) {
			if (isDead [i])
				Check++;
		}
		if (Check == 3)
			return true;
		else
			return false;
	}

    private void refreshButton()
    {	
        //set button to interact or uninteract
		if (_isMyTurn)
			TurnEndo.interactable = true;
		else
			TurnEndo.interactable = false;
    }

    public void JoinClick()
    {
        if (!_gameReady) return;
        NetworkController.Instance.RequestFindMatch();
		_isMyTurn = false;
		refreshButton ();
    }

    public void DisconnectClick()
    {
        if (!NetworkController.Instance.Connected()) return;
        NetworkController.Instance.Disconnect();
    }

	private void myPlay(int C)
    {
		Junk.Add( (GameObject)Instantiate (Resources.Load (cardName[C]), table.transform.position, Quaternion.identity));
		Junk.Last ().transform.SetParent (table.transform);
		CalculateScore(MapCard(C));
		Debug.Log ("Play:" + C%13);
		CalculateNext ();
		checkHand ();

		////-------------------------------------------------------------------------------------------------------------
		NetworkController.Instance.RequestPlay(C);
        ////-------------------------------------------------------------------------------------------------------------
		_isMyTurn = false;
        refreshButton();
    }

	public void SelectCard(){
		int index = 0;
		foreach (GameObject card in myHand) {
			if (card != null) {
				if (card.GetComponent<CardScript1> ().selected) {
					break;
				}
			}
			index++;
		}
		Debug.Log ("Index:"+index);
		int SendCard = MapCard2( myHand [index].GetComponent<CardScript1> ().card );
		GameObject forDes = myHand[index];
		myHand.RemoveAt (index);
		Destroy (forDes);
		myPlay (SendCard);

		player1.GetComponent<CardSelect1> ().setSel (false);
		Debug.Log ("Press TurnEndo!");

	}

	private void CalculateNext(){
		_NextTurn += Direction;
		if (_NextTurn < 0)
			_NextTurn = 3;
		else if (_NextTurn > 3)
			_NextTurn = 0;
		for(int i=0 ; i<2 ; i++) {
			if (isDead [_NextTurn]) {
				_NextTurn += Direction;
				if (_NextTurn < 0)
					_NextTurn = 3;
				else if (_NextTurn > 3)
					_NextTurn = 0;
			} else
				break;
		}
		Debug.Log ("Next : "+_NextTurn);
		if (_NextTurn == _PlayerNo) {
			TurnText.text = "Ore no Turn!";
		} else {
			TurnText.text = "Waiting Player " + (_NextTurn+1);
		} 	
	}

	private Card1 MapCard (int CardNo){
		int Suite = CardNo / 13;
		int Value = CardNo % 13;

		Card1.Value V = Card1.Value.ACE; 
		Card1.Suite S = Card1.Suite.CLUBS;

		switch (Suite) {
		case 0: S = Card1.Suite.CLUBS;break;
		case 1: S = Card1.Suite.DIAMONDS;break;
		case 2: S = Card1.Suite.HEARTS;break;
		case 3: S = Card1.Suite.SPADES;break;
		default: break;
		}
		switch (Value) {
		case 1: V = Card1.Value.TWO; break;
		case 2: V = Card1.Value.THREE; break;
		case 3: V = Card1.Value.FOUR; break;
		case 4: V = Card1.Value.FIVE; break;
		case 5: V = Card1.Value.SIX; break;
		case 6: V = Card1.Value.SEVEN; break;
		case 7: V = Card1.Value.EIGHT; break;
		case 8: V = Card1.Value.NINE; break;
		case 9: V = Card1.Value.TEN; break;
		case 10: V = Card1.Value.JACK; break;
		case 11: V = Card1.Value.QUEEN; break;
		case 12: V = Card1.Value.KING; break;
		case 0: V = Card1.Value.ACE; break;
		default: break;
		}

		Card1 Out = new Card1 (V,S);
		return Out;
	}
	private int MapCard2 (Card1 C){
		int Out = 0;
		switch (C.suite) {
		case Card1.Suite.CLUBS: Out += 0; break;
		case Card1.Suite.DIAMONDS: Out += 13; break;
		case Card1.Suite.HEARTS: Out += 26; break;
		case Card1.Suite.SPADES: Out += 39; break;
		default: break;
		}
		switch (C.value) {
		case Card1.Value.ACE: Out += 0; break;
		case Card1.Value.TWO: Out += 1; break;
		case Card1.Value.THREE: Out += 2; break;
		case Card1.Value.FOUR: Out += 3; break;
		case Card1.Value.FIVE: Out += 4; break;
		case Card1.Value.SIX: Out += 5; break;
		case Card1.Value.SEVEN: Out += 6; break;
		case Card1.Value.EIGHT: Out += 7; break;
		case Card1.Value.NINE: Out += 8; break;
		case Card1.Value.TEN: Out += 9; break;
		case Card1.Value.JACK: Out += 10; break;
		case Card1.Value.QUEEN: Out += 11; break;
		case Card1.Value.KING: Out += 12; break;
		default: break;
		}
		return Out;
	}
	public void CalculateScore(Card1 card){
		int temp = _Score;
		switch(card.value ){
		case Card1.Value.ACE:
			_Score +=1;
			break;
		case Card1.Value.TWO: 
			_Score +=2;
			break;
		case Card1.Value.THREE:	
			_Score = 99;
			break;
		case Card1.Value.FOUR:
			break;	
		case Card1.Value.FIVE:
			_Score +=5;
			break;
		case Card1.Value.SIX:
			_Score += 6;
			break;
		case Card1.Value.SEVEN:
			_Score += 7;
			break;
		case Card1.Value.EIGHT:
			_Score += 8;
			break;
		case Card1.Value.NINE:
			_Score -= 9;
			break;
		case Card1.Value.TEN:
			_Score -= 10;
			break;
		case Card1.Value.JACK:
			Direction *= -1;
			break;
		case Card1.Value.QUEEN:
			_Score += 10;
			break;
		case Card1.Value.KING:
			_Score += 10;
			break;
		default: break;
		}

		if (_Score > 99) {
			_Score = temp;
			isDead [_NextTurn] = true;
			if (isDead [_PlayerNo]) {
				Debug.Log ("Lose");
				Lose.gameObject.SetActive (true);
				Gameplay.gameObject.SetActive (false);
				writeNewPlayer(Username.GetComponent<UserScriupt> ().getUsername(),0,1);
			}
			_Score = 99;
		}
		Debug.Log ("Score :"+_Score);
		scoreboard.text = "Point: " + _Score;
	}
}