using System;
using UnityEngine;
using UnityLogic.BehaviourInterface;

namespace Assets.Scripts.UnityLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Car/SpawnPattern", fileName = "SpawnPattern", order = 51)]
    public class GridPattern : ScriptableObject
    {
        [Serializable]
        public struct rowData
        {
            public ACarType[] cells; // since each cell will hold a prefab, it's GameObject
        }

        public rowData[] rows = new rowData[4];

        public int rowCount => rows == null ? 0 : rows.Length;
        public int colCount => (rows == null || rows.Length == 0) ? 0 : rows[0].cells.Length;

        public int NumberOfCellsWhichHaveCar => numberOfCellsWhichHaveCar == -1 ?
            GetNumberOfCellsWhichHaveCarInIt() : numberOfCellsWhichHaveCar;
        private int numberOfCellsWhichHaveCar = -1;

        public int GetNumberOfCellsWhichHaveCarInIt()
        {
            if (numberOfCellsWhichHaveCar != -1 || rows == null || rows.Length == 0)
            {
                return -1;
            }

            numberOfCellsWhichHaveCar = 0;
            for (int r = 0; r < rows.Length; r++)
            {
                if (rows[r].cells == null || rows[r].cells.Length == 0)
                {
                    return -1;
                }

                for (int c = 0; c < rows[r].cells.Length; c++)
                {
                    if (rows[r].cells[c] != null)
                    {
                        numberOfCellsWhichHaveCar++;
                    }
                }
            }

            return numberOfCellsWhichHaveCar;
        }
    }
}
