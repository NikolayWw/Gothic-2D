using System.Collections.Generic;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Scripts.UI
{
    public class TranslationAnimation : MonoBehaviour
    {
        public float duration = 0.5f;
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        public Vector3[] positions;

        public void MoveToOrigin()
        {
            movementCommands.Enqueue(origin);
        }

        public void MoveToPosition(int index)
        {
            if (index < positions.Length)
                movementCommands.Enqueue(positions[index]);
            else
                Debug.LogError("Position Index out of range.");
        }

        private void Awake()
        {
            origin = transform.position;
        }

        private void Update()
        {
            switch (state)
            {
                case State.Stopped:
                    StoppedState();
                    break;

                case State.Moving:
                    MovingState();
                    break;
            }
        }

        private void StoppedState()
        {
            if (movementCommands.Count > 0)
            {
                start = transform.position;
                target = movementCommands.Dequeue();
                state = State.Moving;
                t = 0;
            }
        }

        private void MovingState()
        {
            t += Time.deltaTime / duration;
            if (t >= 1)
            {
                t = 1;
                state = State.Stopped;
            }
            transform.position = Vector3.Lerp(start, target, curve.Evaluate(t));
        }

        private enum State
        {
            Stopped,
            Moving
        }

        private Vector3 origin, start, target;
        private Queue<Vector3> movementCommands = new Queue<Vector3>();
        private State state;
        private float t;
    }
}