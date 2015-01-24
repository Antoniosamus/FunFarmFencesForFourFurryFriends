using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Runner))]

public class FarmerController : MonoBehaviour {


	[SerializeField] List<GameObject> points;
	[SerializeField] private Queue<Vector3> route;
	//IRouteTracer tracer; 
	Runner runner;

	public delegate void CollideWithObstacle(Collider2D collision);
	public event CollideWithObstacle OnCollideWithObstacle;

	public delegate void CollideWithWayPoint(Collider2D collision);
	public event CollideWithWayPoint OnCollideWithWayPoint;

	[SerializeField] GameObject pointPrefab;

	public void Awake()
	{
		if (runner == null) 
		{
			runner = GetComponent<Runner>();
		}

		route = new Queue<Vector3> ();
		foreach (GameObject vector in points) 
		{
			//Instantiate(pointPrefab, vector, Quaternion.identity);
			Vector2 vectorAux = (Vector2)vector.transform.position;
			vector.GetComponent<WayPoint>().Owner = runner;
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
		Debug.Log (route.Count);

		Vector3 direction = route.Dequeue();
		GoToNextPoint (direction);
	}

	public void GoToNextPoint(Vector3 point)
	{
		runner.GoToNextPoint (point);
	}

	private void OnRunnerCollision (Collider2D collision)
	{
		string collisionName = LayerMask.LayerToName (collision.gameObject.layer);

		if (collisionName == "Obstacle") 
		{
			if (OnCollideWithObstacle != null)
			{
				OnCollideWithObstacle(collision);
			}
			route.Clear ();

		}
		else if (collisionName == "Waypoint")
			{
				FollowRoute();

				if (OnCollideWithWayPoint != null)
				{
					OnCollideWithWayPoint(collision);
				}
			}

	}

	private void HandleOnRouteStart (Vector3 obj)
	{
		FollowRoute ();
	}




}
