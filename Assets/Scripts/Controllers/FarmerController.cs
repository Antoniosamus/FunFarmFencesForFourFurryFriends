using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Runner))]
public class FarmerController : RouteFollower, IFarmerEvents
{
  [SerializeField]
	private Runner _runner;
	private Queue<Vector3> _currentRoute = new Queue<Vector3>();

  [SerializeField]
  private float _routeStartDelay = 1f;

  //--------------------------------------------------------------
  
  #region IFarmerEvents
  public event CollisionHandler OnCollideWithObstacle;
  public event CollisionHandler OnCollideWithWayPoint;
  public event CollisionHandler OnCollideWithAnimal;
  #endregion



  //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
  //////////////////////////////////////////////////////////////////////


  #region MONO

  protected override void Awake()
  {
    base.Awake();
    if (_runner == null) 
			_runner = GetComponent<Runner>();
  }

  //------------------------------------------------------

  protected override void OnDestoy()
  {
    _runner = null;
    base.OnDestoy();
  }

  //--------------------------------------------------------

  protected override void OnEnable()
	{
    base.OnEnable();
		_runner.OnCollision += OnRunnerCollision;
		FollowRoute();
	}
  //------------------------------------------------------

	protected override void OnDisable()
	{
		_runner.OnCollision -= OnRunnerCollision;
    base.OnDisable();
	}

  #endregion

  //================================================================
  
  #region RouteFollower
  protected override void OnRouteStart(Vector2 startPosition)
  {
    _currentRoute.Enqueue(startPosition);
    Invoke("FollowRoute", _routeStartDelay);
  }

  //----------------------------------------------------------
  protected override void OnRouteStay(Vector2 currentPosition)
  {
    _currentRoute.Enqueue(currentPosition);
  }
  //----------------------------------------------------------
  protected override void OnRouteStop(Queue<Vector2> route)
  {
     // 
  }
  //----------------------------------------------------------
  protected override void OnRouteCancel()
  {
    _currentRoute.Clear();
  }
  
  #endregion


  //===============================================


  // TODO Corrutina...?
  public void FollowRoute ()
	{
    if(_currentRoute.Count > 0)
		  _runner.Target = _currentRoute.Dequeue();
	}

  //-------------------------------------------------

	private void OnRunnerCollision (GameObject other)
	{
	  switch(LayerMask.LayerToName(other.layer))
	  {
	    case "Obstacle":
	      if (OnCollideWithObstacle != null)
	        OnCollideWithObstacle(other);
	      _currentRoute.Clear ();
	      break;

	    case "Waypoint":
        if(collider.GetComponent<RoutePoint>().RouteOwner == this) {
	        FollowRoute();
	        if (OnCollideWithWayPoint != null)
	          OnCollideWithWayPoint(other);
        }
	      break;

      case "Animal":
        if(OnCollideWithAnimal != null)
          OnCollideWithAnimal(other);
        break;
	  }
	}


  //==================================================================


  

  
}
