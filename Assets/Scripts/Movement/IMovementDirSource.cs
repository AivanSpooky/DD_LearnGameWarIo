using UnityEngine;


namespace LearnGame.Movement
{
    public interface IMovementDirSource
    {
        Vector3 MoveDir { get; }
    }
}
