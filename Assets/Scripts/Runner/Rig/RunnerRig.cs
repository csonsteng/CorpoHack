using Runner.Deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerRig
{
	private RunnerRigMotherboard _motherboard;
	private RunnerRigProcessor _processor;
	private RunnerRigCooling _cooling;
	private readonly List<RunnerRigRAM> _ram = new List<RunnerRigRAM>();
	private readonly List<AbstractRunnerRigDrive> _drives = new List<AbstractRunnerRigDrive>();
	private readonly List<AbstractRunnerRigInterfaceComponent> _interfaces = new List<AbstractRunnerRigInterfaceComponent>();

	public void Setup(RunnerRigDefaultConfiguration configuration)
    {
        _motherboard = configuration.Motherboard;
        _processor = configuration.Processor;
        _cooling = configuration.Cooling;
		_drives.Add(configuration.BackupDrive);
        _drives.AddRange(configuration.Drives);
        _ram.AddRange(configuration.RAM);
    }

    public int GetMaxHandSize()
    {
        var handSize = 0;
        foreach (var ram in _ram)
        {
            handSize += ram.HandSizeContribution;
        }
        return handSize;
    }

    public int GetEffectiveHandSize(RunnerDeck deck)
    {
        float delta = deck.TotalCount - GetMaxDeckSize();
        var baseHandSize = GetMaxHandSize();

		if (delta <= 0)
        {
            return baseHandSize;
        }
        var penalty = Mathf.CeilToInt(delta / _cooling.CardsOverForPenalty);
        return baseHandSize - penalty;

	}

    public int GetMaxDeckSize()
    {
        var deckSize = 0;
        foreach (var drive  in _drives)
        {
            deckSize += drive.DeckSizeContribution;
        }
        return deckSize;
    }
}
