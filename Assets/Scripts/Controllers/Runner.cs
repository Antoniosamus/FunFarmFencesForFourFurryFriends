using System;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode(), RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Runner : MonoBehaviour
{
  #region Compos
  private CircleCollider2D _nextRoutePoint;
   private CircleCollider2D _runnerCollider;
  #endregion

  //-------------------------------------------------
  #region Events
  public event Action<GameObject> OnRouteInterrupt;
  public event Action OnTargetReach;
  #endregion

  //------------------------------

  [SerializeField]
  private float _routePointRadius = 0.1f;
  private float _targetDistanceMinSqr;

	[SerializeField] 
  Vector2 forwardDirection;
  [SerializeField]
  private float _angularVelocityAbs = 3;
  [SerializeField]
  private float _linearVelocityAbs = 3;
 
  //------------------------------------------------------

  private Vector2 _target;
	public Vector2 Target 
  { 
    get { return _target; }
    set 
    {
      if( (value - _target).sqrMagnitude < _targetDistanceMinSqr) 
      {
        TargetReach();
      }
      else 
      {
        IsFollowingTarget = true;
        _target = value;

        if(_nextRoutePoint == null) {
          _nextRoutePoint = new GameObject(name + "RoutePoint").AddComponent<CircleCollider2D>();
          _nextRoutePoint.radius = _routePointRadius;
          _nextRoutePoint.isTrigger = true;
        }
        _nextRoutePoint.transform.position = _target;
      }
    }
  }

  //-------------------------------------------

  private bool _isFollowingTarget;
  private bool IsFollowingTarget 
  { 
    get { return _isFollowingTarget; }
    set 
    {
      _isFollowingTarget = value;
      if(!_isFollowingTarget) 
      {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0f;
        if(_nextRoutePoint != null)
          _nextRoutePoint.transform.position = transform.position;
      }
    }
  }
 


  //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
  /////////////////////////////////////////////////////////////




  #region MONO

  private void  Awake()
  {
    rigidbody2D.isKinematic = false;
    rigidbody2D.gravityScale = 0f;
    _runnerCollider = (collider2D as CircleCollider2D);
    _targetDistanceMinSqr = (float) Math.Pow(_runnerCollider.radius + _routePointRadius, 2);
  }

  //--------------------------------

  private void OnDestroy()
  {
     _runnerCollider = null;
     _nextRoutePoint = null;
     OnRouteInterrupt = null;
     OnTargetReach = null;
  }

  //-------------------------------------

  private void  OnEnable()
	{
	  // TODO
	}

  //----------------------------------------------

	private void FixedUpdate()
	{
    if(!IsFollowingTarget)
      return;

		Vector2 direction = Target - (Vector2) transform.position;
    if(direction.sqrMagnitude < _targetDistanceMinSqr)
      TargetReach();

		float reorientation = Vector3.Cross(forwardDirection, direction).z;
    rigidbody2D.velocity = Vector3.Normalize(direction) * _linearVelocityAbs;
		rigidbody2D.angularVelocity = _angularVelocityAbs * reorientation;
	}

  //---------------------------------------

  private void OnTriggerEnter2D(Collider2D other) 
  {
    if(_nextRoutePoint != null 
      && other.gameObject == _nextRoutePoint.gameObject)
      TargetReach();
  }
  
  //-----------------------------------------

  private void OnCollisionEnter2D(Collision2D other)
  {
    IsFollowingTarget = false;
    RouterInterrupt(other.gameObject);
  }

  

  #endregion

  //=============================================================================

  #region EVENTS
  protected virtual void TargetReach()
  {
    IsFollowingTarget = false;

    Action e = OnTargetReach;
    if(e != null)
      e();
  }

  //----------------------------------------------
  protected virtual void RouterInterrupt(GameObject other)
  {
    Action<GameObject> e = OnRouteInterrupt;
    if(e != null)
      e(other);
  }
  #endregion


  //====================================================

  public void Stop()
  {
    IsFollowingTarget = false;
  }


}
