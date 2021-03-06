using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class DeckScript : MonoBehaviour {

    public List<GameObject> cards;
    public GameObject topOfDeck;	
    Vector3 offset = new Vector3(0.0f, 0.01f, 0.0f);
    // Use this for initialization
    void Start() {
		//createDeck ();
		//shuffle ();
		//DebugDeck ();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void createDeck()
    {
        Vector3 handLocation = this.transform.position;
        cards = new List<GameObject>();
        topOfDeck = (GameObject)Instantiate(Resources.Load("Back"), handLocation, Quaternion.Euler(0f, 0.0f, 0.0f));
        topOfDeck.transform.position += offset;
	
		string[] Rank = { "2", "3", "4", "5", "6", "7", "8", "9", "10","J","Q","K","A" };
		Card.Value[] X = {Card.Value.TWO,Card.Value.THREE,Card.Value.FOUR,Card.Value.FIVE,Card.Value.SIX,Card.Value.SEVEN,Card.Value.EIGHT,Card.Value.NINE,Card.Value.TEN,Card.Value.JACK,Card.Value.QUEEN,Card.Value.KING,Card.Value.ACE};
		for (int i = 0; i<13; i++) {

			string Club = "Clubs_"+Rank[i];
			string Diamond = "Diamonds_"+Rank[i];
			string Heart = "Hearts_"+Rank[i];
			string Spade = "Spades_"+Rank[i];

			handLocation += new Vector3( -1.0f, -0.5f,0.1f);
			cards.Add ((GameObject)Instantiate (Resources.Load (Club), handLocation, Quaternion.Euler (0.0f, 0.0f, 0.0f)));
			cards.Last ().GetComponent<CardScript> ().setCard (new Card (X[i], Card.Suite.CLUBS));
			cards.Last ().GetComponent<CardScript> ().setOwner (98);

			//handLocation = new Vector3( this.transform.position.x+(i*10), this.transform.position.y+20, this.transform.position.z);
			cards.Add ((GameObject)Instantiate (Resources.Load (Diamond), handLocation, Quaternion.Euler (0.0f, 0.0f, 0.0f)));
			cards.Last ().GetComponent<CardScript> ().setCard (new Card (X[i], Card.Suite.DIAMONDS));
			cards.Last ().GetComponent<CardScript> ().setOwner (98);

			//handLocation = new Vector3( this.transform.position.x+(i*10), this.transform.position.y+40, this.transform.position.z);
			cards.Add ((GameObject)Instantiate (Resources.Load (Heart), handLocation, Quaternion.Euler (0.0f, 0.0f, 0.0f)));
			cards.Last ().GetComponent<CardScript> ().setCard (new Card (X[i], Card.Suite.HEARTS));
			cards.Last ().GetComponent<CardScript> ().setOwner (98);

			//handLocation = new Vector3( this.transform.position.x+(i*10), this.transform.position.y+60, this.transform.position.z);
			cards.Add ((GameObject)Instantiate (Resources.Load (Spade), handLocation, Quaternion.Euler (0.0f, 0.0f, 0.0f)));
			cards.Last ().GetComponent<CardScript> ().setCard (new Card (X[i], Card.Suite.SPADES));
			cards.Last ().GetComponent<CardScript> ().setOwner (98);
		}
		Debug.Log ("total Card = " + cards.Count);
    }

	public bool isEmpty (){
		if (cards.Count () == 0)
			return true;
		return false;
	}

    // Shuffle the Card List
    public void shuffle()
    {
        if (this.cards.Count >= 1)
            this.cards = this.cards.OrderBy(c => Guid.NewGuid()).ToList();
    }

    public GameObject draw()
    {
        GameObject drawCard;
        drawCard = this.cards.ElementAt(0);
        this.cards.RemoveAt(0);

        return drawCard;
    }

    public void resetDeck()
    {
        foreach (GameObject card in this.cards)
        {
            Destroy(card);
        }
        this.cards = new List<GameObject>();
    }

    public List<GameObject> getCards()
    {
        return this.cards;
    }

	public void DebugDeck()
	{
		foreach (GameObject card in this.cards)
		{
			Debug.Log (card.gameObject.name);
		}
	}
}
