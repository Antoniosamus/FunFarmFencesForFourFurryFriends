using System;
using System.Collections.Generic;
using UnityEngine;

public interface IRouteTracer
{
  event Action<Vector2> OnRouteStart;         // Comienzo de trazado -> envía posición inicial
  event Action<Queue<Vector2>> OnRouteStop;   // Fin de trazado     -> envía la ruta trazada
  event Action<Vector2> OnRouteStay;          // Durante el trazado -> envían posiciones actuales
  event Action OnRouteCancel;                 // Cancelación de la ruta

  Queue<Vector2> LastRoute { get; } // Ruta trazada
}


