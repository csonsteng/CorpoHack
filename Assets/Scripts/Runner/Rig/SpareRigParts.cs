using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpareRigParts
{
	private readonly List<RunnerRigMotherboard> _motherboards = new List<RunnerRigMotherboard>();
	private readonly List<RunnerRigProcessor> _processors = new List<RunnerRigProcessor>();
	private readonly List<RunnerRigCooling> _cooling = new List<RunnerRigCooling>();
	private readonly List<RunnerRigRAM> _ram = new List<RunnerRigRAM>();
	private readonly List<AbstractRunnerRigDrive> _drives = new List<AbstractRunnerRigDrive>();
	private readonly List<AbstractRunnerRigInterfaceComponent> _interfaces = new List<AbstractRunnerRigInterfaceComponent>();

	public IEnumerable<T> GetAllSpareParts<T>() where T: RunnerRigComponent
	{
		if (typeof(T) == typeof(RunnerRigMotherboard))
		{
			foreach (var motherboard in _motherboards)
			{
				yield return motherboard as T;
			}
		} else if (typeof(T) == typeof(RunnerRigProcessor))
		{

			foreach (var processor in _processors)
			{
				yield return processor as T;
			}
		} else if (typeof(T) == typeof(RunnerRigCooling))
		{

			foreach (var coolingUnit in _cooling)
			{
				yield return coolingUnit as T;
			}
		} else if (typeof(T) == typeof(RunnerRigRAM))
		{

			foreach (var ram in _ram)
			{
				yield return ram as T;
			}
		} else if (typeof(T) == typeof(AbstractRunnerRigDrive))
		{

			foreach (var drive in _drives)
			{
				yield return drive as T;
			}
		} else if (typeof(T) == typeof(AbstractRunnerRigInterfaceComponent))
		{

			foreach (var interfaceComponent in _interfaces)
			{
				yield return interfaceComponent as T;
			}
		}
	}
}
