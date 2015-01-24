using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]

public class Runner : MonoBehaviour{

	Rigidbody2D rigidbody;
	BoxCollider2D collider;
	//[SerializeField] Vector2 velocity;
	
	public delegate void OnCollision(Collision2D collision);
	public event OnCollision OnCollisionAppears;


	public void OnEnable()
	{
		if (rigidbody == null) 
		{
			rigidbody = GetComponent<Rigidbody2D>();
		}
	}

	public void GoToNextPoint (Vector3 point)
	{
		transform.LookAt(point);
		Walk (point);
	}
	
	void Walk (Vector3 point)
	{
		Vector3 vel = this.transform.right;

		Debug.Log (this.transform.TransformDirection (vel));

		rigidbody.velocity = (Vector2)transform.forward;


	}

	void FixedUpdate()
	{
		rigidbody.velocity = transform.forward;
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		Stop (collision);
	}

	private void Stop (Collision2D collision)
	{
		rigidbody.velocity = Vector2.zero;

		if (OnCollisionAppears != null) 
		{
			OnCollisionAppears(collision);
		}
	}
}
