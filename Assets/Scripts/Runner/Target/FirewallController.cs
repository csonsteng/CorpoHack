using LogicPuddle.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{
    public class FirewallController : MonoBehaviour
    {

		[SerializeField] private RunnerTargetConfiguration _firewallConfiguration;
		[SerializeField] private RunnerTargetDisplay _block1;
		[SerializeField] private RunnerTargetDisplay _block2;

		[SerializeField] private ParticleSystem _firewallEffect;

		public RunnerTargetDisplay Block1 => _block1;
		public RunnerTargetDisplay Block2 => _block2;
		public void Setup(RunnerTargetManager manager)
		{
			_firewallEffect.gameObject.SetActive(true);
			SetupBlockData(_block1, manager);
			SetupBlockData(_block2, manager);
		}

		private void SetupBlockData(RunnerTargetDisplay display, RunnerTargetManager manager)
		{
			var data = new RunnerTargetData(_firewallConfiguration);
			display.Setup(data, manager);
			data.RegisterListener(OnStrengthUpdated);
		}

		private void OnStrengthUpdated()
		{
			if(_block1.Data.Strength <= 0 && _block2.Data.Strength <= 0)
			{
				_firewallEffect.Stop();
			}
		}
	}
}