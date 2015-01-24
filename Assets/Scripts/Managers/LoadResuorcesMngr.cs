using UnityEngine;
using System.Collections;

static public class LoadResourceMngr
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="path"> Path, it can be null, root path of resources.</param>
    /// <param name="goParent"> GameObject parent of the resource that we load </param>
    /// <param name="goResource"> Return the gameobject loading</param>
    static public void LoadResources(string resourceName, string path, GameObject goParent, out GameObject goResource)
    {
        goResource = null;
        if (resourceName != null)
        {
            if (goParent != null)
            {
                Debug.Log("Ruta y recurso: " + path + resourceName + ".prefab");

                GameObject go = (GameObject)Resources.Load(path + resourceName, typeof(GameObject));

                Debug.Log("Nombre recurso " + go.name);

                goResource = GameObject.Instantiate(go) as GameObject;

                goResource.transform.parent = goParent.transform;
            }
            else
            {
                Debug.LogError("Error goParent in LoadResources is null");
            }
        }
        else
        {
            Debug.LogError("Error resourceName in LoadResources is null");

        }
    }

    /// <summary>
    /// Override without gameobject parent
    /// </summary>
    /// <param name="resourceName"> Name of resource to load</param>
    /// <param name="path"> Path</param>
    /// <param name="goResource"> Return the gameobject loading</param>
    static public void LoadResources(string resourceName, string path, out GameObject goResource)
    {
        goResource = null;
        if (resourceName != null)
        {
            goResource = Resources.Load(path + resourceName, typeof(GameObject)) as GameObject;
        }
        else
        {
            Debug.LogError("Error resourceName in LoadResources is null");
        }
    }

    /// <summary>
    /// Instantiate an object and add it to the specified parent.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="prefab"></param>
    /// <param name="bParentScale"> Works with the parent or with the prefab has</param>
    /// <returns></returns>
    static public GameObject AddChild(GameObject parent, GameObject prefab, bool bParentScale)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;

        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            if (bParentScale == true)
            {
                t.localScale = Vector3.one;
            }
            go.layer = parent.layer;
        }

        return go;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="prefab"></param>
    /// <param name="bParentScale"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    static public GameObject AddChild(GameObject parent, GameObject prefab, bool bParentScale, Vector3 scale)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;

        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            if (bParentScale == true)
            {
                t.localScale = scale;
            }
            go.layer = parent.layer;
        }

        return go;
    }
}
