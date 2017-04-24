using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {

	public GameManager gameManager;
	void Awake(){
		if(GameManager.Instance == null)
		GameObject.Instantiate (gameManager);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
