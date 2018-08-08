using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immolator : MonoBehaviour {
	public AudioClip doorsOpen;
	private bool isOpen;
	public GameObject particles;
	private int counterExplosive;
	public int CONST_MAX_COUNTER; 
	private float CONST_TIMER = 20f;
	private float timer;
	private AudioSource audioSource;
	// Use this for initialization
	void Start () {
		this.audioSource = this.GetComponent<AudioSource>();
		this.counterExplosive = 0;
		this.timer = this.CONST_TIMER;
		this.CONST_MAX_COUNTER = 1;
		this.isOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.isOpen){
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
				this.audioSource.PlayOneShot(doorsOpen);
				this.isOpen = true;
			}			
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log(other)
		if(other.gameObject.GetComponent<Book>() != null) {
			this.counterExplosive++;
			Destroy(other.gameObject);
			this.timer = this.CONST_TIMER;
			this.audioSource.Play();
			for(int i=0; i<this.particles.transform.childCount; i++) {
				ParticleSystem system = this.particles.transform.GetChild(i).GetComponent<ParticleSystem>();
				system.startSize++;
			}
		} else if (other.gameObject.GetComponent<Controller>() != null) {
			StartCoroutine(other.gameObject.GetComponent<Controller>().die());
			for(int i=0; i<this.particles.transform.childCount; i++) {
				ParticleSystem system = this.particles.transform.GetChild(i).GetComponent<ParticleSystem>();
				system.startSize = 0;
			}
		}
	}
}
