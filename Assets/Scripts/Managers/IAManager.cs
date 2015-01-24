using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class IAManager : Singleton<IAManager> 
{
    public GameObjectPool AnimalPool;

    public void Inizialize(int animals)
    {
        AnimalPool.PrePopulate(animals);
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
