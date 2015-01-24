using UnityEngine;

[ExecuteInEditMode(), RequireComponent(typeof(CircleCollider2D))]
public class RoutePoint : MonoBehaviour 
{
  private const float TriggerRadius = 0.01f;
  public RouteDrawer ParentDrawer { get; set; }



  //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
  //////////////////////////////////////////////////////



  #region MONO
  private void Awake()
  {
    collider2D.isTrigger = true;
    (collider2D as CircleCollider2D).radius = TriggerRadius;
  }

  //----------------------------------------------------------

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if(ParentDrawer != null && collider.gameObject == ParentDrawer.gameObject)
      ParentDrawer.EraseRoutePoint(this);
  }
  #endregion
}
