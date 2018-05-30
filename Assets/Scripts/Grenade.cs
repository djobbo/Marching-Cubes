using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    public float explosionRadius;
    public float explosionForce;
    List<Transform> objectsInRange;

    public float delay = 2f;
    float countdown;
    bool hasExploded = false;

    public GameObject explosionEffect;

    void Awake() {
    	countdown = delay;
    	objectsInRange = new List<Transform>();
    }

    void OnTriggerEnter(Collider other) {
    	var tr = other.transform;
    	if (!objectsInRange.Contains(tr))
    		objectsInRange.Add(tr);
    }

    void OnTriggerExit(Collider other) {
    	var tr = other.transform;
    	if (objectsInRange.Contains(tr))
    		objectsInRange.Remove(tr);
    }

    void Update() {
    	countdown -= Time.deltaTime;
    	if (countdown <= 0 && !hasExploded) {
    		Explode();
    		hasExploded = true;
    	}
    }

    void Explode() {
    	Debug.Log("Explode !");
    	for (int i = 0; i < objectsInRange.Count; i++) 
        {
            var tr = objectsInRange[i];

            if (tr!= null)
        		if (tr.CompareTag("Chunk")) {
        			tr.parent.GetComponent<MarchingMeshGenerator>().Deform(transform.position, explosionForce, explosionRadius, true);
        		}
    	}
    	Destroy(Instantiate(explosionEffect, transform.position, transform.rotation), 5f);

    	Destroy(gameObject);
    }

}
