using System;
using System.Collections;
using System.Collections.Generic;
using LogicPuddle.Common;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
	public class AbstractHandData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> : ISerializable
		where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TRarity : System.Enum
		where TEffect : AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget>
		where TTargetData : AbstractTargetData<TTargetConfiguration, TTarget>
		where TTargetConfiguration : AbstractTargetConfiguration<TTarget>
		where TTarget : System.Enum
	{
		private readonly CardContainer<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> _cards = new();

		public Action<TCard> CardAdded;
		public Action<TCard> CardRemoved;
		public List<TCard> GetAll() => _cards;

		public void RegisterListeners(Action<TCard> cardAdded, Action<TCard> cardRemoved)
		{
			CardAdded += cardAdded;
			CardRemoved += cardRemoved;
		}
		public void UnregisterListeners(Action<TCard> cardAdded, Action<TCard> cardRemoved)
		{
			CardAdded -= cardAdded;
			CardRemoved -= cardRemoved;
		}
		public void Add(TCard card)
		{
			_cards.AddToBottom(card);
			CardAdded?.Invoke(card);
		}
		public bool Remove(TCard card)
		{
			if (!_cards.RemoveCard(card))
			{
				return false;
			}
			CardRemoved?.Invoke(card);
			return true;
		}
		public void Clear() => _cards.Clear();

		public void Deserialize(Dictionary<string, object> data)
		{
			_cards.Deserialize((Dictionary<string, object>)data["cards"]);
		}

		public Dictionary<string, object> Serialize()
		{
			return new Dictionary<string, object>
			{
				{ "cards" , _cards.Serialize() },
			};
		}
	}
}