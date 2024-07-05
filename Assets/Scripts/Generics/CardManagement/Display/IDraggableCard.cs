using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public interface IDraggableCard 
    {
        public float BaseWidth { get; }
        public float CardThickness { get; }
        public void Scale(float scale, float duration);
        public void MovePositionLocal(Vector3 position, float duration);
        public void MovePosition(Vector3 position, float duration);
        public void TurnFaceDown(float duration);
        public void TurnFaceUp(float duration);
		public void RotateInPlane(float rotation, float duration);
        public void ReturnToHand();
        public void Register(HandDisplayController controller);
    }
}