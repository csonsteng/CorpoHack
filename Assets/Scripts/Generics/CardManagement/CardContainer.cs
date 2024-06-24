using LogicPuddle.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public class CardContainer <TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> : ISerializable
		where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TRarity : System.Enum
		where TEffect : AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget>
		where TTargetData : AbstractTargetData<TTargetConfiguration, TTarget>
		where TTargetConfiguration : AbstractTargetConfiguration<TTarget>
		where TTarget : System.Enum
	{
		private readonly List<TCard> _cards = new();

		public static implicit operator List<TCard>(CardContainer<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> container) => container._cards;

		public void Deserialize(Dictionary<string, object> data)
		{
			_cards.Clear();
			FillListWithIDs((List<object>)data["cards"], _cards);
		}

		public Dictionary<string, object> Serialize()
		{
			return new Dictionary<string, object>
			{
				{ "cards" , CardIDList(_cards) },
			};
		}

		public TCard DrawTopCard()
		{
			if(_cards.Count == 0)
			{
				return null;
			}
			var target = _cards[0];
			_cards.RemoveAt(0);
			return target;
		}

		public TCard DrawBottomCard()
		{
			if (_cards.Count == 0)
			{
				return null;
			}
			var target = _cards[^1];
			_cards.RemoveAt(_cards.Count - 1);
			return target;
		}

		public IEnumerable<TCard> PeekCards(int cardCount)
		{
			for (var i = 0; i < Mathf.Min(cardCount, _cards.Count); i++)
			{
				yield return _cards[i];
			}
		}


		public void AddToBottom(TCard card) => _cards.Add(card);
		public void AddToBottom(List<TCard> cards)
		{
			foreach (var card in cards)
			{
				AddToBottom(card);
			}
		}

		public void AddToTop(TCard card) => _cards.Insert(0, card);
		public void AddToTop(List<TCard> cards)
		{
			foreach (var card in cards)
			{
				AddToTop(card);
			}
		}

		public bool RemoveCard(TCard card)
		{
			if (_cards.Contains(card))
			{
				_cards.Remove(card);
				return true;
			}
			return false;
		}

		public void Clear() => _cards.Clear();


		public void Shuffle() => _cards.Shuffle();

		private object CardIDList(List<TCard> list)
		{
			var cardIDs = new List<string>();
			foreach (var card in list)
			{
				cardIDs.Add(card.UniqueID);
			}
			return cardIDs;
		}

		private void FillListWithIDs(List<object> ids, List<TCard> targetList)
		{
			foreach (var id in ids)
			{
				if (UniqueScriptableObjectLinker.TryGetUniqueObject<TCard>(System.Convert.ToString(id), out var card))
				{
					targetList.Add(card);
				}
			}

		}
	}
}