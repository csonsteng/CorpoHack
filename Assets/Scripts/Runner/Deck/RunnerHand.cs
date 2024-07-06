using LogicPuddle.CardManagement;
using Runner.Deck.Effects;
using Runner.Target;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using System;
using LogicPuddle.Common;

namespace Runner.Deck
{
    public class RunnerHand : ISerializable
    {
		private readonly RunnerCardContainer _cards = new();

		public Action<RunnerCardData> CardAdded;
		public Action<RunnerCardData> CardRemoved;
		public List<RunnerCardData> GetAll() => _cards;

		public void RegisterListeners(Action<RunnerCardData> cardAdded, Action<RunnerCardData> cardRemoved)
		{
			CardAdded += cardAdded;
			CardRemoved += cardRemoved;
		}
		public void UnregisterListeners(Action<RunnerCardData> cardAdded, Action<RunnerCardData> cardRemoved)
		{
			CardAdded -= cardAdded;
			CardRemoved -= cardRemoved;
		}
		public void Add(RunnerCardData card)
		{
			_cards.AddToBottom(card);
			CardAdded?.Invoke(card);
		}
		public bool Remove(RunnerCardData card)
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