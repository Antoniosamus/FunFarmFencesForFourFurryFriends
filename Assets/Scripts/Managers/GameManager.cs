using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using SimpleJSON;

public class GameManager : Singleton<GameManager> 
{
  [SerializeField]	
  private GameObject fencePrefab;

    private const int PositionOffset = 2;
    public int AINumber = 15;
	private int farmerNumber = 4;

    public float Percent { get { 
			return IAManager.Instance.AnimalPool.all.Count() / (float)AINumber; } }

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
	    Inicialize ();
	    InvokeRepeating ("CheckIfEnd", 5.0f, 5.0f);
    }

    JSONArray JsonLevels; 
    private int currentLevel = 1;
    private int LastLevel { get { return JsonLevels.AsArray.Count - 1; } }
    static string jsonFile = "Levels.json";

	public void Inicialize()
	{
        //Load texture from disk
        TextAsset json = Resources.Load("Levels") as TextAsset;
        JsonLevels = JSON.Parse(json.text).AsArray;
        LoadLevel(currentLevel);
	}

    public void LoadLevel(int numLevel)
    {
        Clean();

        var Level = JsonLevels[numLevel];

        IAManager.Instance.Inizialize(Level["Animals"].AsArray.Childs.ToList());

        foreach(var farmer in Level["Farmers"].AsArray.Childs)
        {
            var f = Instantiate(farmerPrefab, GetInWorld(farmer["x"].AsFloat, farmer["y"].AsFloat), Quaternion.identity) as GameObject;
            //Farmers.Add(f.GetComponent<FarmerController>());
        }
    }

    public static Vector3 GetInWorld(float x, float y)
    {
        Vector3 scale = new Vector3();

        scale.x = ChangeScale(x, 0, 100, 0, Screen.width);
        scale.y = ChangeScale(y, 0, 100, 0, Screen.height);
        scale.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(scale);
    }

    public static float ChangeScale(float x, float A, float B, float C, float D) 
    {
        return C * (1 - (x - A) / (B - A)) + D * (x - A) / (B - A);
    }

    public void NextLevel()
    {
        currentLevel++;
        LoadLevel(currentLevel);
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
			if(obj != null)
				Destroy(obj);

		fencesInternal.Clear ();

		foreach (FarmerController obj in Farmers) 
			if (obj != null)
				Destroy(obj.gameObject);

		Farmers.Clear ();

        if (IAManager.Instance.AnimalPool != null)
		    foreach (AnimalBehaviour obj in IAManager.Instance.AnimalPool.all) 
			    if (obj != null)
				    Destroy(obj.gameObject);
	}
}
