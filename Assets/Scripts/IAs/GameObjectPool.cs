using UnityEngine;
using System.Collections;

// A general pool object for reusable game objects.
//
// It supports spawning and unspawning game objects that are
// instantiated from a common prefab. Can be used preallocate
// objects to avoid calls to Instantiate during gameplay. Can
// also create objects on demand (which it does if no objects
// are available in the pool).
public class GameObjectPool
{

    // The prefab that the game objects will be instantiated from.
    private GameObject prefab;

    // The list of available game objects (initially empty by default).
    private Stack available;

    // The list of all game objects created thus far (used for efficiently
    // unspawning all of them at once, see UnspawnAll).
    private ArrayList all;

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
    public GameObjectPool(GameObject prefab, int initialCapacity)
    {
        this.prefab = prefab;
        if (initialCapacity > 0)
        {
            this.available = new Stack(initialCapacity);
            this.all = new ArrayList(initialCapacity);
        }
        else
        {
            // Use the .NET defaults
            this.available = new Stack();
            this.all = new ArrayList();
        }
        //this.initializationFunction = initializationFunction;
    }

    // Spawn a game object with the specified position/rotation.
    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject result;

        if (available.Count == 0)
        {
            // Create an object and initialize it.
            result = GameObject.Instantiate(prefab, position, rotation) as GameObject;
            //if(initializationFunction != null){
            //	initializationFunction(result);
            //}
            // Keep track of it.
            all.Add(result);
        }
        else
        {
            result = available.Pop() as GameObject;

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
        return result;
    }

    // Unspawn the provided game object.
    // The function is idempotent. Calling it more than once for the same game object is
    // safe, since it first checks to see if the provided object is already unspawned.
    // Returns true if the unspawn succeeded, false if the object was already unspawned.
    public bool Unspawn(GameObject obj)
    {
        if (!available.Contains(obj))
        { // Make sure we don't insert it twice.
            available.Push(obj);
            this.SetActive(obj, false);
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
            array[i] = Spawn(Vector3.zero, Quaternion.identity);
            this.SetActive(array[i], false);
        }
        for (var j = 0; j < count; j++)
        {
            Unspawn(array[j]);
        }
    }

    // Unspawns all the game objects created by the pool.
    public void UnspawnAll()
    {
        for (var i = 0; i < all.Count; i++)
        {
            GameObject obj = all[i] as GameObject;
            if (obj.activeInHierarchy)
                Unspawn(obj);
        }
    }

    // Unspawns all the game objects and clears the pool.
    public void Clear()
    {
        UnspawnAll();
        available.Clear();
        all.Clear();
    }

    // Returns the number of active objects.
    public int GetActiveCount()
    {
        return all.Count - available.Count;
    }

    // Returns the number of available objects.
    public int GetAvailableCount()
    {
        return available.Count;
    }

    // Returns the prefab being used by this pool.
    public GameObject GetPrefab()
    {
        return prefab;
    }

    // Applies the provided function to some or all of the pool's game objects.
    //public void ForEach(func : Function, activeOnly : boolean)
    //{
    //    for(var i = 0; i < all.Count; i++){
    //        var obj : GameObject = all[i] as GameObject;
    //        if(!activeOnly || obj.active){
    //            func(obj);
    //        }
    //    }
    //}

    // Activates or deactivates the provided game object using the method
    // specified by the setActiveRecursively flag.
    private void SetActive(GameObject obj, bool val)
    {
        obj.SetActive(val);
    }
}