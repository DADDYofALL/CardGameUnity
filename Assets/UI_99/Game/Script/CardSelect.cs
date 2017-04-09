using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelect : MonoBehaviour {

	public GameObject player;
	public List<GameObject> XCard;
	bool selected;
	// Use this for initialization
	void Start () {
		//player = GameObject.Find ("PlayerPanel");
		XCard = player.GetComponent<PlayerScript> ().myCard;
		selected = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool isSelected(){
		/*foreach (GameObject X in XCard) {
			if (X.GetComponent<CardScript> ().selected)
				return true;
		}*/
		return selected;
	}

	public void setSel(bool XXX){
		selected = XXX;
	}
}
