using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Runner))]
public class FarmerController : RouteFollower
{
  [SerializeField]
	public Runner _runner;
	public Queue<Vector2> _currentRoute = new Queue<Vector2>();

    public bool _canFarm = true;

  [SerializeField]
  private float _routeStartDelay = 1f;


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
  protected override void OnRouteStay(Vector2 currentPosition)
  {
     if (_currentRoute.Count == 0)
      Invoke("FollowRoute", _routeStartDelay);
    _currentRoute.Enqueue(currentPosition);
  }
  //----------------------------------------------------------
  protected override void OnRouteCancel()
  {
    _currentRoute.Clear();
  }
  //---------------------------------------------------
  protected override void OnRouteStart(Vector2 startPosition)
  {
  }

  //---------------------------------------------------

  protected override void OnRouteStop(Queue<Vector2> route)
  {
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
    if(_runner.IsFollowingTarget) {
	    _currentRoute.Clear();
      _runner.Stop();
    }
	}

  #endregion

  //==================================================================


  // TODO Corrutina...? Mejorar bucles....
  public void FollowRoute()
	{
    if(_currentRoute.Count > 0)
		  _runner.Target = _currentRoute.Dequeue();

    // TODO OnRouteComplete
	}

  //-------------------------------------------------











  
}
