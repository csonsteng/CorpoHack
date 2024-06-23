using LogicPuddle.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
	public abstract class AbstractTargetData<TConfiguration, TTarget> : ISerializable where TConfiguration: AbstractTargetConfiguration<TTarget> where TTarget : System.Enum
	{
		protected TConfiguration _configuration;
		public void Deserialize(Dictionary<string, object> data)
		{
			UniqueScriptableObjectLinker.TryGetUniqueObject(System.Convert.ToString(data["key"]), out _configuration);
			FinishDeserialization(data);
		}

		public Dictionary<string, object> Serialize()
		{
			var baseDictionary = new Dictionary<string, object>
			{
				{ "key", _configuration.UniqueID }
			};

			return FinishSerialization(baseDictionary);
		}

		protected abstract void FinishDeserialization(Dictionary<string, object> data);

		protected abstract Dictionary<string, object> FinishSerialization(Dictionary<string, object> data);
	}
}