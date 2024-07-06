using LogicPuddle.CardManagement;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
	public class RunnerCardDisplayManager : MonoBehaviour
	{
		[SerializeField] private RunnerDeckManager _cardManager;
		[SerializeField] private RunnerHandDisplay _handDisplay;
		[SerializeField] private RunnerTargetManager _targetManager;

		[SerializeField] private Transform _trashLocation;
		[SerializeField] private Transform _deckLocation;

		private RunnerHand _handData;

		private void Awake()
		{
			_handData = _cardManager.Hand;
			_handDisplay.Setup(_handData, _deckLocation, OnCardDragged, OnCardDropped);
		}

		private void OnCardDragged(RunnerCardData card)
		{
			_targetManager.OnCardDragged(card);
		}

		private void OnCardDropped(RunnerCardData card)
		{
			if (!_targetManager.OnCardDropped(out var target))
			{
				_handDisplay.UnableToPlayCard();
				return;
			}

			_cardManager.PlayCard(card, target);
			_handDisplay.AnimateCardToTrash(_trashLocation.position);
		}
	}
}
