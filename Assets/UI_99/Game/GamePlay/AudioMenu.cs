using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMenu : MonoBehaviour {
	public Transform canvas1;
	public Transform canvas2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void PauseOn()
	{
		if(canvas1.gameObject.activeInHierarchy == false){
			canvas1.gameObject.SetActive(true);
			canvas2.gameObject.SetActive(false);
		}
		else{
			canvas1.gameObject.SetActive (false);
			canvas2.gameObject.SetActive(true);
			Time.timeScale = 1;
		}
	}
}
