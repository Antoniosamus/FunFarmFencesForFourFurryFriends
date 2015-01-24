using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class DragRouteTracer : MonoBehaviour, IRouteTracer
{
  private Vector3 dist;
  private float posX;
  private float posY;


  //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
  //////////////////////////////////////////////////////////////////


  #region Mouse Events

  void OnMouseDown()
  {
    dist = Camera.main.WorldToScreenPoint(transform.position);
    posX = Input.mousePosition.x - dist.x;
    posY = Input.mousePosition.y - dist.y;
  }

  //------------------------------------------------------------------------

  void OnMouseDrag()
  {
    Vector3 curPos =
              new Vector3(Input.mousePosition.x - posX,
              Input.mousePosition.y - posY, dist.z);

    Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
    transform.position = worldPos;
  }

  #endregion

  //======================================================================


  #region IRouteTracer
  public event Action<Vector3> OnRouteStart;
  public event Action<Queue<Vector3>> OnRouteStop;
  public event Action<Vector3> OnRouteStay;
  public event Action OnRouteCancel;

  public Queue<Vector3> LastRoute
  {
    get { throw new System.NotImplementedException(); }
  }

  #endregion

 

}