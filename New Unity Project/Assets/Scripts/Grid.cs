using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Grid
    {
        int cellSize;
        Soldier[,] cells;

        public Grid(int mapWidth, int cellSize)
        {
            this.cellSize = cellSize;
            int numberOfCells = mapWidth / cellSize;
            cells = new Soldier[numberOfCells, numberOfCells];
        }

        public void Add(Soldier soldier)
        {
            int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

            // See the previous Soldier to null - Will be set by a Soldier coming after
            soldier.previousSoldier = null;
            // Set the next soldier to the previous soldier residing on the list
            soldier.nextSoldier = cells[cellX, cellZ];
            
            cells[cellX, cellZ] = soldier;
            if (soldier.nextSoldier != null)
            {
                soldier.nextSoldier.previousSoldier = soldier;
            }
        }

        public Soldier FindClosestEnemy(Soldier friendlySoldier)
        {
            int cellX = (int)(friendlySoldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(friendlySoldier.soldierTrans.position.z / cellSize);

            Soldier enemy = cells[cellX, cellZ];
            Soldier closestSoldier = null;
            float bestDistSqr = Mathf.Infinity;
            while (enemy != null)
            {
                float distSqr = (enemy.soldierTrans.position - friendlySoldier.soldierTrans.position).sqrMagnitude;
                if (distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;
                    closestSoldier = enemy;
                }
                enemy = enemy.nextSoldier;
            }
            return closestSoldier;
        }
        public void Move(Soldier soldier, Vector3 oldPos)
        {
            int oldCellX = (int)(oldPos.x / cellSize);
            int oldCellz = (int)(oldPos.z / cellSize);

            int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

            if (oldCellX == cellX && oldCellz == cellZ)
                return;
            if (soldier.previousSoldier != null)
                soldier.previousSoldier.nextSoldier = soldier.nextSoldier;
            if (soldier.nextSoldier != null)
                soldier.nextSoldier.previousSoldier = soldier.previousSoldier;
            if (cells[oldCellX, oldCellz] == soldier)
                cells[oldCellX, oldCellz] = soldier.nextSoldier;
            Add(soldier);

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

