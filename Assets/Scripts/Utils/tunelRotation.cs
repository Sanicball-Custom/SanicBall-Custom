using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tunelRotation : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
        bool b = (Random.value > 0.5f);
		speed = b  ? speed : -speed;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
	}
}
