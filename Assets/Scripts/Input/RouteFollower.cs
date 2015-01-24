using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IRouteTracer))]
public abstract class RouteFollower : MonoBehaviour 
{
  [SerializeField]
  private IRouteTracer _routeTracer;


  //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
  /////////////////////////////////////////////////////////

  
  #region MONO
  protected virtual void Awake()
  {
    if(_routeTracer == null)
      _routeTracer = GetComponent( typeof(IRouteTracer) ) as IRouteTracer;

    if(_routeTracer == null) 
      Debug.LogError("<b>RouteFollower::Awake>> </b> RouteTracer not found!!", gameObject);
  }

  //--------------------------------------------------------------

  protected virtual void OnDestoy()
  {
    _routeTracer = null;
  }

  //------------------------------------------------------------

  protected virtual void OnEnable()
  {
    if(_routeTracer != null) {
      _routeTracer.OnRouteStart  += OnRouteStart;
      _routeTracer.OnRouteStay   += OnRouteStay;
      _routeTracer.OnRouteStop   += OnRouteStop;
      _routeTracer.OnRouteCancel += OnRouteCancel;
    }
  }

  //---------------------------------------------------------

  protected virtual void OnDisable()
  {
    if(_routeTracer != null) {
      _routeTracer.OnRouteStart  -= OnRouteStart;
      _routeTracer.OnRouteStay   -= OnRouteStay;
      _routeTracer.OnRouteStop   -= OnRouteStop;
      _routeTracer.OnRouteCancel -= OnRouteCancel;
    }
  }

  #endregion



  //================================================================

  #region EVENT HANDLERS
  protected abstract void OnRouteStart(Vector2 startPosition);
  protected abstract void OnRouteStop(Queue<Vector2> obj);
  protected abstract void OnRouteStay(Vector2 obj);
  protected abstract void OnRouteCancel();
  #endregion
}

