using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Rig/Cooling")]
public class RunnerRigCooling : RunnerRigComponent
{
	public int CardsOverForPenalty;

	public override string ComponentType => "Cooling";
	public override string ComponentDescription => "Cooling reduces the hand size penalty for exceeding the deck size threshold. " +
		"Your rig can only have one cooling component at a time.";
}
