using UnityEngine;

internal interface IRoadGenerator
{
    IObjectPool<IRoadSection> RoadSectionPool { get; }
    int StartingNumberOfSections { get; }
    FloatValue LevelSpeed { get; }

    IRoadSection[] InitializeSections(Vector2 initialSpawnPosition, int startingNumberOfSections);
    IRoadSection[] SpawnNewSectionIfAny();
    void MoveActiveSection(IRoadSection roadSection);
    void EnqueueForRecycle(IRoadSection roadSection);
}