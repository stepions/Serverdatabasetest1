using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using TMPro;

public class GameManager : MonoBehaviour {

	public GameObject panel, buttonStart, buttonNext;
	public Player currentPlayer;
	string URL = "http://localhost:8081/users";
	public static GameManager instance;
	int score;
	public TMP_InputField txtScore;

	void Awake(){
		if(instance==null){
			instance = this;
		}else{
			if(instance!=this){
				Destroy(gameObject);
			}
		}
	}
	
	public void PhaseStart(Player input){
		panel.SetActive(true);
		buttonStart.SetActive(true);
		buttonNext.SetActive(false);
		currentPlayer = input;
	}

	public void PhaseStop(){
		panel.SetActive(false);
		buttonStart.SetActive(true);
		buttonNext.SetActive(false);
	}
	
	
	public void RollNumber(){
		buttonStart.SetActive(false);
		StartCoroutine(WaitForRoll());
	}

	IEnumerator WaitForRoll(){
		int count=0;
		while(count<20){
			yield return new WaitForSeconds(0.25f);
			score = Random.Range(1,1001);
			txtScore.text = score.ToString();
			count++;
		}
		ShowScore();
	}

	public void ShowScore(){
		buttonNext.SetActive(true);
		txtScore.text = score.ToString();
		URL = "http://localhost:8081/user/add/score?name=" + currentPlayer.name + "&score=" + score.ToString();
		try {
			HttpWebRequest request =(HttpWebRequest)WebRequest.Create(URL);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream stream = response.GetResponseStream();
			string responseBody = new StreamReader(stream).ReadToEnd();

			Debug.Log(responseBody);
			ErrorLog.instance.ShowError("Update Your Score!!");

		}
		catch(WebException ex){
			Debug.LogError(ex);
			ErrorLog.instance.ShowError("Error: Malfunctioning program...");
		}
	}

	public void Top10Players(){
		RankManager.instance.PhaseStart();
		PhaseStop();
	}

}
