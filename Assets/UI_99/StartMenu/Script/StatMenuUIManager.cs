using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatMenuUIManager : MonoBehaviour {
	public Animator lowerLeft;
	public Animator upperLeft;


	private const string comeInString = "ComeIn";
	private const string goOutString = "GoOut";
	// Use this for initialization
	void Start () {
		ComeInAll();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ComeInAll()
	{
		StopAllCoroutines ();
		StartCoroutine(ComeInAllRoutine());
	}
	public void GoOutAll()
	{
		//I dont want sequences in GoOut, just disappear together.
		//Coroutine not needed here
		StopAllCoroutines();
		lowerLeft.SetTrigger(goOutString);
		upperLeft.SetTrigger(goOutString);
	}
	IEnumerator ComeInAllRoutine()
	{
		//With Coroutine, I can wait a bit, creating "UI animation sequence"


		upperLeft.SetTrigger(comeInString);
		yield return new WaitForSeconds(0.4f);
		lowerLeft.SetTrigger(comeInString);
	}
}
