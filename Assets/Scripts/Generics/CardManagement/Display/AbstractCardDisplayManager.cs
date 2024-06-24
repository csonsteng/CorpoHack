using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public class AbstractCardDisplayManager<THand, TCard, TCardDisplay, THandDisplay, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> : MonoBehaviour
		where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where THand : AbstractHandData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where THandDisplay : AbstractHandDisplay<THand, TCard, TCardDisplay, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TCardDisplay : AbstractCardDisplay<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TRarity : Enum
		where TEffect : AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget>
		where TTargetData : AbstractTargetData<TTargetConfiguration, TTarget>
		where TTargetConfiguration : AbstractTargetConfiguration<TTarget>
		where TTarget : Enum
    {
		[SerializeField] private THandDisplay _handDisplay;

		private THand _handData;


		private void Start()
		{
			_handDisplay.Setup(_handData, OnCardDragged, OnCardDropped);
		}

		private void OnCardDragged(TCard card)
		{

		}

		private void OnCardDropped(TCard card)
		{

		}
	}
}
