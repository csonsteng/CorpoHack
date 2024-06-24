
using LogicPuddle.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement 
{
	public class AbstractDeckData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> : ISerializable
		where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TRarity : System.Enum
		where TEffect : AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget>
		where TTargetData : AbstractTargetData<TTargetConfiguration, TTarget>
		where TTargetConfiguration : AbstractTargetConfiguration<TTarget>
		where TTarget : System.Enum
	{
		private readonly CardContainer<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> _startingCards = new ();
		private readonly CardContainer<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> _currentCards = new();

		public TCard DrawCard() => _currentCards.DrawTopCard();
		public IEnumerable<TCard> Peek(int cardCount) => _currentCards.PeekCards(cardCount);
		public void AddToTop(TCard card) => _currentCards.AddToTop(card);
		public void AddToBottom(TCard card) => _currentCards.AddToBottom(card);
		public void AddToTop(List<TCard> cards) => _currentCards.AddToTop(cards);
		public void AddToBottom(List<TCard> cards) => _currentCards.AddToBottom(cards);
		public void UnlockCard(TCard card) => _startingCards.AddToBottom(card);
		public void UnlockCards(List<TCard> cards) => _startingCards.AddToBottom(cards);
		public void Shuffle() => _currentCards.Shuffle();
		public void Reset()
		{
			_currentCards.Clear();
			AddToTop(_startingCards);
			Shuffle();
		}


		public void Deserialize(Dictionary<string, object> data)
		{
			_startingCards.Deserialize((Dictionary<string, object>)data["starting"]);
			_currentCards.Deserialize((Dictionary<string, object>)data["current"]);
		}

		public Dictionary<string, object> Serialize()
		{
			return new Dictionary<string, object>
			{
				{ "starting" , _startingCards.Serialize() },
				{ "current" , _currentCards.Serialize() }
			};
		}

	}
}