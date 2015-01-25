using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class IAManager : Singleton<IAManager> 
{
    public AnimalBehaviourPool AnimalPool;
    public GameObject[] prefabs;

    public void Inizialize(int numberAnimals)
    {
        AnimalPool = new AnimalBehaviourPool(prefabs, numberAnimals);
    }

    public void Stop()
    {
        foreach(var a in AnimalPool.all)
        {
            a.ChangeState(AnimalBehaviour.States.Stop);
        }
    }

    public void AddAnimal(AnimalBehaviour animal)
    {
        AnimalPool.Add(animal, animal.transform.position, animal.transform.rotation);
    }

    public void AddAnimal(AnimalBehaviour animal, Vector3 position, Quaternion rotation)
    {
        AnimalPool.Add(animal, position, rotation);
    }

    public void Kill(AnimalBehaviour animal)
    {
        AnimalPool.Unspawn(animal);
    }

    /// <summary>
    /// Return null si no hay a quien perseguir bitch!
    /// </summary>
    /// <param name="me"></param>
    /// <returns>AnimalBehaviour or Null</returns>
    public void GetNearestToMe(AnimalBehaviour me)
    {
        var candidates = AnimalPool.all.FindAll(x => x.FoodChainLevel > me.FoodChainLevel).OrderBy(x => x.FoodChainLevel);
        me.AnimalToHunt = null;

        if (candidates.Count() > 0) me.AnimalToHunt = candidates.First();

        //foreach (var anim in candidates)
        //{
        //    RaycastHit hit;
        //    if (!Physics.Linecast(me.transform.position, anim.transform.position, out hit))
        //    {
        //        // Draw line between m and the hit point
        //        me.AnimalToHunt = anim;
        //        break;
        //    }
        //}
    }
}
