using LogicPuddle.CardManagement;
using LogicPuddle.Common;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runner
{
	public class RunnerTargetManager : MonoBehaviour
	{

		[SerializeField] private RunnerTargetDisplay _nodeDisplayTemplate;
		[SerializeField] private List<RunnerTargetConfiguration> nodeConfigurations = new List<RunnerTargetConfiguration>();

		[SerializeField] private GameObject _playCardNoTargetButton;
		[SerializeField] private RunnerTargetConfiguration _targetlessConfiguration;

		[SerializeField] private List<Transform> _nodeSlots = new List<Transform>();

		[SerializeField] private FirewallController _firewall;
		private List<RunnerTargetDisplay> _targets = new List<RunnerTargetDisplay>();
		private RunnerCardDisplayManager _cardManager;

		private RunnerTargetData _targetlessTarget;

		private void Start()
		{
			_playCardNoTargetButton.SetActive(false);
			_targetlessTarget = new RunnerTargetData(_targetlessConfiguration);
			var availableSlots = new List<Transform>();
			availableSlots.AddRange(_nodeSlots);
			availableSlots.Shuffle();
			_nodeDisplayTemplate.gameObject.SetActive(false);
			foreach (var target in nodeConfigurations)
			{
				if (availableSlots.Count == 0)
				{
					break;
				}
				var targetData = new RunnerTargetData(target);
				var slot = availableSlots.Pop();

				var display = Instantiate(_nodeDisplayTemplate, slot.transform);
				display.Setup(targetData, this);
				display.gameObject.SetActive(true);
				display.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
				_targets.Add(display);
			}
			_firewall.Setup(this);
			_targets.Add(_firewall.Block1);
			_targets.Add(_firewall.Block2);
		}

		public void Register(RunnerCardDisplayManager cardManager)
		{
			_cardManager = cardManager;
		}

		public void OnTargetSelected(RunnerTargetData selected)
		{
			_playCardNoTargetButton.SetActive(false);
			_cardManager.OnTargetSelected(selected);
			foreach (var target in _targets)
			{
				target.OnTargetSelected();
			}
		}
		public void OnPlayButton()
		{
			_cardManager.OnTargetSelected(_targetlessTarget);
		}

		public void OnCardDragged(RunnerCardData card)
		{
			foreach (var target in _targets)
			{
				target.OnCardDragged(card);
			}
		}

		public void OnCardSelected(RunnerCardData card)
		{
			if (card.CanPlay(_targetlessTarget))
			{
				_playCardNoTargetButton.SetActive(true);
			}
			foreach (var target in _targets)
			{
				target.OnCardSelected(card);
			}
		}

		public void OnCardDeselected()
		{
			_playCardNoTargetButton.SetActive(false);
			foreach (var target in _targets)
			{
				target.OnCardDeselected();
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
