using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public enum MovingObjectTypes
    {
        SEEKER, PLAYER
    }

    public class Classroom : MonoBehaviour
    {
        public TextMeshPro seekerReward;

        public int seekerCount = 1;
        public int playerCount = 5;

        public Seeker seekerPrefab;
        public Player playerPrefab;

        private Jail jail;

        private GameObject players;
        private GameObject playerSpawnLocations;
        private GameObject seekers;
        private GameObject seekerSpawnLocations;

        private void OnEnable()
        {
            
            jail = transform.GetComponentInChildren<Jail>();
            players = transform.Find("Players").gameObject;
            seekers = transform.Find("Seekers").gameObject;
            playerSpawnLocations = transform.Find("PlayerSpawnLocations").gameObject;
            seekerSpawnLocations = transform.Find("SeekerSpawnLocations").gameObject;
        }

        private void FixedUpdate()
        {
            var seekerRewardText = 0f;

            foreach (var seeker in seekers.transform.GetComponentsInChildren<Seeker>())
            {
                seekerRewardText += seeker.GetCumulativeReward();
            }

            seekerReward.text = seekerRewardText.ToString("f3");
        }


        public void ClearEnvironment()
        {
            foreach (Transform player in players.transform)
            {
                Destroy(player.gameObject);
            }
            foreach (Transform seeker in seekers.transform)
            {
                Destroy(seeker.gameObject);
            }
        }

        public void ResetSpawnSettings()
        {
            foreach (SpawnLocation sl in playerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>())
            {
                sl.IsUsed = false;
            }

            foreach (SpawnLocation sl in seekerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>())
            {
                sl.IsUsed = false;
            }
        }

        public Vector3 GetAvailableSpawnLocation(MovingObjectTypes type)
        {
            SpawnLocation spawnLocation = GetRandomSpawnLocation(type);
            spawnLocation.IsUsed = true;
            Vector3 pos = spawnLocation.transform.position;
            return new Vector3(pos.x, pos.y, pos.z);
        }

        private SpawnLocation GetRandomSpawnLocation(MovingObjectTypes type)
        {
            IEnumerable<SpawnLocation> locations;
            switch (type)
            {
                case MovingObjectTypes.SEEKER:
                    locations = seekerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>();
                    break;
                case MovingObjectTypes.PLAYER:
                    locations = playerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>();
                    break;
                default:
                    locations = null;
                    break;
            }
            locations = locations.Where(x => !x.IsUsed);
            int randomIndex = Random.Range(0, locations.Count());
            SpawnLocation randomlyPicked = locations.ElementAt(randomIndex);
            return randomlyPicked;
        }

        public void SpawnSeekers()
        {
            // Moet het aantal gevraagde seekers spawnen, maar ook rekening houden met hoeveel spawnplaatsen er effectief zijn.
            for (int i = 0; i < seekerCount && i < seekerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>().Length; i++)
            {
                GameObject seeker = Instantiate(seekerPrefab.gameObject);

                seeker.transform.localPosition = GetAvailableSpawnLocation(MovingObjectTypes.SEEKER);
                seeker.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

                seeker.transform.SetParent(seekers.transform);
            }
        }

        public void SpawnPlayers()
        {
            // Moet het aantal gevraagde seekers spawnen, maar ook rekening houden met hoeveel spawnplaatsen er effectief zijn.
            for (int i = 0; i < playerCount && i < playerSpawnLocations.transform.GetComponentsInChildren<SpawnLocation>().Length; i++)
            {
                GameObject seeker = Instantiate(playerPrefab.gameObject);

                seeker.transform.localPosition = GetAvailableSpawnLocation(MovingObjectTypes.PLAYER);
                seeker.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

                seeker.transform.SetParent(players.transform);
            }
        }
    }
}
