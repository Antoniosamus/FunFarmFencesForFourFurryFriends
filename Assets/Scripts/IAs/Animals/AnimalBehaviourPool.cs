using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// A general pool object for reusable game objects.
//
// It supports spawning and unspawning game objects that are
// instantiated from a common prefab. Can be used preallocate
// objects to avoid calls to Instantiate during gameplay. Can
// also create objects on demand (which it does if no objects
// are available in the pool).

public class AnimalBehaviourPool
{
    // The prefab that the game objects will be instantiated from.
    private GameObject[] prefabs;

    // The list of all game objects created thus far (used for efficiently
    // unspawning all of them at once, see UnspawnAll).
    public List<AnimalBehaviour> all;

    // An optional function that will be called whenever a new object is instantiated.
    // The newly instantiated object is passed to it, which allows users of the pool
    // to do custom initialization.
    //private var initializationFunction : Function;

    // Creates a pool.
    // The initialCapacity is used to initialize the .NET collections, and determines
    // how much space they pre-allocate behind the scenes. It does not pre-populate the
    // collection with game objects. For that, see the PrePopulate function.
    // If an initialCapacity that is <= to zero is provided, the pool uses the default
    // initial capacities of its internal .NET collections.
    //function GameObjectPool(prefab : GameObject, initialCapacity : int, initializationFunction : Function, setActiveRecursively : boolean){
    public AnimalBehaviourPool(GameObject[] _prefabs, int initialCapacity)
    {
        prefabs = _prefabs;

        if (initialCapacity > 0)
            this.all = new List<AnimalBehaviour>(initialCapacity);
        else
            this.all = new List<AnimalBehaviour>();

        PrePopulate(initialCapacity);
    }

    public GameObject GetRamdomPrefab() 
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

    public void Add(AnimalBehaviour animal, Vector3 position, Quaternion rotation) 
    {
        GameObject result;

        if (all.Count == 0)
        {
            if (animal != null) all.Add(animal);
            else Debug.LogError("Prefab " + animal.gameObject.name + " no tiene componente AnimalBehaviour");
        }
        else
        {
            result = all.First().gameObject;

            foreach (ParticleSystem ps in result.GetComponentsInChildren<ParticleSystem>())
                ps.Clear(true);
            foreach (TrailRenderer trail in result.GetComponentsInChildren<TrailRenderer>())
                trail.time = 0.0f;

            // Get the result's transform and reuse for efficiency.
            // Calling gameObject.transform is expensive.
            var resultTrans = result.transform;
            resultTrans.position = position;
            resultTrans.rotation = rotation;

            this.SetActive(result, true);
        }

        Sort();
    }

    // Spawn a game object with the specified position/rotation.
    public GameObject RandomSpawn(Vector3 position, Quaternion rotation)
    {
        // Create an object and initialize it.
         GameObject result = GameObject.Instantiate(GetRamdomPrefab(), position, rotation) as GameObject;
        var av = result.GetComponent<AnimalBehaviour>();
        //this.SetActive(result, true);

        if(all.Count > 0)
        {
            result = all.First().gameObject;

            foreach (ParticleSystem ps in result.GetComponentsInChildren<ParticleSystem>())
                ps.Clear(true);
            foreach (TrailRenderer trail in result.GetComponentsInChildren<TrailRenderer>())
                trail.time = 0.0f;

            // Get the result's transform and reuse for efficiency.
            // Calling gameObject.transform is expensive.
            var resultTrans = result.transform;
            resultTrans.position = position;
            resultTrans.rotation = rotation;
        }



        if (av != null) all.Add(av);
        else Debug.LogError("Prefab " + result.name + " no tiene componente AnimalBehaviour");

        return result;
    }

    // Unspawn the provided game object.
    // The function is idempotent. Calling it more than once for the same game object is
    // safe, since it first checks to see if the provided object is already unspawned.
    // Returns true if the unspawn succeeded, false if the object was already unspawned.
    public bool Unspawn(AnimalBehaviour obj)
    {
        if (!all.Contains(obj))
        { // Make sure we don't insert it twice.
            all.Add(obj);
            this.SetActive(obj.gameObject, false);
            return true; // Object inserted back in stack.
        }
        return false; // Object already in stack.
    }

    // Pre-populates the pool with the provided number of game objects.
    public void PrePopulate(int count)
    {
        GameObject[] array = new GameObject[count];
        for (var i = 0; i < count; i++)
        {
            array[i] = RandomSpawn(Vector3.zero, Quaternion.identity);
            this.SetActive(array[i], true);
        }

        Sort();
    }

    // Unspawns all the game objects created by the pool.
    public void UnspawnAll()
    {
        for (var i = 0; i < all.Count; i++)
        {
            AnimalBehaviour obj = all[i];
            if (obj.gameObject.activeInHierarchy)
                Unspawn(obj);
        }
    }

    // Unspawns all the game objects and clears the pool.
    public void Clear()
    {
        UnspawnAll();
        all.Clear();
    }

    // Returns the prefab being used by this pool.
    public GameObject[] GetPrefab()
    {
        return prefabs;
    }

    // Activates or deactivates the provided game object using the method
    // specified by the setActiveRecursively flag.
    private void SetActive(GameObject obj, bool val)
    {
        obj.SetActive(val);
    }

    private void Sort()
    {
        all.OrderBy(x => x.FoodChainLevel);
    }

    public void ForceSort()
    {
        Sort();
    }
}