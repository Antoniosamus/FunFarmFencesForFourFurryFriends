using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Runner))]

public class FarmerController : MonoBehaviour {


	[SerializeField] List<GameObject> points;
	[SerializeField] private Queue<Vector2> route;
	//IRouteTracer tracer; 
	Runner runner;

	public delegate void CollideWithObstacle(Collision2D collision);
	public event CollideWithObstacle OnCollideWithObstacle;

	public delegate void CollideWithWayPoint(Collision2D collision);
	public event CollideWithWayPoint OnCollideWithWayPoint;

	[SerializeField] GameObject pointPrefab;

	public void Awake()
	{
		if (runner == null) 
		{
			runner = GetComponent<Runner>();
		}

		route = new Queue<Vector2> ();
		foreach (GameObject vector in points) 
		{
			//Instantiate(pointPrefab, vector, Quaternion.identity);
			Vector2 vectorAux = (Vector2)vector.transform.position;
			route.Enqueue(vectorAux);
		}
	}

	public void OnEnable()
	{
		//tracer.OnRouteStart += HandleOnRouteStart;
		runner.OnCollisionAppears += OnRunnerCollision;
		FollowRoute ();
	}

	public void OnDisable()
	{
		//tracer.OnRouteStart -= HandleOnRouteStart;
		runner.OnCollisionAppears -= OnRunnerCollision;
	}
	
	public void FollowRoute ()
	{
		Debug.Log ("Hello");

		GoToNextPoint (route.Dequeue());
	}

	public void GoToNextPoint(Vector3 point)
	{
		runner.GoToNextPoint (point);
	}

	private void OnRunnerCollision (Collision2D collision)
	{
		string collisionName = LayerMask.LayerToName (collision.collider.gameObject.layer);

		if (collisionName == "Obstacle") 
		{
			if (OnCollideWithObstacle != null)
			{
				OnCollideWithObstacle(collision);
			}
		}
		else if (collisionName == "Waypoint")
			{
				FollowRoute();

				if (OnCollideWithWayPoint != null)
				{
					OnCollideWithWayPoint(collision);
				}
			}


		route.Clear ();
	}

	private void HandleOnRouteStart (Vector3 obj)
	{
		FollowRoute ();
	}




}
