using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]

public class Runner : MonoBehaviour{

	[SerializeField] Vector2 forwardDirection;

	public delegate void OnCollision(Collider2D collision);
	public event OnCollision OnCollisionAppears;

	public int angVelModule = 3;

	Vector3 target;


	public void OnEnable()
	{
	
	}

	public void GoToNextPoint (Vector3 point)
	{
		target = point;

		Walk (point);
	}
	
	void Walk (Vector3 point)
	{
		Vector3 vel = this.transform.right;
	}

	void FixedUpdate()
	{
		Vector3 direction = target - this.transform.position;
		Vector3 result = Vector3.Cross (forwardDirection, direction);

		rigidbody2D.angularVelocity = angVelModule * result.z;

		rigidbody2D.velocity = Vector3.Normalize(direction) * angVelModule;
	}

	

	public void CollisionWith (WayPoint wayPoint)
	{
		Stop (wayPoint.collider2D);
	}

	private void Stop (Collider2D collision)
	{
		rigidbody2D.velocity = Vector2.zero;

		if (OnCollisionAppears != null) 
		{
			OnCollisionAppears(collision);
		}
	}
}
