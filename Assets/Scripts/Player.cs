using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Assets.Scripts
{

    public class Player : MovingObject
    {
        public bool IsJailed { get; set; }
        public bool IsGrabbed { get; set; }
        public Seeker CapturedBy { get; set; }
        public override void CollectObservations(VectorSensor sensor)
        {
            base.CollectObservations(sensor);
            sensor.AddObservation(IsJailed);
            sensor.AddObservation(IsGrabbed);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            //if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit))
            //{
            //    Debug.Log(hit.transform.tag);
            //}
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
