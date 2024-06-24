using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public interface IDraggableCard 
    {
        public float BaseWidth { get; }
        public float CardThickness { get; }
        public void TweenScale(float scale, float duration);
        public void TweenPositionLocal(Vector3 position, float duration);
        public void TweenPosition(Vector3 position, float duration);
        public void TweenRotation(float rotation, float duration);
        public void ReturnToHand();
        public void Register(HandDisplayController controller);
    }
}