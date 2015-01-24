using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(FarmerController) )]
public class RouteDrawer : RouteFollower
{
  private const int StepCountMax = 200;
  
  [SerializeField] 
  private string _stepPath = "Route/RouteStep";
  private GameObject _stepPrefab;
  private GameObjectPool2D _stepPool;

  [SerializeField] 
  private string _targetPath = "Route/RouteTarget";
  private GameObject _targetPrefab;
  private GameObject _target;

  [SerializeField]
  private FarmerController _farmerController;
  
  private Vector2 _lastPosition;
  private Vector2 _currentPosition;

  private Queue<GameObject> _steps = new Queue<GameObject>();
  


  //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
  ///////////////////////////////////////////////////////////////////


  #region MONO

  protected override void Awake()
  {
    base.Awake();
    _stepPrefab = Resources.Load<GameObject>(_stepPath);
    if(_stepPrefab == null) 
      Debug.LogError("<b>RouteDrawer::Awake>> </b> StepPrefab not found!!", gameObject);

    _targetPrefab = Resources.Load<GameObject>(_targetPath);
    if(_stepPrefab == null) 
      Debug.LogError("<b>RouteDrawer::Awake>> </b> TargetPrefab not found!!", gameObject);

    if(_farmerController == null)
      _farmerController = GetComponent<FarmerController>();

    _stepPool = new GameObjectPool2D(_stepPrefab, StepCountMax);
  }

  //---------------------------------------------------------------

  protected override void OnDestoy()
  {
    _steps = null;
    _stepPool = null;
    _stepPrefab = null;
    
    _targetPrefab = null;
    _target = null;

    _farmerController = null;
    
    base.OnDestoy();
  }
  #endregion


  //==============================================================

  #region RouteFollower
  protected override void OnRouteStart(Vector2 startPosition)
  {
    _lastPosition = transform.position;
    _currentPosition = startPosition;
  }

  //------------------------------------------------

  protected override void OnRouteStay(Vector2 nextPosition)
  {
    GameObject step = _stepPool.Spawn(_currentPosition, 
      Quaternion.FromToRotation(_stepPrefab.transform.right, nextPosition - _lastPosition));

    step.GetComponent<RoutePoint>().ParentDrawer = this;
    _steps.Enqueue(step);

    _lastPosition = _currentPosition;
    _currentPosition = nextPosition;
  }

  //------------------------------------------------
  protected override void OnRouteStop(Queue<Vector2> route)
  {
    var targetPosition = new Vector3(_lastPosition.x, _lastPosition.y, _targetPrefab.transform.position.z);
    _target = Instantiate(_targetPrefab, targetPosition, Quaternion.identity) as GameObject;
    _target.GetComponent<RoutePoint>().ParentDrawer = this;
  }

  //------------------------------------------------

  protected override void OnRouteCancel()
  {
    _stepPool.UnspawnAll();
    _steps.Clear();

    if(_target != null) {
      Destroy(_target);
      _target = null;
    }
  }

  #endregion


  //==============================================================
  
  public void EraseRoutePoint(RoutePoint routePoint)
  {
    if(routePoint != _target) {
      _stepPool.Unspawn(routePoint.gameObject);
    
    } else {
      Destroy(_target.gameObject);
      _target = null;
    }
  }
}
