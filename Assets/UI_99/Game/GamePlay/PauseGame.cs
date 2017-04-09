using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour {
	public Transform canvas;

	//public GameObject canvas2;

	//public Transform Player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void PauseOn()
	{
		
		if(canvas.gameObject.activeInHierarchy == false){
			canvas.gameObject.SetActive(true);
			Time.timeScale = 0;
	}
		else{
			canvas.gameObject.SetActive (false);

			Time.timeScale = 1;
		}
    }
}
