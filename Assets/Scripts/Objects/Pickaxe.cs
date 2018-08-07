using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : Item {
	// Use this for initialization
	void Start () {
		this.initialScale = this.transform.localScale; 
		this.weight = 5f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void beUsed() {
		Debug.Log("c'est moi la pioche");
	}
}
