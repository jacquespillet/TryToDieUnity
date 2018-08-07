using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, UsableItem {
	protected  Vector3 initialScale;

	public float weight;
	public abstract void beUsed();
}