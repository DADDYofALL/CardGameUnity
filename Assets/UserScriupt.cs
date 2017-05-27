using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserScriupt : MonoBehaviour {

	string Username;

	void Awake(){
		Username = "XXX";
	}

	public void OnClick(){
		//load scene
		SceneManager.LoadScene(3);
	}

	public void OnText(string X){
		Username = X;
	}

	public string getUsername(){
		return Username;
	}
}
