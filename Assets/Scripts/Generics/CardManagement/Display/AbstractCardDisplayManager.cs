using LogicPuddle.Common;
using Runner.Deck;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public abstract class AbstractCardDisplayManager<THand, TCard, TCardDisplay, THandDisplay, TRarity, TEffect, TTargetManager, TTargetDisplay, TTargetData, TTargetConfiguration, TTarget> : MonoBehaviour
		where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where THand : AbstractHandData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>, new()
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
		[SerializeField] private THandDisplay _handDisplay;
		[SerializeField] private TTargetManager _targetManager;
		[SerializeField] private UniqueScriptableObjectList<TCard> _validCards;

		[SerializeField] private int _handSize;


		[SerializeField] private Transform _trashLocation;


		private THand _handData;



		private void Start()
		{
			_handData = new THand();
			var validCards = _validCards.GetAll();
			for(var i = 0; i < _handSize; i++)
			{
				validCards.Shuffle();
				_handData.Add(validCards[0]);
			}
			_handDisplay.Setup(_handData, OnCardDragged, OnCardDropped);
		}

		private void OnCardDragged(TCard card)
		{
			_targetManager.OnCardDragged(card);
		}

		private void OnCardDropped(TCard card)
		{
			if (!_targetManager.OnCardDropped())
			{
				_handDisplay.UnableToPlayCard();
				return;
			}

			// found a target for the card
			Debug.Log("card was played");
			_handDisplay.AnimateCardToTrash(_trashLocation.position);
		}
	}
}
