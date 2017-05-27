using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Database_go : MonoBehaviour {
	//create reference
	ArrayList leaderBoard;



	public Text tt ;
	public Text statpy;
	DatabaseReference myDatabaseRef;

	// Use this for initialization
	void Start () {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://card-e7bea.firebaseio.com/");
		// set to root
		myDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
		//tt.text = "";
	}

	public void writeNewPlayer(string playerName, int win, int lose, float time) {
		FirebaseDatabase.DefaultInstance
			.GetReference("Player").Child(playerName)
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					Debug.Log("Error");
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					if(snapshot.Value != null){
						var stat = snapshot.Value as Dictionary<string, object>;
						foreach (var item in stat)
						{
							if(item.Key == "win")
								win += int.Parse(item.Value.ToString());
							if(item.Key == "lose")
								lose += int.Parse(item.Value.ToString());
							if(item.Key == "time")
								time += float.Parse(item.Value.ToString());
					}
				}
				}
				Stat playerStat = new Stat(win, lose);
				string json = JsonUtility.ToJson(playerStat);

				myDatabaseRef.Child("Player").Child(playerName).SetRawJsonValueAsync(json);
			});
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.V)){
			Debug.Log ("V Key Babe");

			DisplayPlayerStat ("amuro ray");
		}
		else if (Input.GetKeyDown (KeyCode.C)) {
			Debug.Log ("C Key Babe");
			FirebaseDatabase.DefaultInstance.GetReference ("Player").OrderByChild("win").ValueChanged += HandleValueChanged;
		} else if (Input.GetKeyDown (KeyCode.X)) {
			Debug.Log ("X Key Babe");

		} else if (Input.GetKeyDown (KeyCode.Z)) {
			Debug.Log ("Z Key Babe");
		}
	}

		//Debug.Log(Snapshot.key);

	void HandleValueChanged(object sender, ValueChangedEventArgs args) {
			if (args.DatabaseError != null) {
				Debug.LogError(args.DatabaseError.Message);
				return;
			}
			var stat = args.Snapshot.Value as Dictionary<string, object>;
			foreach (var item in stat)
			{
				Debug.Log(item.Key); // Kdq6...
			    tt.text += item.Key + "\n";
				var values = item.Value as Dictionary<string, object>;
				foreach (var v in values)
				{
				Debug.Log(v.Key + ": " + v.Value);
				tt.text += v.Key + " " +  v.Value +   "\n"; // category:livingroom, code:126 ...
				}
			
		}
	}
		
	void DisplayPlayerStat(String name){
		statpy.text = "";
		FirebaseDatabase.DefaultInstance
			.GetReference("Player").Child(name)
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					Debug.Log("Error");
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					Debug.Log("ads"+snapshot.Key);
					var stat = snapshot.Value as Dictionary<string, object>;
					foreach (var item in stat)
					{
						Debug.Log(item.Key + ": " + item.Value);
						statpy.text += item.Key + " " +  item.Value + " ";
					}
				}
			});
	}
		
}
public class Stat {
		public int win;
		public int lose;

		public Stat(int win, int lose) {
			this.win = win;
			this.lose = lose;
		}
}
