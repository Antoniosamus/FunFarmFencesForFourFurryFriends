using UnityEngine;

public delegate void CollisionHandler(GameObject collision);

public interface IFarmerEvents
{
	event CollisionHandler OnCollideWithObstacle;
	event CollisionHandler OnCollideWithWayPoint;
  event CollisionHandler OnCollideWithAnimal;
}
