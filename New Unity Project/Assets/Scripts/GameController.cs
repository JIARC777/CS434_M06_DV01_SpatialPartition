using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialPartitionPattern
{
    public class GameController : MonoBehaviour
    {
        public GameObject friendlyObj;
        public GameObject enemyObj;

        public Material enemyMaterial;
        public Material closestEnemyMaterial;

        public Transform enemyParent;
        public Transform friendlyParent;

        public Text fastResult;
        public Text usingSP;
        

        List<Soldier> enemySoldiers = new List<Soldier>();
        List<Soldier> friendlySoldiers = new List<Soldier>();
        List<Soldier> closestEnemies = new List<Soldier>();

        float mapWidth = 50f;
        int cellSize = 10;
        int numberOfSoldiers = 100;

        float summedTime;
        float avgTime;
        int numberOfSamples;

        bool isSpatial;

        Grid grid;


        // Start is called before the first frame update
        void Start()
        {
            grid = new Grid((int)mapWidth, cellSize);
            for (int i = 0; i < numberOfSoldiers; i++)
            {
                Vector3 randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
                GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;
                enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));
                newEnemy.transform.parent = enemyParent;

                randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));
                GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity) as GameObject;
                friendlySoldiers.Add(new Friend(newFriendly, mapWidth));
                newFriendly.transform.parent = friendlyParent;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
            for (int i = 0; i < enemySoldiers.Count; i++)
            {
                enemySoldiers[i].Move();
            }
            for (int i = 0; i < closestEnemies.Count; i++)
            {
                closestEnemies[i].soldierMeshRenderer.material = enemyMaterial;
            }

            closestEnemies.Clear();
            // The code to change materials is duplicated to make analyzing timestamps easy and accurate as the only line to change is the search algorthim
            // Timestamp start for using slow algorthm
            float startTimeStamp = Time.realtimeSinceStartup * 1000;
            for (int i = 0; i < friendlySoldiers.Count; i++)
            {
                Soldier closestEnemy;
                if (isSpatial)
                    
                    closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);
                else
                    closestEnemy = FindClosestEnemySlow(friendlySoldiers[i]);

                if (closestEnemy != null)
                {
                    closestEnemy.soldierMeshRenderer.material = closestEnemyMaterial;
                    closestEnemies.Add(closestEnemy);
                    friendlySoldiers[i].Move(closestEnemy);
                }
            }
            float endTimeStamp = Time.realtimeSinceStartup * 1000;

            numberOfSamples++;
            summedTime += (endTimeStamp - startTimeStamp);
            Debug.Log(summedTime);
            avgTime = summedTime / numberOfSamples;
            fastResult.text = avgTime.ToString();
            usingSP.text = isSpatial.ToString();

        }
        Soldier FindClosestEnemySlow(Soldier soldier)
        {
            Soldier closestEnemy = null;
            float bestDistSqr = Mathf.Infinity;
            for (int i = 0; i < enemySoldiers.Count; i++)
            {
                float distSqr = (soldier.soldierTrans.position - enemySoldiers[i].soldierTrans.position).sqrMagnitude;
                if (distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;
                    closestEnemy = enemySoldiers[i];
                }
            }
            return closestEnemy;
        }   

        public void toggleSpatialPartition()
        {
            isSpatial = !isSpatial;
            summedTime = 0;
            numberOfSamples = 0;
        }
    }
}

