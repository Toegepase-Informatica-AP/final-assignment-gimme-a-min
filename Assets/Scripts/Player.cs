using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{

    public class Player : MovingObject
    {
        private Seeker seeker;

        public override void Initialize()
        {
            base.Initialize();
            seeker = GetComponentInParent<Seeker>();
        }
    }
}
