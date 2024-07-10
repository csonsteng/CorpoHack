using Runner;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetectionTracker : MonoBehaviour
{
	public static DetectionTracker Instance => GetInstance();

	private static DetectionTracker GetInstance()
	{
		if (_instance == null)
		{
			_instance = FindObjectOfType<DetectionTracker>();
		}
		return _instance;
	}
	private static DetectionTracker _instance;

	[SerializeField] private TextMeshProUGUI _display;

	private Action<int> _detectionLevelChanged;
	private int _detectionCount = 0;

	private void Start()
	{
		_display.text = "0";
	}

	public void IncreaseDetection(int amount)
    {
		if(amount == 0)
		{
			return;
		}
        _detectionCount += amount;
        _display.text = _detectionCount.ToString();
		_detectionLevelChanged?.Invoke(_detectionCount);
    }

	public void Register(Action<int> detectionLevelChanged)
	{
		_detectionLevelChanged = detectionLevelChanged;
	}
}
