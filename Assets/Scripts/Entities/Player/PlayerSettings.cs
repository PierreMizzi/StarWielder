using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{

    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Custom/PlayerSettings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {

        [Header("Health")]

        public float maxHealth = 300f;

        [Header("Energy")]
        public float baseEnergy = 50f;
        public float energyDepleatRate = 0.25f;


        [Header("Speed")]
        public float speed = 80f;

        public float smoothTime = 0.5f;

        public float immediatePositionScale = 0.1f;

        [Header("Dash")]
        public float dashDistance = 1f;
        public float dashDuration = 0.2f;
        public float dashCooldownDuration = 3f;


    }

}