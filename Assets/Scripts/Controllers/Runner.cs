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

  //------------------------------------------------------

  private Vector3 _target;
	public Vector3 Target { 
    get { return _target; }
    set {
      if(value != _target) {
        NextRoutePoint.transform.position = _target;
        _target = value;
      }
    }
  }
  
  //------------------------------------------------

  private RoutePoint _nextRoutePoint;
  private RoutePoint NextRoutePoint {
    get {
      return  _nextRoutePoint != null ? _nextRoutePoint :
        ( _nextRoutePoint = new GameObject().AddComponent<RoutePoint>() );  
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
		Vector3 direction = Target - transform.position;
		float reorientation = Vector3.Cross(forwardDirection, direction).z;

    rigidbody2D.velocity = Vector3.Normalize(direction) * _linearVelocityAbs;
		rigidbody2D.angularVelocity = _angularVelocityAbs * reorientation;
	}

  //---------------------------------------

  private void OnTriggerEnter2D(Collider2D other) 
  {
    Debug.Log(name);
    OnCollision(other.gameObject);

  }
  //-----------------------------------------

  private void OnCollisionEnter2D(Collision2D other)
  {
    OnCollision(other.collider.gameObject);
  }

  #endregion

  //=============================================================================


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
