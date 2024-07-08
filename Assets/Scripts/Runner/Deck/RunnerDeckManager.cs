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
    public class RunnerDeckManager : MonoBehaviour, ISerializable
	{
		public RunnerDeck Deck = new();
		public RunnerHand Hand = new();
		public RunnerTrash Trash = new();

		[SerializeField] private RunnerStartingDeck _startingDeck;
		[SerializeField] private int _handSize;

		private void Start()
		{
			Deck.UnlockCards(_startingDeck.Cards);
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
			Hand.Add(card);
			return true;
		}

		public void PlayCard(RunnerCardData card, RunnerTargetData target)
		{
			if (!Hand.Remove(card))
			{
				return;
			}
			Trash.Add(card);    // add to trash before play so GC can collect itself
			card.Play(target, this);
			AutofillHand();
		}

		public void Discard(RunnerCardData card)
		{
			if (!Hand.Remove(card))
			{
				return;
			}
			Trash.Add(card);
			AutofillHand();
		}

		private void AutofillHand()
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
		}

		public void ShuffleHandIntoDeck()
		{
			Deck.AddToBottom(Hand.GetAll());
			Hand.Clear();
			Deck.Shuffle();
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