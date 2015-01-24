using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RouteTracer : MonoBehaviour, IRouteTracer
{
  #region IRouteTracer
  public event Action<Vector2> OnRouteStart;
  public event Action<Queue<Vector2>> OnRouteStop;
  public event Action<Vector2> OnRouteStay;
  public event Action OnRouteCancel;

  public abstract Queue<Vector2> LastRoute { get; }
  public abstract bool IsRouting { get; }
  #endregion


  //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
  /////////////////////////////////////////////////////////////



  #region EVENTS

  protected virtual void RouteStart(Vector2 startPosition)
  {
    Action<Vector2> e = OnRouteStart;
    if (e != null)
      e(startPosition);
  }

  //------------------------------------------------------------------------

  protected virtual void RouteStay(Vector2 currentPosition)
  {
    Action<Vector2> e = OnRouteStay;
    if (e != null)
      e(currentPosition);
  }

  //------------------------------------------------------------------------

  protected virtual void RouteStop(Queue<Vector2> route)
  {
    Action<Queue<Vector2>> e = OnRouteStop;
    if (e != null)
      e(route);
  }

  //------------------------------------------------------------------------

  protected virtual void RouteCancel()
  {
    Action e = OnRouteCancel;
    if (e != null)
      e();

  }


  #endregion





  
}

