using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, UsableItem {
	public float weight;
	public abstract void beUsed();
}