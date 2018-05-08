﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {

	public Scrollbar healthUI;
	public Scrollbar comboUI;

	public void SetHealthUI(float currHealth, float maxHealth)
	{
		float newSize = currHealth / maxHealth;
		Debug.Log (newSize);

		healthUI.size = newSize;
	}

	public void SetComboUI(float currCombo, float maxCombo)
	{
		comboUI.size = currCombo / maxCombo;
	}
}
