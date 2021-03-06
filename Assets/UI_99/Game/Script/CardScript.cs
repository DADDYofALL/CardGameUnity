using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class CardScript : MonoBehaviour, IComparable
{

    public bool selected = false;
    public int owner;
    public Card card;
	public GameObject X;
    //Change me to change the touch phase used.
    //TouchPhase touchPhase = TouchPhase.Ended;
	//public Button TurnEndo;

    // Use this for initialization
    void Start () {
		X = GameObject.Find ("PlayerPanel");
	}

    void OnMouseDown()
    {
		if (this.owner == 0 && selected == false && !X.GetComponent<CardSelect>().isSelected() )
        {
            var x = this.transform.position.x;
            var y = this.transform.position.y;
            var z = this.transform.position.z;
            this.transform.position = new Vector3(x, y+20.0f, z);
            selected = true;
			X.GetComponent<CardSelect> ().setSel (true);
			//TurnEndo.interactable = true;
        }
        else if (this.owner == 0 && selected == true)
        {
            var x = this.transform.position.x;
            var y = this.transform.position.y;
            var z = this.transform.position.z;
            this.transform.position = new Vector3(x, y-20.0f, z);
            selected = false;
			X.GetComponent<CardSelect> ().setSel (false);
			//TurnEndo.interactable = false;
        }
    }

    public void setOwner(int player)
    {
        this.owner = player;
    }

    public void setCard(Card card)
    {
        this.card = card;
    }

    // implement IComparable interface
    public int CompareTo(object obj)
    {
        if (obj is Card)
        {
            return this.card.value.CompareTo((obj as Card).value);  // compare user names
        }
        throw new ArgumentException("Object is not a Card");
    }

	public Card getCard(){
		return this.card;
	}
}
