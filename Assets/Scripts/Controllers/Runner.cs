using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Runner : MonoBehaviour
{
	[SerializeField] 
  Vector2 forwardDirection;
  [SerializeField]
  private float _angularVelocityAbs = 3;
  [SerializeField]
  private float _linearVelocityAbs = 3;

	public event CollisionHandler OnCollision;
  public event Action OnTargetReach;

  //------------------------------------------------------

  private Vector3 _target;
	public Vector3 Target { 
    get { return _target; }
    set {
      if(value != _target) {
        _isFollowingTarget = true;
        _target = value;
        NextRoutePoint.transform.position = _target;
      }
    }
  }
  private bool _isFollowingTarget;

  //------------------------------------------------

  private const float RoutePointRadius = 0.01f;
  private CircleCollider2D _nextRoutePoint;
  private CircleCollider2D NextRoutePoint {
    get {
      if(_nextRoutePoint == null) {
        _nextRoutePoint = new GameObject(name + "RoutePoint").AddComponent<CircleCollider2D>();
        _nextRoutePoint.radius = RoutePointRadius;
        _nextRoutePoint.isTrigger = true;
      }
      return _nextRoutePoint;
    }
  }






  //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
  /////////////////////////////////////////////////////////////




  #region MONO

  public void Awake()
  {
    rigidbody2D.isKinematic = false;
    rigidbody2D.gravityScale = 0f;
  }

  //-------------------------------------

  public void OnEnable()
	{
	  // TODO
	}

  //----------------------------------------------

	void FixedUpdate()
	{
    if(!_isFollowingTarget)
      return;

		Vector3 direction = Target - transform.position;
		float reorientation = Vector3.Cross(forwardDirection, direction).z;

    rigidbody2D.velocity = Vector3.Normalize(direction) * _linearVelocityAbs;
		rigidbody2D.angularVelocity = _angularVelocityAbs * reorientation;
	}

  //---------------------------------------

  private void OnTriggerEnter2D(Collider2D other) 
  {
    if(other == NextRoutePoint)
      TargetReach();
  }
  
  //-----------------------------------------

  private void OnCollisionEnter2D(Collision2D other)
  {
    Collision(other.collider.gameObject);
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
    CollisionHandler e = OnCollision;
    if(e != null)
      e(other);
  }
  #endregion


  //public void CollisionWith(RoutePoint routePoint)
  //{
  //  Stop(routePoint.collider2D);
  //}

  ////--------------------------------------------

  //private void Stop (Collider2D collision)
  //{
  //  rigidbody2D.velocity = Vector2.zero;

  //  if (OnCollision != null) 
  //  {
  //    OnCollision(collision);
  //  }
  //}
}
