using System;
using Assets.InternalAssets.Scripts.UnityLogic.BehaviourInterface;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityLogic.BehaviourInterface;
using UnityLogic.Mechanics;
using URandom = UnityEngine.Random;

namespace MechanicsTest
{
    public class GridSpawnTests
    {
        public class Generated_locations_array
        {
            [Test]
            public void Should_have_same_size_as_of_number_of_true_cells()
            {
                var gridBehaviourMock = Substitute.For<IGridSpawnBeaviour>();
                GridPattern pattern = Build_a.GridPattern
                                    .With_a_row_count_of(3)
                                    .Has_pattern(0, 0, 1,
                                                 1, 0, 0,
                                                 1, 1, 0);

                var gridSpawner = new GridSpawnLocations(gridBehaviourMock);
                var spawnLocations = gridSpawner.GenerateSpawnLocationsFor(pattern);

                var nonNullPositions = 0;
                for (int r = 0; r < spawnLocations.GetLength(0); r++)
                {
                    for (int c = 0; c < spawnLocations.GetLength(1); c++)
                    {
                        if (spawnLocations[r, c] != null)
                        {
                            nonNullPositions++;
                        }
                    }
                }

                Assert.IsNotNull(spawnLocations);
                Assert.AreEqual(nonNullPositions, pattern.GetNumberOfCellsWhichHaveCarInIt());
            }

            [Test]
            public void Consecutive_horizontal_cells_should_be_at_colOffset()
            {
                var gridBehaviourMock = Substitute.For<IGridSpawnBeaviour>();
                gridBehaviourMock.RowOffset.Returns(URandom.value * 100);
                gridBehaviourMock.ColOffset.Returns(URandom.value * 100);

                GridPattern pattern = Build_a.GridPattern
                                    .With_a_row_count_of(3)
                                    .Has_pattern(1, 1, 1,
                                                 1, 1, 1,
                                                 1, 1, 1);

                var gridSpawner = new GridSpawnLocations(gridBehaviourMock);
                var spawnLocations = gridSpawner.GenerateSpawnLocationsFor(pattern);


                for (int r = 0; r < spawnLocations.GetLength(0); r++)
                {
                    var c0 = (Vector3)spawnLocations[r, 0];
                    var c1 = (Vector3)spawnLocations[r, 1];
                    var c2 = (Vector3)spawnLocations[r, 2];

                    Assert.AreEqual(gridBehaviourMock.ColOffset,
                        Vector3.Distance(c0, c1));
                    Assert.AreEqual(gridBehaviourMock.ColOffset,
                        Vector3.Distance(c1, c2));
                    Assert.AreEqual(gridBehaviourMock.ColOffset * 2,
                        Vector3.Distance(c0, c2));
                }
            }

            [Test]
            public void Consecutive_vertical_cells_should_be_at_rowOffset()
            {
                var gridBehaviourMock = Substitute.For<IGridSpawnBeaviour>();
                gridBehaviourMock.RowOffset.Returns(URandom.value * 100);
                gridBehaviourMock.ColOffset.Returns(URandom.value * 100);

                GridPattern pattern = Build_a.GridPattern
                                    .With_a_row_count_of(3)
                                    .With_a_col_count_of(3)
                                    .Has_pattern(1, 1, 1,
                                                 1, 1, 1,
                                                 1, 1, 1);

                var gridSpawner = new GridSpawnLocations(gridBehaviourMock);
                var spawnLocations = gridSpawner.GenerateSpawnLocationsFor(pattern);

                for (int c = 0; c < spawnLocations.GetLength(1); c++)
                {
                    var r0 = (Vector3)spawnLocations[0, c];
                    var r1 = (Vector3)spawnLocations[1, c];
                    var r2 = (Vector3)spawnLocations[2, c];

                    Assert.AreEqual(gridBehaviourMock.RowOffset,
                        Vector3.Distance(r0, r1));
                    Assert.AreEqual(gridBehaviourMock.RowOffset,
                        Vector3.Distance(r1, r2));
                    Assert.AreEqual(gridBehaviourMock.RowOffset * 2,
                        Vector3.Distance(r0, r2));
                }
            }
        }
    }

    public class Build_a
    {
        public static GridPatternBuilder GridPattern => new GridPatternBuilder();
    }

    public class GridPatternBuilder
    {
        private int rowCount = -1;
        private int colCount = -1;

        private GridPattern gridPattern;

        public GridPatternBuilder With_a_row_count_of(float rowCount)
        {
            this.rowCount = (int)rowCount;
            return this;
        }
        public GridPatternBuilder With_a_col_count_of(float colCount)
        {
            this.colCount = (int)colCount;
            return this;
        }

        public GridPatternBuilder Has_pattern(params int[] pattern)
        {
            if (rowCount < 1 && colCount > 0)
            {
                rowCount = pattern.Length / colCount;
            }
            else if (colCount < 1 && rowCount > 0)
            {
                colCount = pattern.Length / rowCount;
            }
            else if(rowCount < 1 && colCount < 1)
            {
                throw new Exception("Set either rowCount or colCount before setting a pattern");
            }

            gridPattern = new GridPattern();
            gridPattern.rows = new GridPattern.rowData[rowCount];
            for (int r = 0; r < rowCount; r++)
            {
                gridPattern.rows[r] = new GridPattern.rowData
                {
                    cells = new ACarType[colCount]
                };

                for (int c = 0; c < colCount; c++)
                {
                    gridPattern.rows[r].cells[c] = GetCarTypeFor(pattern[r * colCount + c]);
                }
            }

            return this;
        }

        private ACarType GetCarTypeFor(int carType)
        {
            if (carType == 0)
            {
                return null;
            }

            return Substitute.For<ACarType>();
        }

        public static implicit operator GridPattern(GridPatternBuilder patternBuilder)
        {
            if (patternBuilder.gridPattern != null)
            {
                return patternBuilder.gridPattern;
            }

            patternBuilder.gridPattern = new GridPattern();
            patternBuilder.gridPattern.rows = new GridPattern.rowData[patternBuilder.rowCount];
            for (int r = 0; r < patternBuilder.rowCount; r++)
            {
                patternBuilder.gridPattern.rows[r] = new GridPattern.rowData
                {
                    cells = new ACarType[patternBuilder.colCount]
                };
            }


            return patternBuilder.gridPattern;
        }
    }
}
