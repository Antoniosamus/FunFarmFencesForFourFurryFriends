using System;
using System.Collections.Generic;
using UnityEngine;

public interface IRouteTracer
{
  event Action<Vector3> OnRouteStart;
  event Action<Queue<Vector3>> OnRouteStop;
  event Action<Vector3> OnRouteStay;
  event Action OnRouteCancel;

  Queue<Vector3> LastRoute { get; }
}


