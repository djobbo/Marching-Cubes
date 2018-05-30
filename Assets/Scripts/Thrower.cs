using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour {

	public Camera cam;

	public void ThrowItem(Item throwable) {

		var go = Instantiate(throwable.prefab, transform.position + cam.transform.forward, Quaternion.identity);
		var rb = go.GetComponent<Rigidbody>();
		rb.AddForce(cam.transform.forward * 20f, ForceMode.Impulse);
	}

}
