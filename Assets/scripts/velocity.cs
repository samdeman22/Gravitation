using UnityEngine;
using System.Collections;

public class velocity : MonoBehaviour {

	public Vector3 v = Vector3.zero;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().AddForce (v.x, v.y, v.z, ForceMode.VelocityChange);
	}
}
