﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager> {

	[SerializeField] Button startGameButton;
	[SerializeField] Button continueGameButton;

	[SerializeField] Button soundOnOff;

	[SerializeField] GameObject [] starsOff;
	[SerializeField] GameObject [] starsOn;

	[SerializeField] GameObject [] bossesOff;
	[SerializeField] GameObject [] bossesOn;

	[SerializeField] GameObject [] badges;

	[SerializeField] Text stageLabel;

	[SerializeField] Text highScoreLabel;
	[SerializeField] Text scoreLabel;

	[SerializeField] Sprite [] badgesSprites;
	
	[SerializeField] GameObject gameOverPanel;
	[SerializeField] GameObject pausePanel;
  [SerializeField] GameObject startPanel;

	public void OnEnable()
	{
	}

	public void OnDisable()
	{
	
	}

	public void StartGame()
	{
		Application.LoadLevelAdditive ("Main");
		startPanel.SetActive(false);
	}

	public void SetGameOver()
	{
		//this.gameObject.SetActive (true);
		gameOverPanel.SetActive (true);
		SetStars(GameManager.Instance.Percent);
	}

	public void SetBadges()
	{

	}

	public void SetStars(float percentage)
	{
		float divided = percentage / 3;

		starsOff [0].SetActive (percentage < 33);
		starsOff [1].SetActive (percentage < 66);
		starsOff [2].SetActive (percentage < 100);

		starsOn [0].SetActive (percentage > 33);
		starsOn [1].SetActive (percentage > 66);
		starsOn [2].SetActive (percentage == 100);
	}

	public void pauseClicked()
	{
		Time.timeScale = 0;
    pausePanel.SetActive(true);
	}

	public void ContinueClicked()
	{
		Time.timeScale = 1;
    pausePanel.SetActive(false);
	}


	public void AudioClicked()
	{
		AudioListener.pause = !AudioListener.pause;
	}


	public void SetPoints(int highScore = 0, int score = 0, int stage = 1)
	{
		highScoreLabel.text = highScore.ToString ();
		scoreLabel.text = score.ToString ();
		stageLabel.text = score.ToString ();
	}
}
