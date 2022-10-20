using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixygon.Micro
{
    public class AnimatorController : MonoBehaviour {
        [SerializeField] private Animator _anim;
        public void Jump() {
            _anim.SetTrigger("Jump");
        }
        public void Land() {
            _anim.SetBool("InAir", false);
            _anim.SetTrigger("Land");
        }

        public void InAir() {
            _anim.SetBool("InAir", true);
        }
        public void Damage() {
            _anim.SetTrigger("Damage");
        }
        public void Movement(int i) {
            switch (i) {
                case 0:
                    _anim.SetBool("IsWalking", false);
                    _anim.SetBool("IsRunning", false);
                    break;
                case 1:
                    _anim.SetBool("IsWalking", true);
                    _anim.SetBool("IsRunning", false);
                    break;
                case 2:
                    _anim.SetBool("IsWalking", true);
                    _anim.SetBool("IsRunning", true);
                    break;
            }
        }
    }
}
