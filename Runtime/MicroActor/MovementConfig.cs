using UnityEngine;

namespace Pixygon.Micro {
    [CreateAssetMenu(fileName = "New Movementconfig", menuName = "PixygonMicro/MovementConfig")]
    public class MovementConfig : ScriptableObject {
        public float Speed = 1f;
        public float MaxSpeedWalk = 8f;
        public float MaxSpeedRun = 12f;
        public float JumpPower = 12f;
        public float CoyoteTimeDuration = .5f;
        public float JumpBufferDuration = .5f;
        [Range(0f, 1f)] public float HorizontalDampingBasic = .18f;
        [Range(0f, 1f)] public float HorizontalDampingWhenStopping = .5f;
        [Range(0f, 1f)] public float HorizontalDampingWhenTurning = .3f;
        [Range(0f, 1f)] public float VerticalDamping = .5f;
        [Range(0f, 1f)] public float GroundCheckSize = .2f;
    }
}