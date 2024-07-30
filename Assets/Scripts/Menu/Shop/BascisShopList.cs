using Runner.Deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Shop/Basic Inventory")]
public class BascisShopList : ScriptableObject
{
    [SerializeField] private List<RunnerRigComponent> _rigComponents;
    [SerializeField] private List<RunnerCardData> _programs;

    public IEnumerable<RunnerRigComponent> Components() => _rigComponents;
    public IEnumerable<RunnerCardData> Programs() => _programs;
}
