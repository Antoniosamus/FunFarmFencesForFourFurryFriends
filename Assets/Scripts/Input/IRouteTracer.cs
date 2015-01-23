using System;
using System.Collections.Generic;
using UnityEngine;

public interface IRouteTracer
{
  event Action<Vector3> OnRouteStart;         // Comienzo de trazado -> envía posición inicial
  event Action<Queue<Vector3>> OnRouteStop;   // Fin de trazado     -> envía la ruta trazada
  event Action<Vector3> OnRouteStay;          // Durante el trazado -> envían posiciones actuales
  event Action OnRouteCancel;                 // Cancelación de la ruta

  Queue<Vector3> LastRoute { get; } // Ruta trazada
}


