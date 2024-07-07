using LogicPuddle.CardManagement;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Deck.Effects
{
	public abstract class AbstractRunnerCardEffect : ScriptableObject
	{

		public abstract bool IsValidTarget(RunnerTargetData target);

		public abstract void Activate(RunnerTargetData target, RunnerDeckManager manager);
	}
}