﻿using System.Collections.Generic;
﻿using System.Linq;
﻿using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

[RequireComponent(typeof(Runner))]
public class AnimalBehaviour : StateMachineBehaviour
{
	public int FoodChainLevel;
	
	//private Vector3 Target = Vector3.zero;
	public AnimalBehaviour AnimalToHunt = null;
	
	private Runner runner;
  private List<AnimalBehaviour> _animalsNotToHunt = new List<AnimalBehaviour>();

  public enum States { Hunt, Pasture, Escape, Stop }
	
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
        runner.OnTargetReach += OnRouteStepReach;
    }

	void OnDisable()
	{
        runner.OnTargetReach -= OnRouteStepReach;
	}
	
	#endregion
	
	#region Events
	
	private void OnCollisionStay2D(Collision2D other)
	{
        string collisionName = LayerMask.LayerToName(other.gameObject.layer);
        //Debug.Log(name + " -> " + other.gameObject.name + " (" + collisionName + ")");
        //Esto es un ñordo pero bueno
        switch (collisionName)
        {
            case "Farmer":
                Destroy(other.gameObject);
                ChangeState(States.Hunt);
                break;
            case "Obstacle":
                switch ((States)GetState())
                {
                    case States.Hunt:
                        HuntOnRunnerCollision(other.gameObject);
                        break;
                    case States.Pasture:
                        PastureOnRunnerCollision(other.gameObject);
                        break;
                    case States.Escape:
                        EscapeOnRunnerCollision(other.gameObject);
                        break;
                }
                break;
            case "Animal":
                var av = other.gameObject.GetComponent<AnimalBehaviour>();
                if (av != null)
                {
                    if (av.FoodChainLevel < FoodChainLevel) IAManager.Instance.Kill(this);
                    else if (av.FoodChainLevel == FoodChainLevel)
                    {
                        //runner.Target = av.gameObject.transform.position;
                        runner.Target = transform.position.GetRamdomAtDistance((float)Random.Range(1, 5));
                        ChangeState(States.Escape);
                    }
                    else 
                    { 
                        //ChangeState(States.Hunt); 
                        IAManager.Instance.GetNearestToMe(this);

                        if (AnimalToHunt != null) runner.Target = AnimalToHunt.transform.position;
                        else ChangeState(States.Pasture);
                    }
                }
                break;
        }
	}

    private void OnRouteStepReach()
    {
        IAManager.Instance.GetNearestToMe(this);
        //switch ((States)GetState())
        //{
        //    case States.Hunt:
        //        IAManager.Instance.GetNearestToMe(this);
        //        break;
        //    case States.Pasture:
        //        runner.Target = transform.position.GetRamdomAtDistance((float)Random.Range(5, 30));
        //        break;
        //    case States.Escape:
        //        runner.Target = transform.position.GetRamdomAtDistance((float)Random.Range(5, 30));
        //        break;
        //}
    }

	#endregion
	
	#region Hunt State
	
	void Hunt_Enter()
	{
		//Debug.Log("Hunt_Enter");
		
		//1. Look for the nearest animal to hunt
        IAManager.Instance.GetNearestToMe(this);

        if (AnimalToHunt != null) runner.Target = AnimalToHunt.transform.position;
        else ChangeState(States.Pasture);
	}
	
	void Hunt_Update()
	{
        //Debug.Log("Hunt_Update");

        //1. Look at the target and walk 
        if (AnimalToHunt != null)
            runner.Target = AnimalToHunt.transform.position;
        else
            AnimalToHunt = IAManager.Instance.GetNearestToMe(this);

        if (AnimalToHunt == null) 
          ChangeState(States.Pasture);
	}
	
	void Hunt_Exit()
	{
		//Debug.Log("Hunt_Exit");
	}
	
	void HuntOnRunnerCollision(GameObject collision)
	{
    if(collision.layer == LayerMask.NameToLayer("Obstacle"))
    {
      if(!_animalsNotToHunt.Contains(AnimalToHunt))
        _animalsNotToHunt.Add(AnimalToHunt);

      AnimalToHunt = IAManager.Instance.GetAllHuntable(this).Except(_animalsNotToHunt).FirstOrDefault();
      if(AnimalToHunt == null)
        ChangeState(States.Pasture);
    } 
    else 
    {
		  ChangeState(States.Hunt);
    }
	}
	
	#endregion
	
	#region Pasture State
	
	void Pasture_Enter()
	{
		//Debug.Log("Pasture_Enter");
        runner.Target = transform.position.GetRamdomAtDistance((float)Random.Range(1, 5));
        AnimalToHunt = null;
        StartCoroutine(CheckHunt());
	}

    IEnumerator CheckHunt() 
    {
        yield return new WaitForSeconds(3);

        if (AnimalToHunt != null) yield break;

        var a = IAManager.Instance.GetAllHuntable(this).Except(_animalsNotToHunt).FirstOrDefault();
        if (a != null) 
          ChangeState(States.Hunt);
        else 
          StartCoroutine(CheckHunt());
    }

	void Pasture_Update()
	{
		//Debug.Log("Pasture_Update");
        //ChangeState(States.Hunt);
	}
	
	void Pasture_Exit()
	{
		//Debug.Log("Pasture_Exit");
        //runner.Target = Vector2.zero;
	}
	
	void PastureOnRunnerCollision(GameObject collision) 
	{
   
	}
	
	#endregion
	
	#region Escape State
	
	void Escape_Enter()
	{
		//Debug.Log("Escape_Enter");
        runner.Target = transform.position.GetRamdomAtDistance((float)Random.Range(10, 30));
        AnimalToHunt = null;
		
	}
	
	void Escape_Update()
	{
		//Debug.Log("Escape_Update");
        //ChangeState(States.Hunt);
	}
	
	
	void Escape_Exit()
	{
		//Debug.Log("Escape_Exit");
		//ChangeState(States.Hunt);
	}
	
	void EscapeOnRunnerCollision(GameObject collision)
	{
		ChangeState(States.Hunt);
	}
	
	#endregion

    void Stop_Enter()
    {
        //Debug.Log("Escape_Enter");
        runner.Stop();
    }
}