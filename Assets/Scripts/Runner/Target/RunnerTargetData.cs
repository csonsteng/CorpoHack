using LogicPuddle.CardManagement;
using LogicPuddle.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;



namespace Runner.Target
{
	public class RunnerTargetData : ISerializable
	{
		public RunnerTargetType TargetType => _configuration.TargetType;
		protected RunnerTargetConfiguration _configuration;
		public RunnerTargetColor Color { get; private set; }

		public int OriginalStrength { get; private set; }
		public int Strength { get; private set; }

		private Action StrengthChanged;

		public RunnerTargetData() { }

		public RunnerTargetData(RunnerTargetConfiguration configuration)
		{
			_configuration = configuration;
			if(TargetType == RunnerTargetType.None)
			{
				return;
			}
			// todo: build these out based on target type, and difficulty
			Color = (RunnerTargetColor)Random.Range(1, System.Enum.GetValues(typeof(RunnerTargetColor)).Length);
			OriginalStrength = Random.Range(3, 5);
			Strength = OriginalStrength;
		}

		public void RegisterListener(Action onStrengthChange)
		{
			StrengthChanged += onStrengthChange;
		}

		public bool Damage(int amount = 1)
		{
			Strength = Mathf.Max(Strength - amount, 0);
			StrengthChanged?.Invoke();
			return Strength == 0;
		}

		public void Deserialize(Dictionary<string, object> data)
		{

			UniqueScriptableObjectLinker.TryGetUniqueObject(System.Convert.ToString(data["key"]), out _configuration);
			Color = (RunnerTargetColor)System.Convert.ToInt32(data["color"]);
			Strength = System.Convert.ToInt32(data["strength"]);
			OriginalStrength = System.Convert.ToInt32(data["original-strength"]);
		}

		public Dictionary<string, object> Serialize()
		{
			var data = new Dictionary<string, object>
			{
				{ "key", _configuration.UniqueID },
				{ "color", (int)Color },
				{ "strength", Strength },
				{ "original-strength", OriginalStrength }
			};
			return data;
		}
	}
}