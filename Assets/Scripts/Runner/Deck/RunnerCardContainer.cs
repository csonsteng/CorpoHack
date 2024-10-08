using LogicPuddle.Common;
using Runner.Deck;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
	public class RunnerCardContainer : ISerializable
	{
		private readonly List<RunnerCardData> _cards = new();

		public static implicit operator List<RunnerCardData>(RunnerCardContainer container) => container._cards;

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

		public RunnerCardData DrawTopCard()
		{
			if (_cards.Count == 0)
			{
				return null;
			}
			var target = _cards[0];
			_cards.RemoveAt(0);
			return target;
		}

		public RunnerCardData DrawBottomCard()
		{
			if (_cards.Count == 0)
			{
				return null;
			}
			var target = _cards[^1];
			_cards.RemoveAt(_cards.Count - 1);
			return target;
		}

		public IEnumerable<RunnerCardData> PeekCards(int cardCount)
		{
			for (var i = 0; i < Mathf.Min(cardCount, _cards.Count); i++)
			{
				yield return _cards[i];
			}
		}


		public void AddToBottom(RunnerCardData card) => _cards.Add(card);
		public void AddToBottom(List<RunnerCardData> cards)
		{
			foreach (var card in cards)
			{
				AddToBottom(card);
			}
		}

		public void AddToTop(RunnerCardData card) => _cards.Insert(0, card);
		public void AddToTop(List<RunnerCardData> cards)
		{
			foreach (var card in cards)
			{
				AddToTop(card);
			}
		}

		public bool RemoveCard(RunnerCardData card)
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

		private object CardIDList(List<RunnerCardData> list)
		{
			var cardIDs = new List<string>();
			foreach (var card in list)
			{
				cardIDs.Add(card.UniqueID);
			}
			return cardIDs;
		}

		private void FillListWithIDs(List<object> ids, List<RunnerCardData> targetList)
		{
			foreach (var id in ids)
			{
				if (UniqueScriptableObjectLinker.TryGetUniqueObject<RunnerCardData>(System.Convert.ToString(id), out var card))
				{
					targetList.Add(card);
				}
			}

		}
	}
}