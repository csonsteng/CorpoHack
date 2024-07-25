using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/Rig/Configuration")]
public class RunnerRigDefaultConfiguration : ScriptableObject
{
    public RunnerRigMotherboard Motherboard;
    public RunnerRigProcessor Processor;
    public RunnerRigCooling Cooling;
    public RunnerRigBackup BackupDrive;
    public List<RunnerRigRAM> RAM;
    public List<RunnerRigDrive> Drives;
}
