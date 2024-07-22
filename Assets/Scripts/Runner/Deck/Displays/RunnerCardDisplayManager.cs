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
		[SerializeField] private RunnerHandDisplay _handDisplay;
		[SerializeField] private RunnerDeckDisplay _deckDisplay;
		[SerializeField] private RunnerTrashDisplay _trashDisplay;
		[SerializeField] private RunnerTargetManager _targetManager;

		[SerializeField] private GameObject _discardCardButton;

		[SerializeField] private Transform _deckLocation;

		private RunnerDeckManager _cardManager;
		private RunnerHand _handData;

		private void Awake()
		{
			_cardManager = RunnerDeckManager.Instance;
			_handData = _cardManager.Hand;
			_handDisplay.Setup(_handData, _deckLocation, this);
			_deckDisplay.Setup(_cardManager.Deck, this);
			_trashDisplay.Setup(_cardManager.Trash, _deckDisplay.transform.position);
			_targetManager.Register(this);
			_discardCardButton.SetActive(false);
		}
		private void Start()
		{
			_cardManager.StartGame();
		}

		public void OnCardDrawn(RunnerCardDisplay card)
		{
			Debug.Log($"adding {card.Data.Name} to hand");
			_handDisplay.OnCardAdded(card);
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
			AnimateCardToTrash();
			_discardCardButton.SetActive(false);
		}

		private void AnimateCardToTrash()
		{
			var card = _handDisplay.RemoveProcessingCard();
			_trashDisplay.AddCard(card);
		}



		public void OnTargetSelected(RunnerTargetData target)
		{
			_cardManager.PlayCard(_handDisplay.ProcessingCard.Data, target);
			AnimateCardToTrash();
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
			AnimateCardToTrash();
		}
	}
}
