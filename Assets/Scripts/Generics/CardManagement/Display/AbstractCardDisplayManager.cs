using LogicPuddle.Common;
using Runner.Deck;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public abstract class AbstractCardDisplayManager<TCardManager, THand, TDeck, TTrash, TCard, TCardDisplay, THandDisplay, TRarity, TEffect, TTargetManager, TTargetDisplay, TTargetData, TTargetConfiguration, TTarget> : MonoBehaviour
		where TCardManager : AbstractCardManager<THand, TDeck, TTrash, TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where THand : AbstractHandData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>, new()
		where TDeck : AbstractDeckData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>, new()
		where TTrash : AbstractTrashData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>, new()
		where THandDisplay : AbstractHandDisplay<THand, TCard, TCardDisplay, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TCardDisplay : AbstractCardDisplay<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TTargetManager : AbstractTargetManager<TTargetDisplay, TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TTargetDisplay : AbstractTargetDisplay<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TRarity : Enum
		where TEffect : AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget>
		where TTargetData : AbstractTargetData<TTargetConfiguration, TTarget>, new()
		where TTargetConfiguration : AbstractTargetConfiguration<TTarget>
		where TTarget : Enum
    {

		[SerializeField] private TCardManager _cardManager;
		[SerializeField] private THandDisplay _handDisplay;
		[SerializeField] private TTargetManager _targetManager;

		[SerializeField] private Transform _trashLocation;
		[SerializeField] private Transform _deckLocation;


		private THand _handData;
		


		private void Awake()
		{
			_handData = _cardManager.Hand;
			_handDisplay.Setup(_handData, _deckLocation, OnCardDragged, OnCardDropped);
		}

		private void OnCardDragged(TCard card)
		{
			_targetManager.OnCardDragged(card);
		}

		private void OnCardDropped(TCard card)
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
