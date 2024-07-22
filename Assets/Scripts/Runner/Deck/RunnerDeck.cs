using LogicPuddle.CardManagement;
using Runner.Deck.Effects;
using Runner.Target;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using LogicPuddle.Common;
using System;

namespace Runner.Deck
{
    public class RunnerDeck : ISerializable
	{
		private readonly RunnerCardContainer _startingCards = new();
		private readonly RunnerCardContainer _currentCards = new();

		private Action<RunnerCardData> _cardDrawn;
		private Action _deckShuffled;

		public void RegisterListeners(Action<RunnerCardData> cardDrawn, Action deckShuffled)
		{
			_cardDrawn += cardDrawn;
			_deckShuffled += deckShuffled;
		}

		public RunnerCardData DrawCard() {
			var card = _currentCards.DrawTopCard();
			_cardDrawn?.Invoke(card);
			return card;
		}
		public List<RunnerCardData> AllCards => _startingCards;
		public List<RunnerCardData> AllCurrentCards => _currentCards;
		public int CurrentCount => AllCurrentCards.Count;
		public int TotalCount => AllCards.Count;
		public IEnumerable<RunnerCardData> Peek(int cardCount) => _currentCards.PeekCards(cardCount);
		public void AddToTop(RunnerCardData card) => _currentCards.AddToTop(card);
		public void AddToBottom(RunnerCardData card) => _currentCards.AddToBottom(card);
		public void AddToTop(List<RunnerCardData> cards) => _currentCards.AddToTop(cards);
		public void AddToBottom(List<RunnerCardData> cards) => _currentCards.AddToBottom(cards);
		public void UnlockCard(RunnerCardData card) => _startingCards.AddToBottom(card);
		public void UnlockCards(List<RunnerCardData> cards) => _startingCards.AddToBottom(cards);
		public void Shuffle()
		{
			_currentCards.Shuffle();
			_deckShuffled?.Invoke();
		}
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