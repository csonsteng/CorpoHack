using LogicPuddle.CardManagement;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runner
{
	public class RunnerCardDisplayManager : MonoBehaviour
	{
		[SerializeField] private RunnerDeckManager _cardManager;
		[SerializeField] private RunnerHandDisplay _handDisplay;
		[SerializeField] private RunnerTargetManager _targetManager;

		[SerializeField] private GameObject _discardCardButton;

		[SerializeField] private Transform _trashLocation;
		[SerializeField] private Transform _deckLocation;

		private RunnerHand _handData;

		private void Awake()
		{
			_handData = _cardManager.Hand;
			_handDisplay.Setup(_handData, _deckLocation, this);
			_targetManager.Register(this);
			_discardCardButton.SetActive(false);
		}

		public void OnCardSelected(RunnerCardDisplay card)
		{
			_discardCardButton.SetActive(true);
			_targetManager.OnCardSelected(card.Data);
		}
		public void OnCardDeselected()
		{
			_discardCardButton.SetActive(false);
			_targetManager.OnCardDeselected();
		}

		public void Discard()
		{
			_targetManager.OnCardDeselected();
			_cardManager.Discard(_handDisplay.ProcessingCard.Data);
			_handDisplay.AnimateCardToTrash(_trashLocation.position);
			_discardCardButton.SetActive(false);
		}



		public void OnTargetSelected(RunnerTargetData target)
		{
			_cardManager.PlayCard(_handDisplay.ProcessingCard.Data, target);
			_handDisplay.AnimateCardToTrash(_trashLocation.position);
			_discardCardButton.SetActive(false);
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
