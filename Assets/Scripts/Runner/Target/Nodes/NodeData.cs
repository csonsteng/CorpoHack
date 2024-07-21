using LogicPuddle.Common;
using Runner.Target;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Target
{
	public class NodeData : RunnerTargetData
	{
		public enum NodeStatus
		{
			Secured,
			Broken,
			Siphoned,
			Downloaded,
			Corrupted
		}

		private AbstractNodeType _nodeType;
		public NodeStatus Status { get; private set; }

		public NodeData(RunnerTargetConfiguration configuration, AbstractNodeType nodeType) : base(configuration)
		{
			_nodeType = nodeType;
			Status = NodeStatus.Secured;
			RegisterListener(OnStrengthUpdated);
			Debug.Log($"setup {_nodeType.Name}");
		}

		private void OnStrengthUpdated(RunnerTargetData data)
		{
			if(Status !=  NodeStatus.Secured)
			{
				return;
			}
			if(Strength > 0)
			{
				return;
			}
			Status = NodeStatus.Broken;
			Pinged = true;
			_nodeType.WhenBroken();
		}

		public bool CanBeSiphoned => _nodeType.CanBeSiphoned;
		public bool CanBeDownloaded => _nodeType.CanBeDownloaded;
		public bool CanBeCorrupted => _nodeType.CanBeCorrupted;

		public void Siphon()
		{
			Status = NodeStatus.Siphoned;
			_nodeType.Siphon();
		}

		public void Download()
		{
			Status = NodeStatus.Downloaded;
			_nodeType.Download();
		}

		public void Corrupt()
		{
			Status = NodeStatus.Corrupted;
			_nodeType.Corrupt();
		}


		public string Title()
		{
			if (!Pinged)
			{
				return "Node: ???";
			}
			return _nodeType.Name;
		}

		public string Description()
		{
			if (!Pinged)
			{
				return "A node typical stores data. This could be incrimnating data, user data, a crypto wallet, or mundane data.";
			}
			return _nodeType.Description;
		}

		public override void Deserialize(Dictionary<string, object> data)
		{
			base.Deserialize(data);
			UniqueScriptableObjectLinker.TryGetUniqueObject<AbstractNodeType>(data["node-type"].ToString(), out _nodeType);
			Status = (NodeStatus)Convert.ToInt32(data["status"]);
		}

		public override Dictionary<string, object> Serialize()
		{
			var data = base.Serialize();
			data["node-type"] = _nodeType.UniqueID;
			data["status"] = (int)Status;
			return data;
		}
	}
}
