using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Rig/NIC")]
public class RunnerRigNetworkCard : AbstractRunnerRigInterfaceComponent
{
    public int IncreasedDetectionThreshold;
	public override string ComponentType => "Network Card";
	public override string ComponentDescription => "A network card raises the detection threshold before you get ejected from a system. It takes up an interface slot.";
}
