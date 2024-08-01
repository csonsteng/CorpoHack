using LogicPuddle.Common;
using Runner.Deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RunnerRigComponent : UniqueScriptableObject, IShopItem
{
	public string Name;
	public int Cost;
	[TextArea] public string Description;

	public abstract string ComponentType { get; }
	public abstract string ComponentDescription { get; }

	public abstract string Effect();

	public string ItemName() => Name;

	public string Type() => ComponentType;

	public int GetCost() => Cost;

	public void Buy()
	{
		RunnerDeckManager.Instance.SpareParts.AddSparePart(this);
	}
}
