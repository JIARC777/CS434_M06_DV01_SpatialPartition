using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Soldier
    {
        public MeshRenderer soldierMeshRenderer;
        public Transform soldierTrans;
        protected float walkSpeed;

        //Solider Class is a linked List with soldiers being links - each has a reference to the next and previous
        public Soldier previousSoldier;
        public Soldier nextSoldier;
        public virtual void Move() { }
        // Friendly classes will move to nearest soldier
        public virtual void Move(Soldier soldier) { }
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

