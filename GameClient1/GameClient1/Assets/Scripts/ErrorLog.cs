using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ErrorLog : MonoBehaviour {

	public TMP_Text txtError;
	public static ErrorLog instance;

	void Awake(){
		if(instance==null){
			instance = this;
		}else{
			if(instance!=this){
				Destroy(gameObject);
			}
		}
	}

	void Start(){
		ClearError();
	}

	public void ShowError(string text){
		txtError.text = text;
		StopAllCoroutines();
		StartCoroutine(Clear());
	}

	IEnumerator Clear(){
		yield return new WaitForSeconds(3f);
		ClearError();
	}

	public void ClearError(){
		txtError.text = "";
	}

}
