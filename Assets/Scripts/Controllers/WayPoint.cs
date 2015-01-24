using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour {


	private Runner owner; 
	public Runner Owner
	{
		set
		{
			owner = value;
		}

		get
		{
			return owner;
		}
	}



	private void OnTriggerEnter2D(Collider2D other) {
		owner.CollisionWith (this);
	}
	

}
