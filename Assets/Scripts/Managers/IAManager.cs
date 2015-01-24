using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class IAManager : Singleton<IAManager> 
{
    private AnimalBehaviourPool AnimalPool;

    public void Inizialize(int numbreAnimals)
    {
        AnimalPool = new AnimalBehaviourPool(numbreAnimals);
        AnimalPool.PrePopulate(numbreAnimals);
    }

    public void AddAnimal(AnimalBehaviour animal)
    {
        AnimalPool.Add(animal, animal.transform.position, animal.transform.rotation);
    }

    public void AddAnimal(AnimalBehaviour animal, Vector3 position, Quaternion rotation)
    {
        AnimalPool.Add(animal, position, rotation);
    }

    public void UnSpawn(AnimalBehaviour animal)
    {
        AnimalPool.Unspawn(animal);
    }

    /// <summary>
    /// Return null si no hay a quien perseguir bitch!
    /// </summary>
    /// <param name="me"></param>
    /// <returns>AnimalBehaviour or Null</returns>
    public AnimalBehaviour GetNearestToMe(AnimalBehaviour me) 
    {
        return AnimalPool.all.FindAll(x => x.FoodChainLevel < me.FoodChainLevel).OrderBy(x => x.FoodChainLevel).FirstOrDefault();
    }
}
