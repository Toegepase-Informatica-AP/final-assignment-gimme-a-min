namespace Assets.Scripts
{

    public class Player : MovingObject
    {
        public bool IsJailed { get; set; } = false;
        public bool IsGrabbed { get; set; } = false;
        public Seeker CapturedBy { get; set; } = null;

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

        public void CapturedLogic()
        {
            IsJailed = true;
            IsGrabbed = false;
            CapturedBy.HasPlayerGrabbed = false;
            CapturedBy = null;
        }
    }
}
