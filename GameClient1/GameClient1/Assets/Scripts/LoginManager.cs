using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using TMPro;

public class LoginManager : MonoBehaviour {

	public GameObject panel;
	public TMP_InputField txtName, txtPass;
	string URL;
	public static LoginManager instance;

	void Awake(){
		if(instance==null){
			instance = this;
		}else{
			if(instance!=this){
				Destroy(gameObject);
			}
		}
	}

	public void PhaseStart(){
		panel.SetActive(true);
	}

	public void PhaseStop(){
		panel.SetActive(false);
	}

	public void Login () {
		if(txtName.text.Trim().Length == 0 || txtPass.text.Trim().Length == 0){
			ErrorLog.instance.ShowError("Please fill in the blank.");
			return;
		}
		if(txtName.text.Trim().Length < 3){
			ErrorLog.instance.ShowError("Username at least 3 characters.");
			return;
		}
		if(txtPass.text.Trim().Length < 6){
			ErrorLog.instance.ShowError("Password at least 6 characters.");
			return;
		}
		URL = "http://localhost:8081/user/" + txtName.text.Trim();
		try {
			HttpWebRequest request =(HttpWebRequest)WebRequest.Create(URL);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream stream = response.GetResponseStream();
			string responseBody = new StreamReader(stream).ReadToEnd();

			Debug.Log(responseBody);

			Player[] players = JsonConvert.DeserializeObject<Player[]>(responseBody);
			if(players.Length==0){
				ErrorLog.instance.ShowError("Wrong Username!!");
				return;
			}
			if(players[0].password==txtPass.text.Trim()){
				ErrorLog.instance.ShowError("Login Complete!!");
				GameManager.instance.PhaseStart(players[0]);
				PhaseStop();
			}else{
				ErrorLog.instance.ShowError("Wrong Password!!");
			}
		}
		catch(WebException ex){
			Debug.LogError(ex);
			ErrorLog.instance.ShowError("Error: Cannot find this user");
		}
	}

	public void Register () {
		if(txtName.text.Trim().Length == 0 || txtPass.text.Trim().Length == 0){
			ErrorLog.instance.ShowError("Please fill in the blank.");
			return;
		}
		if(txtName.text.Trim().Length < 3){
			ErrorLog.instance.ShowError("Username at least 3 characters.");
			return;
		}
		if(txtPass.text.Trim().Length < 6){
			ErrorLog.instance.ShowError("Password at least 6 characters.");
			return;
		}
		URL = "http://localhost:8081/user/add/user?name=" + txtName.text.Trim() + "&pass=" + txtPass.text.Trim();
		try {
			HttpWebRequest request =(HttpWebRequest)WebRequest.Create(URL);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream stream = response.GetResponseStream();
			string responseBody = new StreamReader(stream).ReadToEnd();

			Debug.Log(responseBody);
			ErrorLog.instance.ShowError("Register Complete!!");

		}
		catch(WebException ex){
			Debug.LogError(ex);
			ErrorLog.instance.ShowError("Error: This name already exist.");
		}
	}

}
