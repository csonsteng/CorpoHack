using LogicPuddle.CardManagement;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runner
{
    public class RunnerHandDisplay : HandDisplayController
	{
		[SerializeField] private RunnerCardDisplay _cardTemplate;

		private RunnerHand _data;

		private Dictionary<IDraggableCard, RunnerCardData> _cardDictionary = new();

		protected override List<IDraggableCard> _cards => _cardDictionary.Keys.ToList();

		private Action<RunnerCardData> _onDragCallback;
		private Action<RunnerCardData> _onDropCallback;

		private IDraggableCard _processingCard;
		private Vector3 _drawPoint;

		private void Awake()
		{
			_cardTemplate.gameObject.SetActive(false);
		}

		public void Setup(RunnerHand hand, Transform drawPoint, Action<RunnerCardData> cardDraggedCallback, Action<RunnerCardData> cardDroppedCallback)
		{
			_data = hand;
			_drawPoint = drawPoint.position;
			_onDragCallback = cardDraggedCallback;
			_onDropCallback = cardDroppedCallback;
			_data.RegisterListeners(OnCardAdded, OnCardRemoved);
			_cards.Clear();
		}

		private void OnCardAdded(RunnerCardData cardData)
		{
			var card = SpawnCard(cardData);
			card.TurnFaceUp(_animationDuration);
			Resize();
		}

		private void OnCardRemoved(RunnerCardData card)
		{
			// logic should probably happen here rather than called AnimateToTrash
		}

		protected override void OnCardDragged(IDraggableCard card)
		{
			_processingCard = card;
			_onDragCallback.Invoke(_cardDictionary[card]);
		}
		protected override void OnCardDropped(IDraggableCard card)
		{
			_onDropCallback.Invoke(_cardDictionary[card]);
		}

		public void UnableToPlayCard()
		{
			_processingCard.ReturnToHand();
			_processingCard = null;
		}

		public void AnimateCardToTrash(Vector3 trashLocation)
		{
			_processingCard.MovePosition(trashLocation, _animationDuration);
			_processingCard.RotateInPlane(0f, _animationDuration);
			_processingCard.TurnFaceDown(_animationDuration);
			_cardDictionary.Remove(_processingCard);
			_processingCard = null;
			Resize();
		}



		private IDraggableCard SpawnCard(RunnerCardData cardData)
		{
			var card = Instantiate(_cardTemplate, _cardTemplate.transform.parent);
			card.gameObject.SetActive(true);
			card.Register(this);
			card.Setup(cardData, _drawPoint);
			card.Scale(_cardScaleSize.x, 0f);
			_cardDictionary[card] = cardData;
			return card;
		}
	}
}