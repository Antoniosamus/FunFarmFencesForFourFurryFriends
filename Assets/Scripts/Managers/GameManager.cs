using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	
	private int AINumber = 10;
	private int farmerNumber = 4;

    private XMLParser LevelFile = new XMLParser();
    private List<XMLParser.LevelData> lLevelData;
    private XMLParser.LevelData[] aLevelData;

	[SerializeField] GameObject farmerPrefab;

	void OnEnable () 
	{
		IAManager.Instance.Inizialize (AINumber);
        //Vector3 vector;

        //Por probar InitializePrefabs();
        for (int i = 0; i < farmerNumber; i++)
        {
            Vector3 v = GetPerifericPointInPlane();
            v.x += Random.Range(-15, 15);
            v.y += Random.Range(-15, 15);
            Instantiate(farmerPrefab, v, Quaternion.identity);
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

		vecAux.y = Random.Range (0, screenSize.y);
		vecAux.x = Random.Range (0, screenSize.x);

		result = (Vector2)(Camera.main.ScreenToWorldPoint(vecAux));
		
		return result;
	}

    public void InitializePrefabs()
    {
        aLevelData = lLevelData.ToArray();
        //int i = 0;
        foreach (XMLParser.AnimalData tempAnimalD in aLevelData[0].Animals)
        {
            IAManager.Instance.prefabs[0].name = tempAnimalD.name;
            IAManager.Instance.Inizialize(tempAnimalD.AnimalAmount);
            //i++;
        }

        farmerNumber = aLevelData[0].FarmerAmount;
    }

}
