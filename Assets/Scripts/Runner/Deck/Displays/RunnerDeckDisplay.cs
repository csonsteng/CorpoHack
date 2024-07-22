using Runner.Deck;
using Runner;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RunnerDeckDisplay : MonoBehaviour
{
	[SerializeField] private Transform _cardHolder;
	[SerializeField] private TextMeshProUGUI _cardCountDisplay;
	[SerializeField] private RunnerCardDisplay _cardTemplate;
	private List<RunnerCardDisplay> _cards = new List<RunnerCardDisplay>();

	private RunnerCardDisplayManager _manager;
	private RunnerDeck _data;

	private void Awake()
	{
		_cardCountDisplay.text = "";
		_cardTemplate.gameObject.SetActive(false);
	}
	public void Setup(RunnerDeck deck, RunnerCardDisplayManager manager)
	{
		_data = deck;
		_manager = manager;
		_data.RegisterListeners(OnCardDrawn, OnDeckShuffled);
		OnDeckShuffled();
	}

	private void OnCardDrawn(RunnerCardData card)
	{
		// todo: copies of cards should probably have instance ids
		foreach (var cardDisplay in _cards.ToArray())
		{
			if (cardDisplay.Data == card)
			{
				_cards.Remove(cardDisplay);
				_manager.OnCardDrawn(cardDisplay);
				UpdateCardCount();
				return;
			}
		}
	}

	private void OnDeckShuffled()
	{
		foreach (var card in _cards)
		{
			Destroy(card);
		}
		_cards.Clear();
		var reverseOrder = new List<RunnerCardData>(_data.AllCurrentCards);
		reverseOrder.Reverse();
		foreach (var card in reverseOrder)
		{
			SpawnCard(card);
		}
	}

	private RunnerCardDisplay SpawnCard(RunnerCardData cardData)
	{
		var card = Instantiate(_cardTemplate, _cardTemplate.transform.parent);
		card.gameObject.SetActive(true);
		card.gameObject.name = cardData.Name;
		card.Setup(cardData);
		card.MovePositionLocal(new Vector3(0f, 0f, _cards.Count * card.CardThickness), 0f);
		card.Scale(1f, 0f);
		_cards.Insert(0, card);
		UpdateCardCount();
		return card;
	}

	private void UpdateCardCount()
	{
		_cardCountDisplay.text = _cards.Count > 0 ? _cards.Count.ToString() : "";
	}
}
