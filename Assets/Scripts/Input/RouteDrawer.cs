using System.Collections.Generic;
using UnityEngine;


public class RouteDrawer : RouteFollower
{
  private const int StepCountMax = 200;
  
  [SerializeField] 
  private string _stepPath = "Route/RouteStep";
  private GameObject _stepPrefab;
  private GameObjectPool2D _stepSpritePool;

  [SerializeField] 
  private string _targetPath = "Route/RouteTarget";
  private GameObject _targetPrefab;
  
  private Vector2 _lastPosition;
  private Vector2 _currentPosition;

  private Queue<GameObject> _stepSprites = new Queue<GameObject>();


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

    _stepSpritePool = new GameObjectPool2D(_stepPrefab, StepCountMax);
  }

  //---------------------------------------------------------------

  protected override void OnDestoy()
  {
    _stepSprites = null;
    _stepPrefab = null;
    _targetPrefab = null;
    _stepSpritePool = null;
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
    _stepSpritePool.Spawn(_currentPosition, 
      Quaternion.FromToRotation(_stepPrefab.transform.right, nextPosition - _lastPosition));

    _lastPosition = _currentPosition;
    _currentPosition = nextPosition;
  }

  //------------------------------------------------
  protected override void OnRouteStop(Queue<Vector2> route)
  {
    //_stepSpritePool.Spawn(_currentPosition, 
    //  Quaternion.FromToRotation(_stepPrefab.transform.up, targetPosition - _lastPosition));
    
    Vector3 targetPosition = new Vector3(_lastPosition.x, _lastPosition.y, _targetPrefab.transform.position.z);
    Instantiate(_targetPrefab, targetPosition, Quaternion.identity);
  }

  //------------------------------------------------

  protected override void OnRouteCancel()
  {
    _stepSpritePool.UnspawnAll();
    _stepSprites.Clear();
  }
  #endregion


  //==============================================================



}
