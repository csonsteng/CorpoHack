using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public class AbstractTargetDisplay<THand, TCard, TCardDisplay, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> : MonoBehaviour
        where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
        where THand : AbstractHandData<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
        where TCardDisplay : AbstractCardDisplay<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
        where TRarity : Enum
        where TEffect : AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget>
        where TTargetData : AbstractTargetData<TTargetConfiguration, TTarget>
        where TTargetConfiguration : AbstractTargetConfiguration<TTarget>
        where TTarget : Enum
    {
		[SerializeField] private GameObject _validTargetIndicator;
		[SerializeField] private GameObject _targetedIndicator;

		[SerializeField] private TTargetData _data;

		private bool _hasCard;
		private bool _hovered;

		private void Start()
		{
			_validTargetIndicator.SetActive(false);
			_targetedIndicator.SetActive(false);
		}

		public void Setup(TTargetData data)
		{
			_data = data;
		}

		public void OnCardDragged(TCard card)
		{
			if (!card.CanPlay(_data.TargetType))
			{
				return;
			}
			_hasCard = true;
			_validTargetIndicator.SetActive(true);
		}

		public bool OnCardDropped()
		{
			_hasCard = false;
			var wasHovered = _hovered;
			MouseExit();
			_validTargetIndicator.SetActive(false);
			return wasHovered;
		}

		private void Update()
		{
			if (!_hasCard)
			{
				return;
			}

			var hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
			if (hits == null)
			{
				return;
			}
			foreach (var hit in hits)
			{
				if (hit.collider.gameObject != gameObject)
				{
					continue;
				}
				MouseEnter();
				return;
			}
			MouseExit();
		}

		private void MouseEnter()
		{
			_targetedIndicator.SetActive(true);
			_hovered = true;
		}
		private void MouseExit()
		{
			_targetedIndicator.SetActive(false);
			_hovered = false;
		}
	}
}