using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDecoration : MonoBehaviour {

    public LayerMask terrainMask;

    public Vector3 stickDirection;

    public void Setup(Vector3 dir) {
        stickDirection = dir;
    }

    public void Stick() {
    	RaycastHit hit;
    	if (Physics.Raycast(transform.position, stickDirection, out hit, 3f, terrainMask)) {
    		transform.position = hit.point;
    		transform.up = hit.normal;
    	}
    	else {
    		Debug.Log("No Terrain !");
            Destroy(gameObject);
    	}
    }

}
