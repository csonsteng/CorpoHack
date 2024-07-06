using LogicPuddle.CardManagement;
using LogicPuddle.Common;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
	public class RunnerTargetManager : MonoBehaviour
	{

		[SerializeField] private RunnerTargetDisplay _displayTemplate;
		[SerializeField] private List<RunnerTargetConfiguration> _targetConfigurations = new List<RunnerTargetConfiguration>();


		[SerializeField] private List<Transform> _targetSlots = new List<Transform>();

		private List<RunnerTargetDisplay> _targets = new List<RunnerTargetDisplay>();


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
				var targetData = new RunnerTargetData();
				targetData.Setup(target);
				var slot = availableSlots.Pop();

				var display = Instantiate(_displayTemplate, _displayTemplate.transform.parent);
				display.Setup(targetData, slot.position);
				display.gameObject.SetActive(true);
				_targets.Add(display);
			}
		}


		public void OnCardDragged(RunnerCardData card)
		{
			foreach (var target in _targets)
			{
				target.OnCardDragged(card);
			}
		}

		/// <summary>
		/// Returns true if valid target
		/// </summary>
		public bool OnCardDropped(out RunnerTargetData targetData)
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
