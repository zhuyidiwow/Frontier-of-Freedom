using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	
	[SerializeField] private Text powerupText;

	private int power = 10;

	private void Start() {
		Instance = this;
		PickupPower();
	}

	public void PickupPower() {
		power++;
		powerupText.text = "Powerup: " + power.ToString();
	}

	public bool HasPower() {
		return power >= 1;
	}
	public void UsePower() {
		power--;
		powerupText.text = "Powerup: " + power.ToString();
	}
}
