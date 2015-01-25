﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIManager : MonoBehaviour {

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

	[SerializeField] GameObject [] badgesSpritesDefault;

	public void OnEnable()
	{

	}

	public void OnDisable()
	{

	}

	public void SetBadges()
	{

	}

	public void SetStars(int percentage)
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
	}

	public void ContinueClicked()
	{
		Time.timeScale = 1;
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