﻿using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

[RequireComponent(typeof(Runner))]
public class AnimalBehaviour : StateMachineBehaviour
{
	public int FoodChainLevel;
	
	private Vector3 Target = Vector3.zero;
	public AnimalBehaviour AnimalToHunt;
	
	private Runner runner;
	
	public enum States { Hunt, Pasture, Escape }
	
	#region Inicialization

    void Awake() 
    {
        if (runner == null)
            runner = GetComponent<Runner>();
    }

    void Start()
    {
        Initialize<States>();
        ChangeState(States.Hunt);
    }

    void OnEnable()
    {
        runner.OnRouteInterrupt += OnRunnerRouteInterrupt;
    }

	void OnDisable()
	{
		runner.OnRouteInterrupt -= OnRunnerRouteInterrupt;
	}
	
	#endregion
	
	#region Events
	
	private void OnRunnerRouteInterrupt(GameObject other)
	{
        string collisionName = LayerMask.LayerToName(other.layer);
        //Debug.Log(name + " -> " + other.name + " (" + collisionName + ")");
        //Esto es un ñordo pero bueno
        switch (collisionName)
        {
            case "Fence":
                switch ((States)GetState())
                {
                    case States.Hunt:
                        HuntOnRunnerCollision(other);
                        break;
                    case States.Pasture:
                        PastureOnRunnerCollision(other);
                        break;
                    case States.Escape:
                        EscapeOnRunnerCollision(other);
                        break;
                }
                break;
            case "Animal":
                var av = other.GetComponent<AnimalBehaviour>();
                if (av != null)
                {
                    if (av.FoodChainLevel < FoodChainLevel) IAManager.Instance.Kill(this);
                    else if (av.FoodChainLevel == FoodChainLevel)
                    {
                        Target = av.gameObject.transform.position;
                        ChangeState(States.Escape);
                    }
                    else ChangeState(States.Hunt);
                }
                break;
        }
	}
	
	#endregion
	
	#region Hunt State
	
	void Hunt_Enter()
	{
		//Debug.Log("Hunt_Enter");
		
		//1. Look for the nearest animal to hunt
        IAManager.Instance.GetNearestToMe(this);
		
        if (AnimalToHunt != null) Target = AnimalToHunt.transform.position;
		
        if (Target == Vector3.zero) ChangeState(States.Pasture);
	}
	
	void Hunt_Update()
	{
        //Debug.Log("Hunt_Update");

        //1. Look at the target and walk 
        if (AnimalToHunt != null)
        {
            Target = AnimalToHunt.transform.position;
            runner.Target = Target;
        }

        /*if (Target == Vector3.zero)*/
        ChangeState(States.Pasture);
	}
	
	void Hunt_Exit()
	{
		//Debug.Log("Hunt_Exit");
		
		//1. Free params
		Target = Vector3.zero;
	}
	
	void HuntOnRunnerCollision(GameObject collision)
	{
		ChangeState(States.Hunt);
	}
	
	#endregion
	
	#region Pasture State
	
	void Pasture_Enter()
	{
		//Debug.Log("Pasture_Enter");
		Target = transform.position.GetRamdomAtDistance((float)Random.Range(5, 30));
		//1. No idea what to do here
	}
	
	void Pasture_Update()
	{
		//Debug.Log("Pasture_Update");
		// 1. Select ramdom target near to me
		if (Target == Vector3.zero)
		{
			// 1.Find a near point to me
			Target = transform.position.GetRamdomAtDistance((float)Random.Range(5, 30));
		}
		
		// 2. Move to the target
		runner.Target = Target;
		
	}
	
	void Pasture_Exit()
	{
		//Debug.Log("Pasture_Exit");
		//1. No idea what to do here
		Target = Vector2.zero;
	}
	
	void PastureOnRunnerCollision(GameObject collision) 
	{
		ChangeState(States.Hunt);
	}
	
	#endregion
	
	#region Escape State
	
	void Escape_Enter()
	{
		//Debug.Log("Escape_Enter");
		//1. Pillo un punto al que huir
        Target = transform.position.GetRamdomAtDistance((float)Random.Range(5, 30));
		
	}
	
	void Escape_Update()
	{
		//Debug.Log("Escape_Update");
		// 1. Select ramdom target near to me
		if (Target == Vector3.zero)
		{
			// 1.Find a near point to me
			Target = transform.position.GetRamdomAtDistance((float)Random.Range(5, 30));
		}
		else
		{
			// 2. Move to the target
			runner.Target = Target;
			// 3. Reach it?
		}
	}
	
	
	void Escape_Exit()
	{
		//Debug.Log("Escape_Exit");
		//1. No idea what to do here
		Target = Vector2.zero;
		ChangeState(States.Hunt);
	}
	
	void EscapeOnRunnerCollision(GameObject collision)
	{
		ChangeState(States.Hunt);
	}
	
	#endregion
}