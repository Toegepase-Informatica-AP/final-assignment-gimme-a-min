
using UnityEngine;

namespace Assets.Scripts
{

    public class Player : MovingObject
    {
        public bool IsJailed { get; set; } = false;
        public bool IsGrabbed { get; set; } = false;
        public Seeker CapturedBy { get; set; } = null;

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!IsGrabbed && !IsJailed)
            {
                AddReward(0.001f);
            }
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            if (!IsGrabbed)
            {
                base.OnActionReceived(vectorAction);
            }
        }
    }
}
