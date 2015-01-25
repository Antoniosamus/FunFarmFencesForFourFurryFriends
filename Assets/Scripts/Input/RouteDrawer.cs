using System;
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

  [SerializeField]
  private int _stepGapMax = 5;
  private List<GameObject> _steps = new List<GameObject>();
  


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

  //------------------------------------------------

  private void OnTriggerEnter2D(Collider2D other)
  {
    int stepIndex = -1;
    if(_steps.Count > 0) {
      int stepIndexMax = Math.Min(_steps.Count - 1, _stepGapMax);
      stepIndex = _steps.FindIndex(0, stepIndexMax, go => go == other.gameObject);
    }

    if(stepIndex > -1) 
    {
      List<GameObject> reachedSteps = _steps.GetRange(0, stepIndex + 1);
      foreach(GameObject go in reachedSteps)
        _stepPool.Unspawn(go);
      _steps.RemoveRange(0, stepIndex + 1);
    } 
    else if(_target == other.gameObject && _steps.Count < _stepGapMax) 
    {
      EraseAll();
    }
  }

  

  //--------------------------------------------------

  private void OnCollisionEnter2D()
  {
    EraseAll();
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

    _steps.Add(step);

    _lastPosition = _currentPosition;
    _currentPosition = nextPosition;
  }

  //------------------------------------------------
  protected override void OnRouteStop(Queue<Vector2> route)
  {
    var targetPosition = new Vector3(_lastPosition.x, _lastPosition.y, _targetPrefab.transform.position.z);
    _target = Instantiate(_targetPrefab, targetPosition, Quaternion.identity) as GameObject;
  }

  //------------------------------------------------

  protected override void OnRouteCancel()
  {
    EraseAll();
  }

  #endregion


  //====================================

  private void EraseAll()
  {
    _stepPool.UnspawnAll();
    _steps.Clear();
    if (_target != null) {
      Destroy(_target);
      _target = null;
    }
  }

}
