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


		public void DrawCard()
		{
			var card = Deck.DrawCard();
			Hand.Add(card);
		}

		public void PlayCard(TCard card, TTargetData target)
		{
			// todo: need animation timing?
			if (!card.CanPlay(target.TargetType))
			{
				return;
			}
			if (!Hand.Remove(card))
			{
				return;
			}
			card.Play(target, this);
			Trash.Add(card);
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