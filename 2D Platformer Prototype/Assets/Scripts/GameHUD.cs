using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameHUD : MonoBehaviour {

	public Text scoreText;
	public Text timeText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = string.Format("Points: {0}", GameManager.instance.points);
		TimeSpan time = LevelManager.instance.runningTime;
		timeText.text = string.Format("{0:00}:{1:00} with {2} bonus", time.Minutes + (time.Hours * 60), time.Seconds, LevelManager.instance.currentTimeBonus);
	}
}
