using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Rig/GPU")]
public class RunnerRigGPU : AbstractRunnerRigInterfaceComponent
{
    public float MineChance;


	public override string ComponentType => "GPU";
	public override string ComponentDescription => "GPUs have a chance to mine CryptoCoins on every action taken. They take up an interface slot.";

	public override string Effect() => $"[Not Implemented] Has a {100f * MineChance:0.00}% chance to earn 1 crypto every tick.";

}
