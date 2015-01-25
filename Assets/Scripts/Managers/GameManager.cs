using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	
  private const int PositionOffset = 10;
  private int AINumber = 15;
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
		InvokeRepeating ("CheckIfEnd", 5.0f, 5.0f);
	}

    void Update() 
    {
        if (Farmers.TrueForAll(x => !x._canFarm)) StartCoroutine(TimeOutEndGame());
    }

    IEnumerator TimeOutEndGame() 
    {
        yield return new WaitForSeconds(endTime);
		GameOver ();
    }

	public void GameOver()
	{
		IAManager.Instance.Stop();
		
		UIManager.Instance.SetGameOver ();
		
		Application.LoadLevel ("EmptyScene");
		Debug.Log("FINAL DEL JUEGO!");
	}

	public Vector2 GetPerifericPointInPlane()
	{
		Vector3 vecAux = Vector3.zero;
		Vector3 worldPoint = Vector3.zero;
    
		Vector2 screenSize = new Vector2(Screen.width, Screen.height);
		
    int choosenSide = Random.Range(0, 2);
		if (choosenSide == 1) {
			vecAux.x = Random.Range(0, 2) * screenSize.x;
			vecAux.y = Random.Range (0, screenSize.y);
		
    } else {
			vecAux.x = Random.Range (0, screenSize.x);
			vecAux.y = Random.Range(0, 2) * screenSize.y ;
		}

    vecAux.x = Mathf.Clamp(vecAux.x, PositionOffset, Screen.width - PositionOffset);
    vecAux.y = Mathf.Clamp(vecAux.y, PositionOffset, Screen.height - PositionOffset);
    vecAux.z   = -Camera.main.transform.position.z;
		worldPoint = Camera.main.ScreenToWorldPoint(vecAux);
    
    return (Vector2) worldPoint;
	}
	
	public Vector2 GetRandomPointInPlane()
	{
		Vector2 result;
		Vector3 vecAux = Vector3.zero;
		
		Vector2 screenSize = new Vector2(Screen.width, Screen.height);

		vecAux.y = Random.Range(0f, screenSize.y);
		vecAux.x = Random.Range(0f, screenSize.x);
    vecAux.z = -Camera.main.transform.position.z;
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


	public void CheckIfEnd()
	{
		if (!(IAManager.Instance.AnimalPool.all.Any(x => (x.AnimalToHunt != null))))
			GameOver ();
	}

}
