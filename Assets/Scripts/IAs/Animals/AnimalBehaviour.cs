using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class AnimalBehaviour : StateMachineBehaviour
{
    public int FoodChainLevel;

    public AnimalBehaviour me;

    public Vector3 Target = Vector3.zero;

    public enum States
    {
        Hunt,
        Pasture
    }

    public void Awake()
    {
        me = this;
    }

    void Hunt_Enter()
    {
        Debug.Log("Hunt_Enter");

        //1. Look for the nearest animal to hunt
        var animal = IAManager.Instance.GetNearestToMe(me);

        if (animal != null) Target = animal.transform.position;

        if (Target == Vector3.zero) ChangeState(States.Pasture);
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

    void Pasture_Enter()
    {
        Debug.Log("Pasture_Enter");
        //1. No idea what to do here
    }

    void Pasture_Update()
    {
        Debug.Log("Pasture_Update");
        // 1. Select ramdom target near to me
        if (Target == Vector3.zero)
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
        if (Target != Vector3.zero)
        {
 
        }
    }

    void Pasture_Exit()
    {
        Debug.Log("Pasture_Exit");
        //1. No idea what to do here
        Target = Vector3.zero;
    }
}
