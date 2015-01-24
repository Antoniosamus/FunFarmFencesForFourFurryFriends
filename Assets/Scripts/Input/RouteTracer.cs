using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RouteTracer : MonoBehaviour, IRouteTracer
{
  #region IRouteTracer
  public event Action<Vector3> OnRouteStart;
  public event Action<Queue<Vector3>> OnRouteStop;
  public event Action<Vector3> OnRouteStay;
  public event Action OnRouteCancel;

  public abstract Queue<Vector3> LastRoute { get; }

  #endregion


  //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
  /////////////////////////////////////////////////////////////



  #region EVENTS

  protected virtual void RouteStart(Vector3 startPostion)
  {
    Action<Vector3> e = OnRouteStart;
    if (e != null)
      e(startPostion);
  }

  //------------------------------------------------------------------------

  protected virtual void RouteStay(Vector3 currentPostion)
  {
    Action<Vector3> e = OnRouteStay;
    if (e != null)
      e(currentPostion);
  }

  //------------------------------------------------------------------------

  protected virtual void RouteStop(Queue<Vector3> route)
  {
    Action<Queue<Vector3>> e = OnRouteStop;
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

