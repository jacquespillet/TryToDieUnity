using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immolator : MonoBehaviour {

	private int counterExplosive;
	// Use this for initialization
	void Start () {
		this.counterExplosive = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(this.counterExplosive >= 10) {
			Debug.Log("Ca brule memene");
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(!other.gameObject.GetComponent<Book>().Equals(null)) {
			this.counterExplosive++;
			Debug.Log("FIRE IN THE HOLE!!!!!");
		}
	}
}
