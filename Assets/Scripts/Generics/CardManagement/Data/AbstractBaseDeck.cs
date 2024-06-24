using LogicPuddle.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
	public abstract class AbstractBaseDeck<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> : UniqueScriptableObject, IDeckConfiguration
		where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TRarity : System.Enum
		where TEffect : AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget>
		where TTargetData : AbstractTargetData<TTargetConfiguration, TTarget>
		where TTargetConfiguration : AbstractTargetConfiguration<TTarget>
		where TTarget : System.Enum
	{
		public string Name;
		public List<TCard> Cards;

		public abstract Sprite Backing { get; }
	}
}