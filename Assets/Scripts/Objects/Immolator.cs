using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immolator : MonoBehaviour {
	public GameObject particles;
	private int counterExplosive;
	public int CONST_MAX_COUNTER; 
	private float CONST_TIMER = 20f;
	private float timer;
	// Use this for initialization
	void Start () {
		this.counterExplosive = 0;
		this.timer = this.CONST_TIMER;
		this.CONST_MAX_COUNTER = 1;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer < 0 && this.counterExplosive > 0) {
			this.counterExplosive--;
			for(int i=0; i<this.particles.transform.childCount; i++) {
				ParticleSystem system = this.particles.transform.GetChild(i).GetComponent<ParticleSystem>();
				system.startSize--;
			}
			this.timer = this.CONST_TIMER;
		}
		if(this.counterExplosive >= this.CONST_MAX_COUNTER) {	
			this.transform.GetChild(1).GetComponent<Animator>().SetTrigger("isFinish");
			this.transform.GetChild(2).GetComponent<Animator>().SetTrigger("isFinish");
			this.counterExplosive = 0;
		}			
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other);
		if(other.gameObject.GetComponent<Book>() != null) {
			this.counterExplosive++;
			Destroy(other.gameObject);
			this.timer = this.CONST_TIMER;
			for(int i=0; i<this.particles.transform.childCount; i++) {
				ParticleSystem system = this.particles.transform.GetChild(i).GetComponent<ParticleSystem>();
				system.startSize++;
			}
		} else if (other.gameObject.GetComponent<Controller>() != null) {
			other.gameObject.GetComponent<Controller>().die();
			for(int i=0; i<this.particles.transform.childCount; i++) {
				ParticleSystem system = this.particles.transform.GetChild(i).GetComponent<ParticleSystem>();
				system.startSize = 0;
			}
		}
	}
}
