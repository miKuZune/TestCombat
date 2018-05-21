using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {

	public Scrollbar healthUI;
	public Scrollbar comboUI;

	public void SetHealthUI(float currHealth, float maxHealth)
	{
        if (healthUI == null) { return; }

		float newSize = currHealth / maxHealth;

		healthUI.size = newSize;
	}

	public void SetComboUI(float currCombo, float maxCombo)
	{
        if (comboUI == null) { return; }
		comboUI.size = currCombo / maxCombo;
	}
}
