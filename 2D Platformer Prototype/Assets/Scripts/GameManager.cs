using UnityEngine;
using System.Collections;

public class GameManager {

	private static GameManager _instance;

	public static GameManager instance {
		get {
			return _instance ?? (_instance = new GameManager());
		}
	}

	private GameManager() {

	}

	public int points {
		get;
		private set;
	}

	public void Reset() {
		points = 0;
	}

	public void ResetPoints(int p) {
		points = p;
	}

	public void AddPoints(int p) {
		points += p;
	}
}
