#if DEBUG
//#define DEBUG_ROUTE
#endif

using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class DragRouteTracer : RouteTracer, IRouteTracer
{
  #region RouteTracer

  private Queue<Vector2> _lastRoute = new Queue<Vector2>();

  public override Queue<Vector2> LastRoute{ get { return _lastRoute; } }

  private  bool _isRouting;
  public override bool IsRouting { get { return _isRouting; } }
  #endregion

  //-------------------------------------------------------------

  private Vector3 _lastScreenPosition;
  private Vector3 _clickOffsset;

  [SerializeField]
  private float _stepDistanceMinSqr = 1f;

  


  //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
  //////////////////////////////////////////////////////////////////


  #region MONO
  private void OnDestroy()
  {
    _lastRoute = null;
  }
  #endregion

  //=============================================================

  #region Mouse Events

  private void OnMouseDown()
  {
    _lastScreenPosition = WorldToScreenPoint(transform.position);
    _clickOffsset = Input.mousePosition - _lastScreenPosition;
  }

  //------------------------------------------------------------------------

  private void OnMouseDrag()
  {
    Vector3 currentScreenPosition = Input.mousePosition - _clickOffsset;
    if ((currentScreenPosition - _lastScreenPosition).sqrMagnitude > _stepDistanceMinSqr)
    {
      if (!_isRouting) 
        RouteStart( ScreenToWorldPoint(_lastScreenPosition) );
    
      RouteStay( ScreenToWorldPoint(currentScreenPosition) );
      _lastScreenPosition = currentScreenPosition;
    }
  }

  //-----------------------------------------------------------------------------

  private void OnMouseUp()
  {
    if(_isRouting)
      RouteStop(_lastRoute);
    else 
      RouteCancel();
  }
  #endregion


  //==================================================================


  #region RouteTracer

  protected override void RouteStart(Vector2 startPosition)
  {
    #if DEBUG_ROUTE
    Debug.Log("<b>DragRouteTracer::RouteStart</b>", gameObject);
    #endif

    _isRouting = true;
    _lastRoute.Clear();
    _lastRoute.Enqueue(startPosition);
    base.RouteStart(startPosition);
  }

  //------------------------------------------

  protected override void RouteStay(Vector2 currentPosition)
  {
    //transform.position = currentPosition;
    #if DEBUG_ROUTE
    Debug.Log("<b>DragRouteTracer::RouteStay>> </b> " + currentPosition, gameObject);
    #endif

    _lastRoute.Enqueue(currentPosition);
    base.RouteStay(currentPosition);


  }
  //------------------------------------------

  protected override void RouteStop(Queue<Vector2> route)
  {
    #if DEBUG_ROUTE
    Debug.Log("<b>DragRouteTracer::RouteStop</b>", gameObject);
    #endif

    _isRouting = false;
    base.RouteStop(route);
  }

  //------------------------------------------

  protected override void RouteCancel()
  {
    #if DEBUG_ROUTE
    Debug.Log("<b>DragRouteTracer::RouteCancel</b>", gameObject);
    #endif
    _isRouting = false;
    _lastRoute.Clear();
    base.RouteCancel();
  }

  #endregion


  //========================================================


  #region AUX
  private Vector3 ScreenToWorldPoint(Vector3 screenPoint)
  {
    return Camera.main.ScreenToWorldPoint(screenPoint);
  }
  //--------------------------------------------------
  private Vector3 WorldToScreenPoint(Vector3 worldPoint)
  {
    return Camera.main.WorldToScreenPoint(worldPoint);
  }
  #endregion


  
}










  