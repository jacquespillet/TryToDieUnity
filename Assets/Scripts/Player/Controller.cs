using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	// Test commit stan
	// Mouse and keys
	private float x, y;
	public bool willDie;
	public GameObject coffin;
	public float MAX_FORWARD_Y = 0.80f;
	public float CONST_SPEED = 10f;
	public float MAX_WEIGHT = 10f;
	public float CONST_JUMP = 20f;
	private  DivingSystem divingSystem;

	private Vector3 rotation;
	private int numDeath;
	public CapsuleCollider collider;
	// hold and object
	private bool hasObject = false;
	private Item currentObject;

	// User speed
	private float speed;
	private AudioSource audioSource;
	public CursorLockMode wantedMode;

	void Start () {
		Cursor.lockState = wantedMode;
		this.speed = this.CONST_SPEED;
		this.divingSystem = this.GetComponent<DivingSystem>();
		this.audioSource = this.GetComponent<AudioSource>();
		this.willDie = false;
		audioSource.Play();
	}
	
	void Update () {
		// Get directionals pushed keys and move
		float horizontaltranslation = Input.GetAxis("Horizontal");
		float verticalTranslation =  -Input.GetAxis("Vertical");
		this.GetComponent<Rigidbody>().velocity = new Vector3(this.transform.forward.x* verticalTranslation * this.speed, this.GetComponent<Rigidbody>().velocity.y, this.transform.forward.z* verticalTranslation* this.speed);
		this.GetComponent<Rigidbody>().velocity += this.transform.right * horizontaltranslation * this.speed;
		this.collider.transform.eulerAngles = new Vector3(0f, 0f, 0f);


		// Get mouse movment and rotate
		this.y = Input.GetAxis("Mouse X");
		this.x = Input.GetAxis("Mouse Y");
		this.rotation = new Vector3(x, y * -1, 0 );
		transform.eulerAngles = transform.eulerAngles - this.rotation;
		if(this.transform.forward.y > MAX_FORWARD_Y) {
			this.transform.forward = new Vector3(this.transform.forward.x, MAX_FORWARD_Y, this.transform.forward.z);
		} else if (this.transform.forward.y < -MAX_FORWARD_Y) {
			this.transform.forward = new Vector3(this.transform.forward.x, -MAX_FORWARD_Y, this.transform.forward.z);

		}

		// Object management
		if(!this.hasObject) {
			// Check if there's an object to catch
			checkCatchableObject();
		} else {
			// Make the current object follows the camera in a smoothie way
			this.currentObject.gameObject.transform.position = this.transform.position + this.transform.forward * 0.5f + this.transform.right * 0.2f;
			
			this.currentObject.gameObject.transform.eulerAngles = 	this.currentObject.gameObject.transform.eulerAngles - this.rotation;
			this.currentObject.gameObject.transform.localEulerAngles = new Vector3(0f, 90f, 90f);
			if(Input.GetKeyDown(KeyCode.E)){
				// Release the object if E is pressed
				releaseObject();
			} else if (Input.GetKeyDown(KeyCode.F)){
				//Throw the object if F is pressed
				throwObject();
			}
			if (Input.GetMouseButtonDown(0)) {
				this.currentObject.beUsed();
			}
			checkPainting();
		}

		// Respawn
		if(Input.GetKeyDown(KeyCode.P)) {
			 Application.LoadLevel(Application.loadedLevel);
		}

		// Jump
		if (Input.GetKeyDown(KeyCode.Space)){
			this.GetComponent<Rigidbody>().velocity = new Vector3(this.GetComponent<Rigidbody>().velocity.x, this.GetComponent<Rigidbody>().velocity.y + this.CONST_JUMP, this.GetComponent<Rigidbody>().velocity.z);

		}

		if (this.gameObject.GetComponent<Rigidbody>().velocity.y < -5f){
			this.willDie = true;
		}
	}

	void checkCatchableObject() {
		RaycastHit hit;
		// If the raycast find an object with the tag catchatchable
		if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, 10f, 256)) {

			if(hit.transform.gameObject.tag == "Lader" && Input.GetAxis("Vertical") < 0 && hit.distance < 1f) {
				this.transform.GetComponent<Rigidbody>().velocity = new Vector3(0f, 2.5f, 0f);

				//Mettre le collider de l'échelle trigger et garder l'autre collider
			}
			if(hit.transform.gameObject.tag == "Catchable") {
				// Si on a pas d'objet dans les mains on le choppe poto
				if(Input.GetKeyDown(KeyCode.E)) {
					this.hasObject = true;
					this.currentObject = hit.transform.gameObject.GetComponent<Item>();
					this.speed = 0.75f * this.CONST_SPEED;
					this.currentObject.transform.SetParent(this.transform);
				}
			}
			if(hit.transform.gameObject.tag == "Lever") {
				if(Input.GetKeyDown(KeyCode.E)) {
					hit.transform.gameObject.GetComponent<Animator>().SetTrigger("isPulled");
					this.divingSystem.launch();
				}
			}
		}
	}

	void checkPainting() {
		RaycastHit hit2;
		if(Physics.Raycast(this.transform.position + new Vector3(0f, 0.5f, 0f), this.transform.forward, out hit2, 10f, 256)) {
			if(hit2.transform.gameObject.tag == "painting") {
				if(Input.GetMouseButton(0)) {
					if(hit2.transform.childCount==0) {
						this.hasObject = false;
						this.currentObject.gameObject.transform.position = hit2.transform.position + hit2.transform.right * 0.1f;
						this.currentObject.transform.forward = hit2.transform.forward;
						this.currentObject.GetComponent<Rigidbody>().isKinematic = true;
						this.currentObject.transform.SetParent(hit2.transform);
						this.speed = this.CONST_SPEED;
						this.currentObject = null;
					}
				}
				// hit2.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
			}
		}
	}

	void throwObject() {
		this.currentObject.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(this.transform.forward.x, this.transform.forward.y, this.transform.forward.z)*(this.MAX_WEIGHT - this.currentObject.weight);
		releaseObject();
	}

	void releaseObject() {
		this.hasObject = false;
		this.currentObject.gameObject.transform.position = this.transform.position + new Vector3(this.transform.forward.x,this.transform.forward.y + 0.3f,this.transform.forward.z) * 1.0f;
		this.currentObject.transform.SetParent(this.transform.parent);
		this.currentObject.GetComponent<Rigidbody>().isKinematic = false;
		this.speed = this.CONST_SPEED;
		this.currentObject = null;
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Death1") {
			die();
		}
	}

	public void die() {
		this.willDie = false;
		this.numDeath++;
		instantiateNewCoffin();
		this.transform.position = new Vector3(0f, 1f, 0f);
	}

	public void instantiateNewCoffin(){
		var newCoffin = Instantiate(this.coffin, new Vector3(2.0f, 1f, 2.0f), Quaternion.identity);
		newCoffin.transform.SetParent(this.transform.parent);
		newCoffin.transform.localScale = new Vector3(1f,1f,1f);
	}
}
