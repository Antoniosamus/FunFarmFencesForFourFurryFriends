using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager> 
{
  [SerializeField]	
  private GameObject fencePrefab;

  private const int PositionOffset = 2;
  public int AINumber = 15;
	private int farmerNumber = 4;

    private XMLParser LevelFile = new XMLParser();
    private List<XMLParser.LevelData> lLevelData;
    private XMLParser.LevelData[] aLevelData;

    public float Percent { get { return IAManager.Instance.AnimalPool.all.Count / AINumber; } }

	public List<GameObject> fencesInternal;

    public List<GameObject> AliveAnimals (string prefabName)
    {
        return IAManager.Instance.AnimalPool.all.FindAll(x => x.name.Contains(prefabName)).ConvertAll(x => x.gameObject).ToList();
    }

    [SerializeField]
    private float endTime;

	[SerializeField] GameObject farmerPrefab;
  public List<FarmerController> Farmers = new List<FarmerController>();
  private Vector2 _bottomLeft;
  private Vector2 _bottomRight;
  private Vector2 _upLeft;
  private Vector2 _upRight;


  void OnEnable () 
	{
    FenceWorld();
		Reinicialize ();
		InvokeRepeating ("CheckIfEnd", 5.0f, 5.0f);
	}


	public void Reinicialize()
	{
		IAManager.Instance.Inizialize(AINumber);
		//Vector3 vector;
		
		//Por probar InitializePrefabs();
		for (int i = 0; i < farmerNumber; i++)
		{
			Vector3 v = GetPerifericPointInPlane();
			var f = Instantiate(farmerPrefab, v, Quaternion.identity) as GameObject;
			Farmers.Add(f.GetComponent<FarmerController>());
		}
	}

  private void FenceWorld()
  {
    _bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, -Camera.main.transform.position.z));
    _bottomRight= Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, -Camera.main.transform.position.z));
    _upLeft     = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, -Camera.main.transform.position.z));
    _upRight    = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -Camera.main.transform.position.z));
  
    float fenceWidth = (fencePrefab.collider2D as CircleCollider2D).radius * 2;
    int verticalFencesCount   =  Mathf.CeilToInt( (_upLeft      - _bottomLeft).magnitude/ fenceWidth );
    int horizontalFencesCount =  Mathf.CeilToInt( (_bottomRight - _bottomLeft).magnitude/ fenceWidth );

    for(int i = 0; i < verticalFencesCount; ++i){
      ((GameObject) Instantiate(fencePrefab, _bottomLeft  +  i * fenceWidth * Vector2.up, Quaternion.identity)).collider2D.enabled = true;
      ((GameObject) Instantiate(fencePrefab, _bottomRight +  i * fenceWidth * Vector2.up, Quaternion.identity)).collider2D.enabled = true;
    }

    for(int i = 0; i < horizontalFencesCount; ++i){
      ((GameObject) Instantiate(fencePrefab, _bottomLeft  +  i * fenceWidth * Vector2.right, Quaternion.FromToRotation(Vector2.up, Vector3.right))).collider2D.enabled = true;
      ((GameObject) Instantiate(fencePrefab, _upLeft      +  i * fenceWidth * Vector2.right, Quaternion.FromToRotation(Vector2.up, Vector3.right))).collider2D.enabled = true;
    }
      
  }

  public void GameOver()
	{
		
		UIManager.Instance.SetGameOver();
		
		//Application.LoadLevel ("EmptyScene");
		Debug.Log("FINAL DEL JUEGO!");
	}

	public Vector2 GetPerifericPointInPlane()
	{
	  switch (Random.Range(0,4))
	  {
      case 0:
      return new Vector2(Random.Range(_bottomLeft.x, _bottomRight.x), _bottomLeft.y + PositionOffset);
      case 1:
      return new Vector2(Random.Range(_upLeft.x, _upRight.x)        , _upLeft.y - PositionOffset);
      case 2:
      return new Vector2(_bottomLeft.x  + PositionOffset, Random.Range(_bottomLeft.y, _upLeft.y) );
      default:
      return new Vector2(_bottomRight.x - PositionOffset, Random.Range(_bottomRight.y, _upRight.y) );
    }
	}
	
	public Vector2 GetRandomPointInPlane()
	{
		return new Vector2(Random.Range(_bottomLeft.x, _upRight.x), Random.Range(_bottomLeft.y, _upRight.y));;
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
		if (!(IAManager.Instance.AnimalPool.all.Any(x => (x.AnimalToHunt != null)))
      || Farmers.Count == 0 || Farmers.All(f => f == null))
			GameOver ();

        if (IAManager.Instance.AnimalPool.all.All(x => x.samePlace > 300))
            GameOver();
	}

	public void Clean ()
	{

		foreach (GameObject obj in fencesInternal) 
		{
			if(obj != null)
				Destroy(obj);
		}

		fencesInternal.Clear ();

		foreach (FarmerController obj in Farmers) 
		{
			if (obj != null)
				Destroy(obj.gameObject);
		}

		Farmers.Clear ();


		foreach (AnimalBehaviour obj in IAManager.Instance.AnimalPool.all) 
		{
			if (obj != null)
				Destroy(obj.gameObject);
		}


	}
}
