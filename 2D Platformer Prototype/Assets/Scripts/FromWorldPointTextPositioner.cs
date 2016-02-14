using UnityEngine;
using System.Collections;

public class FromWorldPointTextPositioner : IFloatingTextPositioner {
	
	private float _timeToLive;
	private float _speed;

	public FromWorldPointTextPositioner(float timeToLive, float speed) {
		_timeToLive = timeToLive;
		_speed = speed;
	}

	public bool GetPosition(ref Vector2 position) {
		if ((_timeToLive -= Time.deltaTime) <= 0) {
			return false;
		}
		position.y += Time.deltaTime * _speed;

		return true;
	}
}
