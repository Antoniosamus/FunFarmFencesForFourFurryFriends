using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RoutePoint : MonoBehaviour 
{
	public Runner RouteOwner { get; set; }

	private void OnTriggerEnter2D(Collider2D other) 
  {
    Debug.Log(name);
	}
	

}
