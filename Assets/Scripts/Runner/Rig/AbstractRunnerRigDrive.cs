using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRunnerRigDrive : RunnerRigComponent
{
	public abstract bool IsBackup { get; }
	public int DeckSizeContribution;
}
