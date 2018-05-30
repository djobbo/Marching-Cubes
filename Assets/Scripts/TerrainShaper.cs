using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainShaper : MonoBehaviour {

    public Camera cam;
    public ChunkGenerator chunkGen;

    [Header("Chunk Shaper")]
    public float brushRadius = 2f;
    public float brushGravity = 1f;
    public float brushDensity = 1f;
    public float maxDistance = 12f;
    public float cubeSize;

    public Transform shaperProjector;
    public GameObject digParticles;

    public AnimationCurve deformCurve;

    private void Start()
    {
        chunkGen = ChunkGenerator.singleton;
    }

    public void ShapeTerrain()
    {
        bool dug = false;
        bool activateProjector = false;

        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0)), out hit, maxDistance))
        {
            var hitTr = hit.transform;
            var dig = Input.GetKey(KeyCode.LeftShift);

            if (!hitTr.parent || (!dig && Vector3.Distance(transform.position, hit.point) <= brushRadius)) return;
            hitTr = hitTr.parent;
            if (hitTr.CompareTag("Chunk"))
            {
                activateProjector = true;
                shaperProjector.rotation = Quaternion.LookRotation(cam.transform.forward);
                shaperProjector.position = hit.point - cam.transform.forward * .2f;

                if (Input.GetMouseButton(0))
                {
                    dug = true;
                    digParticles.transform.position = hit.point;

                    var chunk = hitTr.GetComponent<MarchingMeshGenerator>();

                    chunk.Deform(hit.point, brushGravity, brushRadius, dig, deformCurve);
                }
            }
        }

        digParticles.SetActive(dug);
        //shaperProjector.gameObject.SetActive(activateProjector);
    }

}
