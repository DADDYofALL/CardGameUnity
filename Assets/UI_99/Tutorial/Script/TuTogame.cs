using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TuTogame : MonoBehaviour {

	// Use this for initialization
	public TutorialAnimation manager;

	public void GoTo(int sceneIndex)
	{
		StartCoroutine(GoToGame(sceneIndex));
	}

	IEnumerator GoToGame(int sceneIndex)
	{
		manager.GoOutAll();
		//wait a bit so we can see the UI go out before switching scene...
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(sceneIndex);
	}
}
