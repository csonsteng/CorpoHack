using LogicPuddle.CardManagement;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LogicPuddle.Common;
using static UnityEngine.GraphicsBuffer;

namespace Runner.Deck
{
    public class RunnerDeckManager : Singleton<RunnerDeckManager>, ISerializable
	{
		// todo: need to store unlocked cards separately from deck
		public RunnerDeck Deck = new();
		public RunnerHand Hand = new();
		public RunnerTrash Trash = new();

		[SerializeField] private RunnerStartingDeck _startingDeck;
		[SerializeField] private int _handSize;

		private void Awake()
		{
			Deck.UnlockCards(_startingDeck.Cards);
			DontDestroyOnLoad(this);
		}

		public void StartGame()
		{
			Trash.Clear();
			Hand.Clear();
			Deck.Reset();
			for (var i = 0; i < _handSize; i++)
			{
				DrawCard();
			}
		}


		public bool DrawCard()
		{
			var card = Deck.DrawCard();
			if (card == null)
			{
				Debug.Log("no more cards in deck");
				return false;
			}
			Logger.Log($"Drew {card.Name}");
			Hand.Add(card);
			return true;
		}

		public void PlayCard(RunnerCardData card, RunnerTargetData target)
		{
			if (!Hand.Remove(card))
			{
				Debug.LogError($"cannot play {card.Name}. (Not in hand)");
				return;
			}
			Trash.Add(card);    // add to trash before play so GC can collect itself
			card.Play(target, this);

			Logger.Log($"Played {card.Name} on {target.TargetType}");
			AutofillHand();
		}

		public void Discard(RunnerCardData card, bool autoReplace = true)
		{
			if (!Hand.Remove(card))
			{
				Debug.LogError($"cannot discard {card.Name}. (Not in hand)");
				return;
			}
			Trash.Add(card);
			Logger.Log($"Discarded {card.Name}");
			if (autoReplace)
			{
				AutofillHand();
			}
		}

		public RunnerCardData RandomCard()
		{
			var currentHand = new List<RunnerCardData>();
			currentHand.AddRange(Hand.GetAll());
			currentHand.Shuffle();
			return currentHand[0];
		}

		public void AutofillHand()
		{
			while (Hand.GetAll().Count < _handSize && DrawCard())
			{
				// fill back up to handsize
			}
		}

		public void ShuffleTrashIntoDeck()
		{
			Deck.AddToBottom(Trash.GetAll());
			Trash.Clear();
			Deck.Shuffle();
			Logger.Log($"Shuffle trash into deck");
		}

		public void ShuffleHandIntoDeck()
		{
			Deck.AddToBottom(Hand.GetAll());
			Hand.Clear();
			Deck.Shuffle();
			Logger.Log($"Shuffle hand into deck");
		}

		public void ShuffleAll()
		{
			Deck.AddToBottom(Trash.GetAll());
			Trash.Clear();
			Deck.AddToBottom(Hand.GetAll());
			Hand.Clear();
			Deck.Shuffle();
		}

		public void Reset()
		{
			Deck.Reset();
			Trash.Clear();
			Hand.Clear();
		}

		public void UnlockCard(RunnerCardData card)
		{
			Deck.UnlockCard(card);
		}

		public Dictionary<string, object> Serialize()
		{
			return new Dictionary<string, object>
			{
				{ "hand", Hand.Serialize()},
				{ "deck", Deck.Serialize()},
				{ "trash", Trash.Serialize()},
			};
		}

		public void Deserialize(Dictionary<string, object> data)
		{
			Hand.Deserialize(data["hand"] as Dictionary<string, object>);
			Deck.Deserialize(data["deck"] as Dictionary<string, object>);
			Trash.Deserialize(data["trash"] as Dictionary<string, object>);
		}
	}
}