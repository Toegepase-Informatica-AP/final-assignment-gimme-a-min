using Unity.MLAgents;
using UnityEngine;

namespace Assets.Scripts
{
    public class Seeker : MovingObject
    {
        private Player player;

        public override void Initialize()
        {
            base.Initialize();
            player = GetComponentInParent<Player>();
        }
    }
}
