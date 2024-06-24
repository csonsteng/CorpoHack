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

		public List<TCard> GetAll() => _cards;
		public void Add(TCard card) => _cards.AddToBottom(card);
		public bool Remove(TCard card) => _cards.RemoveCard(card);
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