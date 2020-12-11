using Unity.MLAgents;
using UnityEngine;

namespace Assets.Scripts
{
    public class Seeker : MovingObject
    {
        protected Player player;

        public override void Initialize()
        {
            base.Initialize();
            player = GetComponentInParent<Player>();
        }

        // D'Haese code
        // URL: https://ddhaese.github.io/ML-Agents/gedragingen-van-de-agent-en-de-andere-spelobjecten.html#obelix.cs
        public override void OnEpisodeBegin()
        {
            transform.localPosition = new Vector3(-22f, 1.5f, 22f);
            transform.localRotation = Quaternion.Euler(0f, 225f, 0f);

            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
        }
    }
}
