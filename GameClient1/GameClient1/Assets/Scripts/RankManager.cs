using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using TMPro;

public class RankManager : MonoBehaviour {

	public GameObject panel;
	string URL;
	public static RankManager instance;
	public Transform rankParent;

	void Awake(){
		if(instance==null){
			instance = this;
		}else{
			if(instance!=null){
				Destroy(gameObject);
			}
		}
	}

	public void PhaseStart(){
		panel.SetActive(true);
		QueryUsers();
	}

	void QueryUsers(){
		URL = "http://localhost:8081/top10users/";
		try {
			HttpWebRequest request =(HttpWebRequest)WebRequest.Create(URL);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream stream = response.GetResponseStream();
			string responseBody = new StreamReader(stream).ReadToEnd();

			Debug.Log(responseBody);

			Player[] players = JsonConvert.DeserializeObject<Player[]>(responseBody);
			for(int i=0; i<10; i++){
				if(i<players.Length){
					ShowScore(i,players[i]);
				}else{
					HideScore(i);
				}
			}
		}
		catch(WebException ex){
			Debug.LogError(ex);
			ErrorLog.instance.ShowError("Error: Malfunctioning program...");
		}
	}

	public void PhaseStop(){
		panel.SetActive(false);
	}

	public void ShowScore(int index, Player data){
		rankParent.GetChild(index).GetComponent<TMP_Text>().text = (index+1).ToString();
		rankParent.GetChild(index).GetChild(0).GetComponent<TMP_Text>().text = data.name;
		rankParent.GetChild(index).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = data.score.ToString();
	}

	public void HideScore(int index){
		rankParent.GetChild(index).GetComponent<TMP_Text>().text = "";
		rankParent.GetChild(index).GetChild(0).GetComponent<TMP_Text>().text = "";
		rankParent.GetChild(index).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "";
	}

	public void Logout(){
		LoginManager.instance.PhaseStart();
		PhaseStop();
	}


}
