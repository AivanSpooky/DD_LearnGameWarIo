using System.Collections;
using LearnGame.Movement;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class DummyDirController : MonoBehaviour, IMovementDirSource
    {
        public Vector3 MoveDir { get; private set; }
        protected void Awake()
        {
            MoveDir = Vector3.zero;
        }
    }
}