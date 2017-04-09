using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGame : MonoBehaviour {

	// Use this for initialization
	public StatMenuUIManager manager;

	public void GoTo(int sceneIndex)
	{
		StartCoroutine(GoToGame(sceneIndex));
	}

	IEnumerator GoToGame(int sceneIndex)
	{
		manager.GoOutAll();
		//wait a bit so we can see the UI go out before switching scene...
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(sceneIndex);
	}
}
