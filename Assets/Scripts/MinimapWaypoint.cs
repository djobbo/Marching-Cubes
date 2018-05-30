using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapWaypoint : MonoBehaviour {

    public AnimationCurve curve;

    public Slider slider;
    public Image img;
    public Transform target;
    public Transform player;

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        var forward = player.forward;
        var vector = target.position - player.position;
        var angle = Vector3.SignedAngle(forward, vector, player.up);
        slider.value = curve.Evaluate(angle);
    }

    public static float InOut(float k)
    {
        if (k == 0f) return 0f;
        if (k == 1f) return 1f;
        if ((k *= 2f) < 1f) return 0.5f * Mathf.Pow(1024f, k - 1f);
        return 0.5f * (-Mathf.Pow(2f, -10f * (k - 1f)) + 2f);
    }

    public float NormalLaw(float k) {
        return Mathf.Exp(-Mathf.Pow(k, 2));
    }

}
