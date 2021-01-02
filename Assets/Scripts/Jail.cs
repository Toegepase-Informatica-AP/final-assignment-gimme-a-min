using UnityEngine;

namespace Assets.Scripts
{
    public class Jail : MonoBehaviour
    {
        private Player player = null;
        private Seeker seeker = null;

        private void OnTriggerEnter(Collider collision)
        {
            Transform collObject = collision.transform;

            if (collObject.CompareTag("LeftEye") || collObject.CompareTag("RightEye"))
            {
                collObject = collObject.parent;
            }

            if (collObject.CompareTag("Player"))
            {
                player = collObject.gameObject.GetComponent<Player>();

                if (player != null && player.CapturedBy != null)
                {
                    seeker = player.CapturedBy;
                    PerformCapturingProcedure();
                }
            }

            if (collObject.CompareTag("Seeker"))
            {
                seeker = collObject.gameObject.GetComponent<Seeker>();

                if (seeker != null)
                {
                    if (seeker.HasPlayerGrabbed)
                    {
                        player = seeker.CapturedPlayer;
                        PerformCapturingProcedure();
                    }
                }
            }
        }

        private void PerformCapturingProcedure()
        {
            if (seeker != null)
            {
                PutPlayerInJail();
                seeker = null;
                player = null;
            }
        }

        private void PutPlayerInJail()
        {
            if (player != null && !player.IsJailed && seeker != null)
            {
                // Player
                player.CapturedLogic();
                player.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

                // Seeker
                seeker.ClearCapturedPlayer();
                // seeker.transform.position = seeker.Classroom.transform.position;
            }
        }
    }
}
