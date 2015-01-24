using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	// Use this for initialization
	void Awake () {
        IAManager.Instance.Inizialize(4);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
