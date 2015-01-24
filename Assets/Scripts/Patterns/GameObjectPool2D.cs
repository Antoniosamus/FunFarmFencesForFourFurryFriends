
using UnityEngine;

public class GameObjectPool2D : GameObjectPool
{
  public GameObjectPool2D(GameObject prefab, int initialCapacity) : base(prefab, initialCapacity) {}
 
  public GameObject Spawn(Vector2 position, Quaternion rotation)
	{
		return base.Spawn( new Vector3(position.x, position.y, prefab.transform.position.z), rotation );
	}

}
