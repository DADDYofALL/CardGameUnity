using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimation : MonoBehaviour {
	public Animator WordTu;
	public Animator WordTuR;
	public Animator Button;
	// Use this for initialization
	private const string comeInString = "ComeIn";
	private const string goOutString = "GoOut";

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

		WordTu.SetTrigger(goOutString);
		WordTuR.SetTrigger(goOutString);
		Button.SetTrigger(goOutString);
	}
	IEnumerator ComeInAllRoutine()
	{
		//With Coroutine, I can wait a bit, creating "UI animation sequence"

		WordTu.SetTrigger(comeInString);
		WordTuR.SetTrigger(comeInString);
		yield return new WaitForSeconds(0.4f);
		Button.SetTrigger(comeInString);
	}
}
