using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {

    private XMLParser LevelFile = new XMLParser();
    private List<XMLParser.LevelData> lLevelData;
    private XMLParser.LevelData[] aLevelData;
    

	// Use this for initialization
	void Awake () {
        //IAManager.Instance.Inizialize(100);
	}

    void Start()
    {
        lLevelData = LevelFile.GetLevelData();
        if (lLevelData == null)
            Debug.LogError("ERROR carga de fichero xml");
        //invocamos al IAMnager con los tipos de animales y cantidad de cada uno
        InitializeIA();
    }
	
	// Update is called once per frame
	void Update () {
	
	}



    public void InitializeIA()
    {
        aLevelData = lLevelData.ToArray();
        //int i = 0;
        foreach (XMLParser.AnimalData tempAnimalD in aLevelData[0].Animals)
        {
            IAManager.Instance.prefabs[0].name = tempAnimalD.name;
            IAManager.Instance.Inizialize(tempAnimalD.AnimalAmount);
            //i++;
        }
    }
}
