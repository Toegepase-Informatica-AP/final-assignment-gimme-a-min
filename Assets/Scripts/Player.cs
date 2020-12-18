
using UnityEngine;

namespace Assets.Scripts
{

    public class Player : MovingObject
    {
        public bool IsJailed { get; set; } = false;
        public bool IsGrabbed { get; set; } = false;

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
            //Debug.Log(GetCumulativeReward().ToString("f3"));
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            if (!IsGrabbed && !IsJailed)
            {
                base.OnActionReceived(vectorAction);
                AddReward(-0.1f);
            }
        }
    }
}
