using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Friend : Soldier
    {
        // Start is called before the first frame update
        public Friend(GameObject soldierObj, float mapWidth)
        {
            this.soldierTrans = soldierObj.transform;
            this.walkSpeed = 2f;
        }
        public override void Move(Soldier closestEnemy)
        {
            soldierTrans.rotation = Quaternion.LookRotation(closestEnemy.soldierTrans.position - soldierTrans.position);
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
        }
    }
}

