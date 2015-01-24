using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class AnimalBehaviour : StateMachineBehaviour
{
    public int FoodChainLevel;

    public Vector3 Target;

    public enum States
    {
        Hunt,
        Run
    }

    void Hunt_Enter()
    {
        Debug.Log("Hunt_Enter");

        //1. Look for the nearest animal to hunt
    }

    void Hunt_Update()
    {
        Debug.Log("Hunt_Update");

        //1. Look at the target and walk 
    }

    void Hunt_Exit()
    {
        Debug.Log("Hunt_Exit");

        //1. Free params
    }

    void Run_Enter()
    {
        Debug.Log("Run_Enter");
        //1. No idea what do here
    }

    void Run_Update()
    {
        Debug.Log("Run_Update");
        // 1. Select ramdom target near to me
        // 2. Move to the target
        //Reach?
    }

    void Run_Exit()
    {
        Debug.Log("Run_Exit");
        //1. No idea what do here
    }
}
