using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour {
	public static float  BOOK_SIZE = 1.05f;
	public GameObject book;
	public GameObject shelfs;

	// Use this for initialization
	void Start () {
		for(int i=0; i< shelfs.transform.GetChildCount(); i++) {
			GameObject shelf = shelfs.transform.GetChild(i).gameObject;
			Debug.Log(shelf.transform.lossyScale);
			// int numBooks = shelf.transform.lossyScale. / BOOK_SIZE;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
