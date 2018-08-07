using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	// Test commit stan
	// Mouse and keys
	private float x, y;
	public GameObject coffin;
	public float MAX_FORWARD_Y = 0.80f;
	public float CONST_SPEED = 10f;
	public float MAX_WEIGHT = 10f;
	public float CONST_JUMP = 20f;

	private Vector3 rotation;
	private int numDeath;
	public CapsuleCollider collider;
	// hold and object
	private bool hasObject = false;
	private Item currentObject;

	// User speed
	private float speed;
	public CursorLockMode wantedMode;

	void Start () {
		Cursor.lockState = wantedMode;
		this.speed = this.CONST_SPEED;
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
			this.currentObject.gameObject.transform.position = this.transform.position + new Vector3(this.transform.forward.x,this.transform.forward.y,this.transform.forward.z) * 0.5f + this.transform.right * 0.2f;
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
		}

		// Respawn
		if(Input.GetKeyDown(KeyCode.P)) {
			 Application.LoadLevel(Application.loadedLevel);
		}

		// Jump
		if (Input.GetKeyDown(KeyCode.Space)){
			this.GetComponent<Rigidbody>().velocity = new Vector3(this.GetComponent<Rigidbody>().velocity.x, this.GetComponent<Rigidbody>().velocity.y + this.CONST_JUMP, this.GetComponent<Rigidbody>().velocity.z);
		}
	}

	void checkCatchableObject() {
		RaycastHit hit;
		// If the raycast find an object with the tag catchatchable
		if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, 10f, 256)) {
			if(hit.transform.gameObject.tag == "Catchable") {
				// Si on a pas d'objet dans les mains on le choppe poto
				if(Input.GetKeyDown(KeyCode.E) && !this.hasObject) {
					this.hasObject = true;
					this.currentObject = hit.transform.gameObject.GetComponent<Item>();
					this.speed = 0.75f * this.CONST_SPEED;
					this.currentObject.transform.SetParent(this.transform);
				}
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
		this.numDeath++;
		Instantiate(this.coffin, new Vector3(2.0f, 1f, 2.0f), Quaternion.identity);
		this.transform.position = new Vector3(0f, 1f, 0f);
	}
}
