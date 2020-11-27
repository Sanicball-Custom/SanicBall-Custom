using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Gameplay;

public enum GravityType
{
	Default, Custom
}
[ExecuteAlways]
public class customGravity : MonoBehaviour
{
	//[System.Serializable]
	public GravityType gravityType;
    Vector3 Normal;
    RaycastHit hit;

    [HideInInspector]
    Vector3 gravityDir;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Physics.Raycast(transform.position, -transform.up, out hit, 10000))
        {
            Normal = hit.normal;
        }
    }

	public void manageGrav(Ball ball)
	{
        if (gravityType == GravityType.Custom) {
            ball.rb.useGravity = false;
            ball.gravDir = Normal;
            Debug.Log(ball.gravDir);
        } else {
            ball.GetComponent<Rigidbody>().useGravity = true;
            ball.gravDir = Vector3.down;
        }
	}
    /*
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
    }
    */

    public void OnDrawGizmosSelected()
    {
        if (gravityType == GravityType.Custom)
        {
            Gizmos.DrawSphere(hit.point, 5);
            Gizmos.DrawRay(hit.point, hit.normal * 10);
            //Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
