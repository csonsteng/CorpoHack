using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public class AbstractHandDisplay<THand, TCard, TCardDisplay, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> : HandDisplayController
		where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where THand : AbstractHandData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TCardDisplay : AbstractCardDisplay<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TRarity : Enum
		where TEffect : AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget>
		where TTargetData : AbstractTargetData<TTargetConfiguration, TTarget>
		where TTargetConfiguration : AbstractTargetConfiguration<TTarget>
		where TTarget : Enum
	{
		[SerializeField] private TCardDisplay _cardTemplate;

		private THand _data;

		private Dictionary<IDraggableCard, TCard> _cardDictionary = new();

		protected override List<IDraggableCard> _cards => _cardDictionary.Keys.ToList();

		private Action<TCard> _onDragCallback;
		private Action<TCard> _onDropCallback;

		private IDraggableCard _processingCard;
		
		private void Awake()
		{
			_cardTemplate.gameObject.SetActive(false);
		}

		public void Setup(THand hand, Action<TCard> cardDraggedCallback, Action<TCard> cardDroppedCallback)
		{
			_data = hand;
			_onDragCallback = cardDraggedCallback;
			_onDropCallback = cardDroppedCallback;
			_cards.Clear();
			foreach (var cardData in _data.GetAll())
			{
				SpawnCard(cardData);
			}
			Resize();
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
			_processingCard.TweenPosition(trashLocation, _animationDuration);
			_cardDictionary.Remove(_processingCard);
			_processingCard = null;
			Resize();
		}



		private IDraggableCard SpawnCard(TCard cardData)
		{
			var card = Instantiate(_cardTemplate, _cardTemplate.transform.parent);
			card.gameObject.SetActive(true);
			card.Register(this);
			card.Setup(cardData);
			card.TweenScale(_cardScaleSize.x, 0f);
			_cardDictionary[card] = cardData;
			return card;
		}

		public void OnCardDraw(TCard cardData, Vector3 drawPoint)
		{
			var card = SpawnCard(cardData);
			card.TweenPosition(drawPoint, 0f);
			Resize();
		}


	}
}