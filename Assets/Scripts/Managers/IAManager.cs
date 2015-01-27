using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using SimpleJSON;

public class IAManager : Singleton<IAManager> 
{
    public AnimalBehaviourPool AnimalPool;
    public GameObject[] prefabs;

    void Awake() 
    {
        AnimalPool = new AnimalBehaviourPool(prefabs, 200);
    }

    public void InizializeRamdom(int numberAnimals)
    {
        AnimalPool.all.Clear();
        AnimalPool = new AnimalBehaviourPool(prefabs, numberAnimals);
    }

    internal void Inizialize(List<JSONNode> animals)
    {
        if (AnimalPool != null)
            AnimalPool.all.Clear();
        else
            AnimalPool = new AnimalBehaviourPool(prefabs, 200);

        foreach (var animal in animals) 
        {
            var prefab = prefabs.ToList().FirstOrDefault(p => p.name == animal["Type"].ToString().Replace("\"", ""));
            if(prefab != null)
            {
                var f = Instantiate(prefab, GameManager.GetInWorld(animal["x"].AsFloat, animal["y"].AsFloat), Quaternion.identity) as GameObject;
                AddAnimal(f.GetComponent<AnimalBehaviour>());
            }
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
    public AnimalBehaviour GetNearestToMe(AnimalBehaviour me)
    {
        var candidates = AnimalPool.all.FindAll(x => x.FoodChainLevel > me.FoodChainLevel).OrderBy(x => Vector3.Distance(me.transform.position, x.transform.position));

        //Debug.Log(me.name + " -> " + string.Join(", ", candidates.ToList().ConvertAll(x=>x.name).ToArray()));

        if (candidates.Count() > 0) return me.AnimalToHunt = candidates.First().GetComponent<AnimalBehaviour>();

        return null;
    }

    //--------------------

    public List<AnimalBehaviour> GetAllHuntable(AnimalBehaviour me)
    {
        return AnimalPool.all
          .FindAll(x => x.FoodChainLevel > me.FoodChainLevel)
          .OrderBy(x => Vector3.Distance(me.transform.position, x.transform.position))
          .ToList().ConvertAll(x => x.GetComponent<AnimalBehaviour>());
    }
}
