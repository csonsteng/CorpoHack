using LogicPuddle.CardManagement;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
	public class RunnerTargetDisplay : AbstractTargetDisplay<RunnerCardData, RunnerCardRarity, AbstractRunnerCardEffect, RunnerTargetData, RunnerTargetConfiguration, RunnerTargetType>
	{
		[SerializeField] RunnerTargetIndicator _strengthIndicator;

		public override void Setup(RunnerTargetData data, Vector3 worldPosition)
		{
			base.Setup(data, worldPosition);
			_strengthIndicator.Setup(data);
		}


	}
}
