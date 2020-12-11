namespace Assets.Scripts
{

    public class Player : MovingObject
    {
        public bool IsJailed { get; set; }
        public bool IsGrabbed { get; set; }

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
    }
}
