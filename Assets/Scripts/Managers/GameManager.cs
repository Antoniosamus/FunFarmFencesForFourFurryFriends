using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	
	private int AINumber = 10;
	private int farmerNumber = 4;

    private XMLParser LevelFile = new XMLParser();
    private List<XMLParser.LevelData> lLevelData;
    private XMLParser.LevelData[] aLevelData;

    public float Percent { get { return IAManager.Instance.AnimalPool.all.Count / AINumber; } }

    public List<GameObject> AliveAnimals (string prefabName)
    {
        return IAManager.Instance.AnimalPool.all.FindAll(x => x.name.Contains(prefabName)).ConvertAll(x => x.gameObject).ToList();
    }

    [SerializeField]
    private float endTime;

	[SerializeField] GameObject farmerPrefab;
    private List<FarmerController> Farmers = new List<FarmerController>();

	void OnEnable () 
	{
		IAManager.Instance.Inizialize (AINumber);
        //Vector3 vector;

        //Por probar InitializePrefabs();
        for (int i = 0; i < farmerNumber; i++)
        {
            Vector3 v = GetPerifericPointInPlane();
            var f = Instantiate(farmerPrefab, v, Quaternion.identity) as GameObject;
            Farmers.Add(f.GetComponent<FarmerController>());
        }
	}

    void Update() 
    {
        if (Farmers.TrueForAll(x => !x._canFarm)) StartCoroutine(TimeOutEndGame());
    }

    IEnumerator TimeOutEndGame() 
    {
        yield return new WaitForSeconds(endTime);
        IAManager.Instance.Stop();
        Debug.Log("FINAL DEL JUEGO!");
    }

	public Vector3 GetPerifericPointInPlane()
	{
		Vector3 vecAux = Vector3.zero;
		Vector3 worldPoint = Vector3.zero;
    
		Vector2 screenSize = new Vector2(Screen.width, Screen.height);

		int ChoosenMax  = Random.Range(0, 1);
		int ChoosenSide = Random.Range(0, 1);
		
		if (ChoosenSide == 1) 
		{
			vecAux.x =  (ChoosenMax == 0) ? 0 : screenSize.x ;
			vecAux.y = Random.Range (0, screenSize.y);
		}
		else
		{
			vecAux.x = Random.Range (0, screenSize.x);
			vecAux.y =  (ChoosenMax == 0) ? 0 : screenSize.y ;
		}

    vecAux.z = -Camera.main.transform.position.z;
		worldPoint = Camera.main.ScreenToWorldPoint(vecAux);
		return new Vector3(worldPoint.x, worldPoint.y, 0);
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
        //aLevelData = lLevelData.ToArray();
        ////int i = 0;
        //foreach (XMLParser.AnimalData tempAnimalD in aLevelData[0].Animals)
        //{
        //    IAManager.Instance.prefabs[0].name = tempAnimalD.name;
        //    IAManager.Instance.Inizialize(tempAnimalD.AnimalAmount);
        //    //i++;
        //}

        //farmerNumber = aLevelData[0].FarmerAmount;
    }

}
