using Runner;
using Runner.Deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckManagementController : MonoBehaviour
{
	[SerializeField] private DeckManagementCard _template;
	[SerializeField] private TextMeshProUGUI _countDisplay;
	[SerializeField] private RunnerStartingDeck _startingDeck;

	private Dictionary<RunnerCardData, DeckManagementCard> _cards = new Dictionary<RunnerCardData, DeckManagementCard>();

	private RunnerDeck _deck;

	private void Start()
	{
		_template.gameObject.SetActive(false);
		RunnerDeckManager.Instance.UnlockCards(_startingDeck.Cards);
		_deck = RunnerDeckManager.Instance.Deck;
		foreach (var cardData in RunnerDeckManager.Instance.UnlockedCards.Cards)
		{
			var cardDisplay = Instantiate(_template, _template.transform.parent);
			cardDisplay.gameObject.SetActive(true);
			cardDisplay.Setup(cardData, this);
			_cards.Add(cardData, cardDisplay);
		}
		foreach (var cardData in _startingDeck.Cards)
		{
			if (_cards.TryGetValue(cardData, out var displayCard))
			{
				displayCard.AddCopy();
			}
		}
	}

	public void OnCardAdded(RunnerCardData cardData)
	{
		_deck.AddCardToStartingDeck(cardData);
		UpdateCountDisplay();
	}
	public void OnCardRemoved(RunnerCardData cardData)
	{
		_deck.RemoveCardFromStartingDeck(cardData);
		UpdateCountDisplay();
	}

	public void Play()
	{
		SceneManager.LoadScene("Main");
	}

	private void UpdateCountDisplay()
	{
		var count = _deck.TotalCount;
		var maxSize = RunnerDeckManager.Instance.Rig.GetMaxDeckSize();
		var handSize = RunnerDeckManager.Instance.HandSize;
		_countDisplay.text = $"Programs {count}/{maxSize} (Hand Size: {handSize})";
	}
}
