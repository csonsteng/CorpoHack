using LogicPuddle.Common;
using Runner.Target;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceData : RunnerTargetData
{
	private AbstractIceType _effect;
	private IceTrigger _trigger;


	public IceData(RunnerTargetConfiguration configuration, AbstractIceType iceType) : base(configuration)
	{
		_effect = iceType;
		_trigger = _effect.GetRandomValidTrigger();
		Debug.Log($"setup {_effect.Name} with {_trigger} trigger");

	}

	public void IcePinged(IceData iceData)
	{
		if (iceData == this)
		{
			if (_trigger == IceTrigger.SelfPinged)
			{
				_effect.OnTrigger();
			}
			return;
		}
		if (_trigger == IceTrigger.OtherPinged)
		{
			_effect.OnTrigger();
		}
	}
	public void IceBroken(IceData iceData)
	{
		if (iceData == this)
		{
			if (_trigger == IceTrigger.SelfBroken)
			{
				_effect.OnTrigger();
			}
			return;
		}
		if (_trigger == IceTrigger.OtherBroken)
		{
			_effect.OnTrigger();
		}
	}
	public void NodePinged()
	{
		if (_trigger == IceTrigger.NodePinged)
		{
			_effect.OnTrigger();
		}
	}
	public void NodeBroken()
	{
		if (_trigger == IceTrigger.NodeBroken)
		{
			_effect.OnTrigger();
		}
	}
	public void DetectionAt15()
	{
		if (_trigger == IceTrigger.DetectionAt15)
		{
			_effect.OnTrigger();
		}
	}


	public override void Deserialize(Dictionary<string, object> data)
	{
		base.Deserialize(data);
		UniqueScriptableObjectLinker.TryGetUniqueObject<AbstractIceType>(data["effect"].ToString(), out _effect);
		_trigger = (IceTrigger)Convert.ToInt32(data["trigger"]);
	}

	public override Dictionary<string, object> Serialize()
	{
		var data = base.Serialize();
		data["effect"] = _effect.UniqueID;
		data["trigger"] = (int)_trigger;
		return data;
	}
}
