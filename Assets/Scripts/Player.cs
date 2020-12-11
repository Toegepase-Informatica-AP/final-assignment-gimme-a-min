using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

namespace Assets.Scripts
{

    public class Player : MovingObject
    {
        protected Seeker seeker;

        public override void Initialize()
        {
            base.Initialize();
            seeker = GetComponentInParent<Seeker>();
        }

        // D'Haese code
        // URL: https://ddhaese.github.io/ML-Agents/gedragingen-van-de-agent-en-de-andere-spelobjecten.html#obelix.cs
        public override void OnEpisodeBegin()
        {
            transform.localPosition = new Vector3(0f, 1.5f, 0f);
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
        }
    }
}
