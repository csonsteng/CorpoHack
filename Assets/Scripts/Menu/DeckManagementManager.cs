using Runner;
using Runner.Deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckManagementManager : MonoBehaviour
{
	[SerializeField] private DeckManagementCard _template;
	[SerializeField] private TextMeshProUGUI _countDisplay;

	private Dictionary<RunnerCardData, DeckManagementCard> _cards = new Dictionary<RunnerCardData, DeckManagementCard>();

	private RunnerDeck _deck;
	private int _count;

	private void Start()
	{
		_template.gameObject.SetActive(false);
		_deck = RunnerDeckManager.Instance.Deck;
		_count = 0;
		foreach (var cardData in _deck.AllCards)
		{
			if(_cards.TryGetValue(cardData, out var display))
			{
				display.AddCopy();
				continue;
			}
			var cardDisplay = Instantiate(_template, _template.transform.parent);
			cardDisplay.gameObject.SetActive(true);
			cardDisplay.Setup(cardData, this);
			_count++;
			_cards.Add(cardData, cardDisplay);
		}
		UpdateCountDisplay();
	}

	public void OnCardAdded(RunnerCardData cardData)
	{
		_count++;
		UpdateCountDisplay();
	}
	public void OnCardRemoved(RunnerCardData cardData)
	{
		_count--;
		UpdateCountDisplay();
	}

	private void UpdateCountDisplay()
	{
		_countDisplay.text = $"Cards {_count}/15";
		_countDisplay.color = _count > 15 ? Color.red : Color.green;
	}
}
