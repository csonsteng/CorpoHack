using LogicPuddle.CardManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



namespace Runner.Target
{
	public class RunnerTargetData : AbstractTargetData<RunnerTargetConfiguration, RunnerTargetType>
	{
		public RunnerTargetColor Color { get; private set; }

		public int OriginalStrength { get; private set; }
		public int Strength { get; private set; }

		private Action StrengthChanged;

		public override void Setup(RunnerTargetConfiguration configuration)
		{
			base.Setup(configuration);
			// todo: build these out based on target type, and difficulty
			Color = (RunnerTargetColor)Random.Range(0, System.Enum.GetValues(typeof(RunnerTargetColor)).Length);
			OriginalStrength = Random.Range(2, 6);
			Strength = OriginalStrength;
		}

		public void RegisterListener(Action onStrengthChange)
		{
			StrengthChanged += onStrengthChange;
		}

		public bool Damage(int amount)
		{
			Strength = Mathf.Max(Strength - amount, 0);
			StrengthChanged?.Invoke();
			return Strength == 0;
		}

		protected override void FinishDeserialization(Dictionary<string, object> data)
		{
			Color = (RunnerTargetColor)System.Convert.ToInt32(data["color"]);
			Strength = System.Convert.ToInt32(data["strength"]);
			OriginalStrength = System.Convert.ToInt32(data["original-strength"]);
		}

		protected override Dictionary<string, object> FinishSerialization(Dictionary<string, object> data)
		{
			data["color"] = (int)Color;
			data["strength"] = Strength;
			data["original-strength"] = OriginalStrength;
			return data;
		}
	}
}