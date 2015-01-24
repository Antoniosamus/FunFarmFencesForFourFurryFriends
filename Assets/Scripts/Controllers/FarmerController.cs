using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Runner))]
public class FarmerController : RouteFollower, IFarmerEvents
{
  [SerializeField]
	private Runner _runner;
	private Queue<Vector2> _currentRoute = new Queue<Vector2>();

  [SerializeField]
  private float _routeStartDelay = 1f;

  //--------------------------------------------------------------
  
  #region IFarmerEvents
  public event CollisionHandler OnCollideWithObstacle;
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
		_runner.OnRouteInterrupt += OnRouteRouteInterrupt;
    _runner.OnTargetReach += OnRouteStepReach;
	}
  //------------------------------------------------------

	protected override void OnDisable()
	{
		_runner.OnRouteInterrupt -= OnRouteRouteInterrupt;
    _runner.OnTargetReach -= OnRouteStepReach;
    base.OnDisable();
	}

  #endregion

  //================================================================
  
  #region RouteFollower
  protected override void OnRouteStart(Vector2 startPosition)
  {
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

  #region EVENT HANDLERS

  private void OnRouteStepReach()
  {
	  FollowRoute();
  }

  //----------------------------------------
  
  private void OnRouteRouteInterrupt (GameObject other)
	{
	  switch(LayerMask.LayerToName(other.layer))
	  {
	    case "Obstacle":
	      if (OnCollideWithObstacle != null)
	        OnCollideWithObstacle(other);
	      _currentRoute.Clear ();
	      break;

      case "Animal":
        if(OnCollideWithAnimal != null)
          OnCollideWithAnimal(other);
        break;
	  }
	}

  #endregion

  //==================================================================


  // TODO Corrutina...? Mejorar bucles....
  public void FollowRoute()
	{
    //while(_currentRoute.Count > 0 && _currentRoute.Peek() == _runner.Target)
    //  _currentRoute.Dequeue();

    if(_currentRoute.Count > 0)
		  _runner.Target = _currentRoute.Dequeue();

    // TODO OnRouteComplete
	}

  //-------------------------------------------------

	


  


  

  
}
