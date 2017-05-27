using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelect1 : MonoBehaviour {

	//public GameObject player;
	//public List<GameObject> XCard;
	bool selected;
	// Use this for initialization
	void Start () {
		//player = GameObject.Find ("PlayerPanel");
		//XCard = player.GetComponent<PlayerScript> ().myCard;
		selected = false;
	}

	public bool isSelected(){
		return selected;
	}

	public void setSel(bool XXX){
		selected = XXX;
	}
}
