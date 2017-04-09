using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCon : MonoBehaviour {
	/*GameObject player1;
	int Chip = 0;
	int pot = 0;

	int sliderbar = 0;

	public Text raise;
	public Text AllChip;
	public Text Pot;
	public Text CheckText;
	bool checkswitch = true;


	public void SeeValue(float X){
		sliderbar = Mathf.FloorToInt( X*Chip );
		raise.text = "$ " + sliderbar;
	}

	public void RaiseButton(){
		pot += sliderbar;
		Chip -= sliderbar;
		AllChip.text = "$ " + Chip;
		Pot.text = "POT\n$" + pot;
	}

	public void CheckButton(){
		if (checkswitch) {
			CheckText.text = "Check";
		} else {
			CheckText.text = "Call";
		}
		checkswitch = !checkswitch;
	}

	public void FoldButton(){
		//Debug.Log ("Fold");

	}

	void Start () {	
		player1 = GameObject.Find("Player");
		ui ();
	}

	void Update () {
		if(player1.GetComponent<PlayerScript> ().updateUI)
			ui ();
	}

	void ui(){

		Chip = player1.GetComponent<PlayerScript>().money;
		Debug.Log (Chip);
		raise.text = "$ " + 0;
		AllChip.text = "$ " + Chip;
		Pot.text = "POT\n$" + pot;
		player1.GetComponent<PlayerScript> ().updateUI = false;
	}*/
}