using Assets.InternalAssets.Scripts.UnityLogic.BehaviourInterface;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;

namespace UnityLogic.Mechanics
{
    public class GridSpawnLocations
    {
        IGridSpawnBeaviour gridSpawnBeaviour;

        public GridSpawnLocations(IGridSpawnBeaviour gridSpawnBeaviour)
        {
            this.gridSpawnBeaviour = gridSpawnBeaviour;
        }

        public Vector3?[,] GenerateSpawnLocationsFor(GridPattern pattern)
        {
            if (pattern == null)
            {
                return null;
            }

            var spawnLocations = new Vector3?[pattern.rowCount, pattern.colCount];
            var basePosition = gridSpawnBeaviour.SpawnPosition;

            for (int r = 0; r < pattern.rowCount; r++)
            {
                for (int c = 0; c < pattern.colCount; c++)
                {
                    if (pattern.rows[r].cells[c] == null)
                    {
                        spawnLocations[r, c] = null;
                        continue;
                    }

                    var colPos = basePosition.x + (gridSpawnBeaviour.ColOffset * c);
                    var rowPos = basePosition.y +
                        ((pattern.rowCount - r - 1) * gridSpawnBeaviour.RowOffset);

                    spawnLocations[r, c] = new Vector3(colPos, rowPos, 0);
                }
            }

            return spawnLocations;
        }

        //public List<Vector3> GenerateSpawnLocationsFor(GridPattern pattern)
        //{
        //    if (pattern == null)
        //    {
        //        return null;
        //    }

        //    var spawnLocations = new List<Vector3>();
        //    var basePosition = gridSpawnBeaviour.SpawnPosition;

        //    for (int r = 0; r < pattern.rowCount; r++)
        //    {
        //        for (int c = 0; c < pattern.colCount; c++)
        //        {
        //            if (pattern.rows[r].cells[c] == null)
        //            {
        //                continue;
        //            }

        //            var colPos = basePosition.x + (gridSpawnBeaviour.ColOffset * c);
        //            var rowPos = basePosition.y +
        //                (pattern.rowCount - gridSpawnBeaviour.RowOffset * r);

        //            spawnLocations.Add(new Vector3(colPos, rowPos, 0));
        //        }
        //    }

        //    return spawnLocations;
        //}
    }
}
