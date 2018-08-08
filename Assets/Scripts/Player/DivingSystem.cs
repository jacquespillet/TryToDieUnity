using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivingSystem : MonoBehaviour {
	public GameObject diving, slab, lever;
	public List<PaintingContainer> paintings;
	bool unlocked = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		unlocked = checkPaintings();
		if(unlocked) {
			unlockLever();
		}
	}

	private bool checkPaintings() {
        Debug.Log(paintings.Count);
		for(int i=0; i< paintings.Count; i++) {
			if(paintings[i].gameObject.transform.childCount ==0) {
				return false;
			} else if(paintings[i].id != paintings[i].transform.GetChild(0).GetComponent<Painting>().id){
				return false;
			}
		}
		return true;
	}

	public void launch() {
		slab.GetComponent<Animator>().SetTrigger("isOpen");
		diving.GetComponent<Animator>().SetTrigger("isLifting");
	}

	public void unlockLever() {
		this.lever.SetActive(true);
	}
}
