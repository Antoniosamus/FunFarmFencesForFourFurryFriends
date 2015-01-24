using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {
	
	private int AINumber = 10;
	private int farmerNumber = 1;

	[SerializeField] GameObject farmerPrefab;

	void OnEnable () 
	{

		IAManager.Instance.Inizialize (AINumber);
		Vector3 vector;

		for (int i = 0; i < farmerNumber; i++) 
		{
			Instantiate (farmerPrefab, GetPerifericPointInPlane(), Quaternion.identity);
		}
	}

	
	public Vector3 GetPerifericPointInPlane()
	{
		Vector3 result;
		Vector3 vecAux = Vector3.zero;
		Vector3 vecCamera = Vector3.zero;

		Vector3 screenSize = new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 10);

		int ChoosenMax = Random.Range (0, 1);
		int ChoosenSide = Random.Range (0, 1);
		
		if (ChoosenSide == 1) 
		{
			vecAux.x =  (ChoosenMax == 0) ? 0 : screenSize.x ;
			vecAux.y = Random.Range (0, screenSize.z);
		}
		else
		{
			vecAux.x = Random.Range (0, screenSize.x);
			vecAux.y =  (ChoosenMax == 0) ? 0 : screenSize.z ;
		}


		vecCamera = Camera.main.ScreenToWorldPoint (vecAux);
		result = new Vector3 (vecCamera.x, vecCamera.y, 10);


		return result;
	}
	
	public Vector2 GetRandomPointInPlane()
	{
		Vector2 result;
		Vector3 vecAux = Vector3.zero;
		
		Vector3 screenSize = new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, 10);

		vecAux.y = Random.Range (0, screenSize.z);
		vecAux.x = Random.Range (0, screenSize.x);

		result = (Vector2)(Camera.main.ScreenToWorldPoint(vecAux));
		
		return result;
	}

}
