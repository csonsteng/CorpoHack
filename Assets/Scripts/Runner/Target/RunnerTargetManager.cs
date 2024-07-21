using LogicPuddle.CardManagement;
using LogicPuddle.Common;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace Runner
{
	public class RunnerTargetManager : MonoBehaviour
	{
		public static RunnerTargetManager Instance => GetInstance();

		private static RunnerTargetManager GetInstance()
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<RunnerTargetManager>();
			}
			return _instance;
		}
		private static RunnerTargetManager _instance;



		[SerializeField] private RunnerTargetDisplay _nodeDisplayTemplate;
		[SerializeField] private List<RunnerTargetConfiguration> _nodeConfigurations = new List<RunnerTargetConfiguration>();


		[SerializeField] private IceDisplay _iceDisplayTemplate;
		[SerializeField] private IceList _iceList;
		[SerializeField] private List<RunnerTargetConfiguration> _iceConfigurations = new List<RunnerTargetConfiguration>();


		[SerializeField] private GameObject _playCardNoTargetButton;
		[SerializeField] private RunnerTargetConfiguration _targetlessConfiguration;

		[SerializeField] private List<Transform> _nodeSlots = new List<Transform>();
		[SerializeField] private List<Transform> _iceSlots = new List<Transform>();

		[SerializeField] private FirewallController _firewall;
		private List<RunnerTargetDisplay> _targets = new List<RunnerTargetDisplay>();
		private List<IceData> _iceData = new List<IceData>();
		private RunnerCardDisplayManager _cardManager;

		private RunnerTargetData _targetlessTarget;

		private void Start()
		{
			_playCardNoTargetButton.SetActive(false);
			DetectionTracker.Instance.Register(DetectionLevelChanged);
			_targetlessTarget = new RunnerTargetData(_targetlessConfiguration);
			SetupNodes();
			SetupICE();
			_firewall.Setup(this);
		}

		private void SetupNodes()
		{
			var availableSlots = new List<Transform>();
			availableSlots.AddRange(_nodeSlots);
			availableSlots.Shuffle();
			_nodeDisplayTemplate.gameObject.SetActive(false);
			foreach (var target in _nodeConfigurations)
			{
				if (availableSlots.Count == 0)
				{
					break;
				}
				var targetData = new RunnerTargetData(target);
				RegisterWithTargetData(targetData);
				var slot = availableSlots.Pop();

				var display = Instantiate(_nodeDisplayTemplate, slot.transform);
				display.Setup(targetData, this);
				display.gameObject.SetActive(true);
				display.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
				_targets.Add(display);
			}
		}
		private void SetupICE()
		{
			var availableSlots = new List<Transform>();
			availableSlots.AddRange(_iceSlots);
			availableSlots.Shuffle();
			_iceDisplayTemplate.gameObject.SetActive(false);

			var iceOptions = _iceList.GetAll();
			foreach (var target in _iceConfigurations)
			{
				if (availableSlots.Count == 0)
				{
					break;
				}
				iceOptions.Shuffle();
				var iceType = iceOptions[0];
				var targetData = new IceData(target, iceType);
				RegisterWithTargetData(targetData);
				_iceData.Add(targetData);
				var slot = availableSlots.Pop();

				var display = Instantiate(_iceDisplayTemplate, slot.transform);
				display.Setup(targetData, this);
				display.gameObject.SetActive(true);
				display.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
				_targets.Add(display);
			}
		}

		private void RegisterWithTargetData(RunnerTargetData targetData)
		{
			targetData.RegisterListener(TargetStrengthChanged);
		}

		private void TargetStrengthChanged(RunnerTargetData target)
		{
			if(target.Strength > 0)
			{
				return;
			}
			if(target is IceData iceData)
			{
				foreach (var ice in _iceData)
				{
					ice.IceBroken(iceData);
				}
				return;
			}

			foreach (var ice in _iceData)
			{
				ice.NodeBroken();
			}
			return;
		}

		private void DetectionLevelChanged(int level)
		{
			if (level == 15)
			{
				foreach (var ice in _iceData)
				{
					ice.DetectionAt15();
				}
			}
		}

		public void PingTarget(RunnerTargetData target)
		{
			target.Ping();
			if (target is IceData iceData)
			{
				foreach (var ice in _iceData)
				{
					ice.IcePinged(iceData);
				}
				return;
			}

			foreach (var ice in _iceData)
			{
				ice.NodePinged();
			}
		}

		public IEnumerable<RunnerTargetData> GetAllData()
		{
			foreach(var display in _targets)
			{
				yield return display.Data;
			}
		}

		public void Register(RunnerCardDisplayManager cardManager)
		{
			_cardManager = cardManager;
		}

		public void ReEnableFirewall()
		{
			_firewall.Block1.Data.SetStrength(_firewall.Block1.Data.OriginalStrength);
			_firewall.Block2.Data.SetStrength(_firewall.Block2.Data.OriginalStrength);
		}

		public void OnTargetSelected(RunnerTargetData selected)
		{
			_playCardNoTargetButton.SetActive(false);
			_cardManager.OnTargetSelected(selected);

			_firewall.Block1.OnTargetSelected();
			_firewall.Block2.OnTargetSelected();
			if (_firewall.IsActive)
			{
				return;
			}
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

			if (_firewall.IsActive)
			{
				_firewall.Block1.OnCardSelected(card);
				_firewall.Block2.OnCardSelected(card);
				return;
			}
			foreach (var target in _targets)
			{
				target.OnCardSelected(card);
			}
		}

		public void OnCardDeselected()
		{
			_playCardNoTargetButton.SetActive(false);

			if (_firewall.IsActive)
			{
				_firewall.Block1.OnCardDeselected();
				_firewall.Block2.OnCardDeselected();
				return;
			}
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
