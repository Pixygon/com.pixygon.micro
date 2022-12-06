using UnityEngine;

namespace Pixygon.Micro {
    public class AnimatorController : MonoBehaviour {
        [SerializeField] private Animator _anim;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int Jump1 = Animator.StringToHash("Jump");
        private static readonly int Air = Animator.StringToHash("InAir");
        private static readonly int Land1 = Animator.StringToHash("Land");
        private static readonly int Damage1 = Animator.StringToHash("Damage");
        private const float IdleThreshold = .5f;
        private const float RunThreshold = 9f;

        public void Jump() {
            _anim?.SetTrigger(Jump1);
        }

        public void Land() {
            _anim?.SetBool(Air, false);
            _anim?.SetTrigger(Land1);
        }

        public void InAir() {
            _anim?.SetBool(Air, true);
        }

        public void Damage() {
            _anim?.SetTrigger(Damage1);
        }

        public void SetMovement(float i) {
            if (i is > IdleThreshold and < RunThreshold) {
                _anim?.SetBool(IsWalking, true);
                _anim?.SetBool(IsRunning, false);
            }
            else if (i > RunThreshold) {
                _anim?.SetBool(IsWalking, true);
                _anim?.SetBool(IsRunning, true);
            }
            else {
                _anim?.SetBool(IsWalking, false);
                _anim?.SetBool(IsRunning, false);
            }
        }
    }
}