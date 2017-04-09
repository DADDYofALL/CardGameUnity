using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour {

	/*public GameObject Card;
	GameObject dummy = null;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < 5; i++) {	
			dummy = Instantiate (Card, this.transform.position, Quaternion.identity);
			dummy.transform.SetParent (this.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}*/

	float Offset = 0.0f;
	float OffsetC = 0.0f;
	//Vector3 handLocation = new Vector3 (0.0f,0.0f,0.0f) ;
	int factor = 1;

	public void addCard(GameObject c) {
		
		//c.transform.position = this.transform.GetChild (index).position;
		c.transform.SetParent (this.transform);
		//Debug.Log (index);
		c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.z - OffsetC);
		//c.transform.position = handLocation;
		c.transform.rotation = new Quaternion(0.0f, 0.0f, Offset*factor, 1.0f);
		//handLocation += new Vector3( -1.0f, -0.5f, -0.5f);
		Offset += 0.01f;
		OffsetC += 1.0f;
		factor *= -1;
	}
}
