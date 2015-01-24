using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

[RequireComponent(typeof(Runner))]
public class AnimalBehaviour : StateMachineBehaviour
{
    public int FoodChainLevel;

    public AnimalBehaviour me;

    public Vector2 Target = Vector3.zero;
    private Runner runner;

    public enum States { Hunt, Pasture }

    #region Inicialization
    public void Awake()
    {
        me = this;

        if (runner == null)
            runner = GetComponent<Runner>();

        Initialize<States>();
    }

    public void OnEnable()
    {
        //tracer.OnRouteStart += HandleOnRouteStart;
        runner.OnCollisionAppears += OnRunnerCollision;
    }

    public void OnDisable()
    {
        //tracer.OnRouteStart -= HandleOnRouteStart;
        runner.OnCollisionAppears -= OnRunnerCollision;
    }

    #endregion

    #region Events

    private void OnRunnerCollision(Collision2D collision)
    {
        string collisionName = LayerMask.LayerToName(collision.collider.gameObject.layer);

        //Esto es un ñordo pero bueno
        switch (collisionName)
        {
            case "Fence":
                switch ((States)GetState()) 
                {
                    case States.Hunt:
                        Hunt_OnRunnerCollision(collision);
                        break;
                    case States.Pasture:
                        Pasture_OnRunnerCollision(collision);
                        break;
                }
            break;
            case "Animal":
                switch ((States)GetState())
                {
                    case States.Hunt:
                        Hunt_OnRunnerCollision(collision);
                        break;
                    case States.Pasture:
                        Pasture_OnRunnerCollision(collision);
                        break;
                }
            break;
        }
    }

    #endregion

    #region Hunt State

    void Hunt_Enter()
    {
        Debug.Log("Hunt_Enter");

        //1. Look for the nearest animal to hunt
        var animal = IAManager.Instance.GetNearestToMe(me);

        if (animal != null) Target = animal.transform.position;

        if (Target == Vector2.zero) ChangeState(States.Pasture);
    }

    void Hunt_Update()
    {
        Debug.Log("Hunt_Update");

        //1. Look at the target and walk 
        MoveToTarget();
    }

    void Hunt_Exit()
    {
        Debug.Log("Hunt_Exit");

        //1. Free params
        Target = Vector3.zero;
    }

    void Hunt_OnRunnerCollision(Collision2D collision)
    {

    }

    #endregion

    #region Pasture State

    void Pasture_Enter()
    {
        Debug.Log("Pasture_Enter");
        //1. No idea what to do here
    }

    void Pasture_Update()
    {
        Debug.Log("Pasture_Update");
        // 1. Select ramdom target near to me
        if (Target == Vector2.zero)
        {
            // 1.Find a near point to me
            Target = transform.position.GetRamdomAtDistance((float)Random.Range(5, 30));
        }
        else 
        {
            // 2. Move to the target
            MoveToTarget();
            // 3. Reach it?
        }
    }

    private void MoveToTarget()
    {
        if (Target != Vector2.zero)
        {
 
        }
    }

    void Pasture_Exit()
    {
        Debug.Log("Pasture_Exit");
        //1. No idea what to do here
        Target = Vector2.zero;
    }

    void Pasture_OnRunnerCollision(Collision2D collision) 
    {

    }

    #endregion
}
