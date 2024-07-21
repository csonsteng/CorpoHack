using LogicPuddle.CardManagement;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
	public class RunnerTargetDisplay : MonoBehaviour
	{
		[SerializeField] RunnerTargetIndicator _strengthIndicator;
		[SerializeField] private GameObject _validTargetIndicator;
		[SerializeField] private GameObject _targetedIndicator;

		private RunnerTargetData _data;

		public RunnerTargetData Data => _data;
		private RunnerTargetManager _manager;

		private bool _isTargeted;
		private bool _hovered;
		private void Start()
		{
			_validTargetIndicator.SetActive(false);
			_targetedIndicator.SetActive(false);
		}

		public void Setup(RunnerTargetData data, RunnerTargetManager manager)
		{
			_manager = manager;
			_data = data;
			data.RegisterListener(OnStrengthChange);
			_strengthIndicator.Setup(data);
		}

		private void OnStrengthChange(RunnerTargetData data)
		{
			if (data.IsBroken)
			{
				OnSecurityBroken();
			}
		}

		protected virtual void OnSecurityBroken()
		{

		}

		public void OnCardDragged(RunnerCardData card)
		{
			if (!card.CanPlay(_data))
			{
				return;
			}
			_isTargeted = true;
			_validTargetIndicator.SetActive(true);
		}

		public void OnCardSelected(RunnerCardData card)
		{
			if (!card.CanPlay(_data))
			{
				return;
			}
			_isTargeted = true;
			_validTargetIndicator.SetActive(true);
		}
		public void OnCardDeselected()
		{
			_isTargeted = false;
			_validTargetIndicator.SetActive(false);
		}

		private void OnMouseUpAsButton()
		{
			if (!_isTargeted)
			{
				return;
			}
			_manager.OnTargetSelected(_data);
		}
		public void OnTargetSelected()
		{
			_isTargeted = false;
			MouseExit();
			_validTargetIndicator.SetActive(false);
		}

		public bool OnCardDropped()
		{
			_isTargeted = false;
			var wasHovered = _hovered;
			MouseExit();
			_validTargetIndicator.SetActive(false);
			return wasHovered;
		}

		private void Update()
		{
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
			{
				MouseExit();
				return;
			}

			if (hit.collider.gameObject == gameObject)
			{
				MouseEnter();
				return;

			}
			MouseExit();
		}

		private void MouseEnter()
		{
			TargetTooltip.Instance.Show(_data);
			if (!_isTargeted)
			{
				return;
			}
			_targetedIndicator.SetActive(true);
			_hovered = true;
		}
		private void MouseExit()
		{
			TargetTooltip.Instance.Hide(_data);
			_targetedIndicator.SetActive(false);
			_hovered = false;
		}


	}
}
