using UnityEngine;
using System.Collections;

public class CenteredTextPositioner : IFloatingTextPositioner {

	private float _speed;

	public CenteredTextPositioner(float speed) {
		_speed = speed;
	}

	public bool GetPosition(ref Vector2 position) { 
		return false;
	}
}
