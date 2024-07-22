using Runner;
using Runner.Deck;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckManagementCard : MonoBehaviour
{
    [SerializeField] private BaseRunnerCardDisplay _cardDisplay;
    [SerializeField] private Button _removeButton;
    [SerializeField] private TextMeshProUGUI _countDisplay;

    private RunnerCardData _data;
    private int _copyCount;
    private DeckManagementManager _manager;

	public void Setup(RunnerCardData data, DeckManagementManager manager)
    {
        _data = data;
        _manager = manager;
        _cardDisplay.Setup(data);
        _copyCount = 1;
        UpdateCountDisplay();
	}

    public void SetCount(int count)
    {
        _copyCount = count;
        UpdateCountDisplay();
    }

    /// <summary>
    /// Exposed for inspector
    /// </summary>
    public void AddCopy()
    {
        _copyCount++;
        _manager.OnCardAdded(_data);
		UpdateCountDisplay();
	}
	/// <summary>
	/// Exposed for inspector
	/// </summary>
	public void RemoveCopy()
    {
		_copyCount--;
		_manager.OnCardRemoved(_data);
		UpdateCountDisplay();
	}

    private void UpdateCountDisplay()
    {
        _countDisplay.text = _copyCount.ToString();
        _removeButton.interactable = _copyCount > 0;   
    }
}
