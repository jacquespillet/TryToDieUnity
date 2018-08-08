using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : Item {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void beUsed() {
		Debug.Log("c'est moi le book");
	}

}
