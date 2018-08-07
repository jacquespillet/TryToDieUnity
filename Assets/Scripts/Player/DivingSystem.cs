using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivingSystem : MonoBehaviour {
	public GameObject diving, slab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void launch() {
		slab.GetComponent<Animator>().SetTrigger("isOpen");
		diving.GetComponent<Animator>().SetTrigger("isLifting");
	}
}
