﻿using System;
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

  private const float RoutePointRadius = 0.01f;
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
        _isFollowingTarget = true;
        _target = value;

        if(_nextRoutePoint == null) {
          _nextRoutePoint = new GameObject(name + "RoutePoint").AddComponent<CircleCollider2D>();
          _nextRoutePoint.radius = RoutePointRadius;
          _nextRoutePoint.isTrigger = true;
        }
        _nextRoutePoint.transform.position = _target;
      }
    }
  }
  private bool _isFollowingTarget;
 


  //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
  /////////////////////////////////////////////////////////////




  #region MONO

  private void  Awake()
  {
    rigidbody2D.isKinematic = false;
    rigidbody2D.gravityScale = 0f;
    _runnerCollider = (collider2D as CircleCollider2D);
    _targetDistanceMinSqr = (float) Math.Pow(_runnerCollider.radius + RoutePointRadius, 2);
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
    if(!_isFollowingTarget)
      return;

		Vector2 direction = Target - (Vector2) transform.position;
		float reorientation = Vector3.Cross(forwardDirection, direction).z;

    rigidbody2D.velocity = Vector3.Normalize(direction) * _linearVelocityAbs;
		rigidbody2D.angularVelocity = _angularVelocityAbs * reorientation;
	}

  //---------------------------------------

  private void OnTriggerEnter2D(Collider2D other) 
  {
    if(other == _nextRoutePoint)
      TargetReach();
  }
  
  //-----------------------------------------

  private void OnCollisionEnter2D(Collision2D other)
  {
    Collision(other.gameObject);
  }

  #endregion

  //=============================================================================

  #region EVENTS
  protected virtual void TargetReach()
  {
    _isFollowingTarget = false;
    rigidbody2D.velocity = Vector2.zero;
    rigidbody2D.angularVelocity = 0f;

    Action e = OnTargetReach;
    if(e != null)
      e();
  }

  //----------------------------------------------
  protected virtual void Collision(GameObject other)
  {
    Action<GameObject> e = OnRouteInterrupt;
    if(e != null)
      e(other);
  }
  #endregion


}
