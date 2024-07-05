using LogicPuddle.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public class AbstractCardManager<THand, TDeck, TTrash, TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTargetType>:
		MonoBehaviour, ICardManager, ISerializable
        where THand: AbstractHandData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTargetType>, new()
        where TDeck: AbstractDeckData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTargetType>, new()
        where TTrash : AbstractTrashData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTargetType>, new()
		where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTargetType>
		where TRarity : System.Enum
		where TEffect : AbstractCardEffect<TTargetData, TTargetConfiguration, TTargetType>
		where TTargetData : AbstractTargetData<TTargetConfiguration, TTargetType>
		where TTargetConfiguration : AbstractTargetConfiguration<TTargetType>
		where TTargetType : System.Enum
	{
		public TDeck Deck = new();
		public THand Hand = new();
		public TTrash Trash = new();

		[SerializeField] private AbstractBaseDeck<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTargetType> _startingDeck;
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
			if(card == null)
			{
				Debug.Log("no more cards in deck");
				return false;
			}
			Hand.Add(card);
			return true;
		}

		public void PlayCard(TCard card, TTargetData target)
		{
			if (!Hand.Remove(card))
			{
				return;
			}
			Trash.Add(card);	// add to trash before play so GC can collect itself
			card.Play(target, this);

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

		public void UnlockCard(TCard card)
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