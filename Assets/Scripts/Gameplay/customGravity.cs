using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Gameplay;

public enum GravityType
{
	Default, Custom
}
public class customGravity : MonoBehaviour
{
	//[System.Serializable]
	public GravityType gravityType;

    [HideInInspector]
    Vector3 gravityDir;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void manageGrav(Ball ball)
	{
        if (gravityType == GravityType.Custom) {
            ball.rb.useGravity = false;
            ball.gravDir = transform.TransformDirection(Vector3.down);
            Debug.Log(ball.gravDir);
        } else {
            ball.rb.useGravity = true;
            ball.gravDir = Vector3.down;
        }
	}
    /*
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
    }
    */
}
