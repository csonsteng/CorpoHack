using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Rig/Processor")]
public class RunnerRigProcessor : RunnerRigComponent
{
	public int DetectReductionThreshold;
	public int Tier;

	public override string ComponentType => "Processor";
	public override string ComponentDescription => "Your rig requires exactly one processor. " +
		"Faster processors can reduce the rate at which detection increases. Your motherboard must support the correct tier of processor.";

	public override string Effect()
	{
		var suffix = "th";
		switch (DetectReductionThreshold)
		{
			case 1:
				suffix = "st";
				break;
			case 2:
				suffix = "nd";
				break;
			case 3:
				suffix = "rd";
				break;
		}
		return $"[Not Implemented] Every {DetectReductionThreshold}{suffix} program played doesn't cost detect. Requires a tier {Tier} motherboard.";
	}
}
