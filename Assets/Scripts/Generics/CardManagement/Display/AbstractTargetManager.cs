using LogicPuddle.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public abstract class AbstractTargetManager<TTargetDisplay, TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> : MonoBehaviour
		where TTargetDisplay: AbstractTargetDisplay<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TRarity : Enum
		where TEffect : AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget>
		where TTargetData : AbstractTargetData<TTargetConfiguration, TTarget>, new()
		where TTargetConfiguration : AbstractTargetConfiguration<TTarget>
		where TTarget : Enum
	{

		[SerializeField] private TTargetDisplay _displayTemplate;
		[SerializeField] private List<TTargetConfiguration> _targetConfigurations = new List<TTargetConfiguration>();


		[SerializeField] private List<Transform> _targetSlots = new List<Transform>();

		private List<TTargetDisplay> _targets = new List<TTargetDisplay>();


		private void Start()
		{
			var availableSlots = new List<Transform>();
			availableSlots.AddRange(_targetSlots);
			availableSlots.Shuffle();
			_displayTemplate.gameObject.SetActive(false);
			foreach (var target in _targetConfigurations)
			{
				if (availableSlots.Count == 0)
				{
					break;
				}
				var targetData = new TTargetData();
				targetData.Setup(target);
				var slot = availableSlots.Pop();

				var display = Instantiate(_displayTemplate, _displayTemplate.transform.parent);
				display.Setup(targetData, slot.position);
				display.gameObject.SetActive(true);
				_targets.Add(display);
			}
		}


		public void OnCardDragged(TCard card)
		{
			foreach (var target in _targets)
			{
				target.OnCardDragged(card);
			}
		}

		/// <summary>
		/// Returns true if valid target
		/// </summary>
		public bool OnCardDropped(out TTargetData targetData)
		{
			targetData = null;
			// this approach will definitely bug out if targets are overlapped
			foreach (var target in _targets)
			{
				if (target.OnCardDropped())
				{
					targetData = target.Data;
					return true;
				}
			}
			return false;
		}

	}
}