using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FarmerController : MonoBehaviour {

	[SerializeField] private Queue<Vector3> route;

	IRouteTracer tracer; 

	public void OnEnable()
	{
		tracer.OnRouteStart += HandleOnRouteStart;
	}
	
	public void OnDisable()
	{
		tracer.OnRouteStart -= HandleOnRouteStart;
	}

	
	public void FollowRoute ()
	{
		
	}

	void HandleOnRouteStart (Vector3 obj)
	{
		
	}




}
