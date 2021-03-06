using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class CardScript1 : MonoBehaviour, IComparable
{

    public bool selected = false;
    public int owner;
    public Card1 card;
	public GameObject X;
    //Change me to change the touch phase used.
    //TouchPhase touchPhase = TouchPhase.Ended;
	//public Button TurnEndo;

    // Use this for initialization
    void Start () {
		X = GameObject.Find ("Player");
	}

    void OnMouseDown()
    {
		if (this.owner == 0 && selected == false && !X.GetComponent<CardSelect1>().isSelected() )
        {
            this.transform.position += new Vector3(0, 0.20f, 0);
            selected = true;
			X.GetComponent<CardSelect1> ().setSel (true);
			//TurnEndo.interactable = true;
        }
        else if (this.owner == 0 && selected == true)
        {
            this.transform.position += new Vector3(0, -0.20f, 0);
            selected = false;
			X.GetComponent<CardSelect1> ().setSel (false);
			//TurnEndo.interactable = false;
        }
    }

    public void setOwner(int player)
    {
        this.owner = player;
    }

    public void setCard(Card1 card)
    {
        this.card = card;
    }

    // implement IComparable interface
    public int CompareTo(object obj)
    {
        if (obj is Card1)
        {
            return this.card.value.CompareTo((obj as Card1).value);  // compare user names
        }
        throw new ArgumentException("Object is not a Card");
    }

	public Card1 getCard(){
		return this.card;
	}
}
