using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {

    private XMLParser LevelFile;
    private List<XMLParser.LevelData> lLevelData;
    

	// Use this for initialization
	void Awake () {
        IAManager.Instance.Inizialize(100);
	}

    void Start()
    {
        lLevelData = LevelFile.GetLevelData();
        //invocamos al IAMnager con los tipos de animales y cantidad de cada uno

    }
	
	// Update is called once per frame
	void Update () {
	
	}


}
