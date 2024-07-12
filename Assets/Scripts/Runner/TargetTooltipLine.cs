using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetTooltipLine : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _text;

	public void SetLabel(string text)
	{
		_text.text = text;
	}
}
