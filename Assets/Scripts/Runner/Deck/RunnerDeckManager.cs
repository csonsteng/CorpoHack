using LogicPuddle.CardManagement;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LogicPuddle.Common;
using System;

namespace Runner.Deck
{
    public class RunnerDeckManager : Singleton<RunnerDeckManager>, ISerializable
	{
		public RunnerRig Rig = new();
		public SpareRigParts SpareParts = new();
		public RunnerDeck Deck = new();
		public RunnerHand Hand = new();
		public RunnerTrash Trash = new();
		public RunnerUnlockedCardsList UnlockedCards = new();

		[SerializeField] private RunnerRigDefaultConfiguration _defaultRig;

		private Action<RunnerCardData> _onForceDiscard;

		private void Awake()
		{
			Rig.Setup(_defaultRig);
			DontDestroyOnLoad(this);
		}

		public void Register(Action<RunnerCardData> onForceDiscard)
		{
			_onForceDiscard = onForceDiscard;
		}
		public void ForceDiscard(RunnerCardData card)
		{
			_onForceDiscard?.Invoke(card);
		}

		public void StartGame()
		{
			Trash.Clear();
			Hand.Clear();
			Deck.Reset();
			for (var i = 0; i < HandSize; i++)
			{
				DrawCard();
			}
		}

		public int HandSize => Rig.GetEffectiveHandSize(Deck);

		public void UnlockCard(RunnerCardData card) => UnlockedCards.UnlockCard(card);
		public void UnlockCards(IEnumerable<RunnerCardData> cards) => UnlockedCards.UnlockCards(cards);


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
			while (Hand.GetAll().Count < HandSize && DrawCard())
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