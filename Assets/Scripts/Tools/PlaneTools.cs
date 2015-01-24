using UnityEngine;
using System.Collections;

public class PlaneTools : MonoBehaviour{

	[SerializeField] GameObject plane;

	public Vector2 GetPerifericPoint()
	{
		Vector3 boundsSize = plane.collider.bounds.size;
		Vector2 result;

		int ChoosenMax = Random.Range (0, 1);
		int ChoosenSide = Random.Range (0, 1);

		if (ChoosenSide == 1) 
		{
			result.x =  (ChoosenMax == 0) ? 0 : boundsSize.x ;
			result.y = Random.Range (0, boundsSize.z);
		}
		else
		{
			result.x = Random.Range (0, boundsSize.x);
			result.y =  (ChoosenMax == 0) ? 0 : boundsSize.z ;
		}

		return result;
	}

	public Vector2 GetRandom()
	{
		Vector2 result;
		Vector3 boundsSize = plane.collider.bounds.size;

		result.x = Random.Range (0, boundsSize.x);
		result.y = Random.Range (0, boundsSize.z);

		return result;
	}
}
