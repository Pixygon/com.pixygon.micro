using System;
using UnityEngine;

namespace Pixygon.Micro {
    public class PlatformMovement : MonoBehaviour {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _maxSpeedWalk = 8f;
        [SerializeField] private float _maxSpeedRun = 12f;
        [SerializeField] private float _jumpPower = 12f;
        [Range(0f, 1f)] [SerializeField] private float _horizontalDampingBasic = .18f;
        [Range(0f, 1f)] [SerializeField] private float _horizontalDampingWhenStopping = .5f;
        [Range(0f, 1f)] [SerializeField] private float _horizontalDampingWhenTurning = .3f;
        [Range(0f, 1f)] [SerializeField] private float _verticalDamping = .5f;
        [Range(0f, 1f)] [SerializeField] private float _groundCheckSize = .2f;
        
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayer;
        
        [SerializeField] private ParticleSystem _landFx;
        [SerializeField] private AudioSource _landSfx;
        [SerializeField] private ParticleSystem _jumpFx;
        [SerializeField] private AudioSource _jumpSfx;
        [SerializeField] private ParticleSystem _runFx;
        
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private AnimatorController _anim;
        
        private bool _isGrounded;
        private Rigidbody2D _rigid;
        private float _horizontalForce;
        private float _maxSpeed;
        private MicroActor _actor;

        public bool XFlip => _renderer.flipX;
        
        public void Initialize() {
            _rigid = GetComponent<Rigidbody2D>();
            _actor = GetComponent<MicroActor>();
            _maxSpeed = _maxSpeedWalk;
        }
        private void OnEnable() {
            MicroController._instance.Input._jump += Jump;
            MicroController._instance.Input._run += Run;
        }
        private void OnDisable() {
            MicroController._instance.Input._jump -= Jump;
            MicroController._instance.Input._run -= Run;
        }
        private void Jump(bool started) {
            if (_actor.IsDead) return;
            var velocity = _rigid.velocity;
            if (started && _isGrounded)
                velocity = new Vector2(velocity.x, _jumpPower);
            else if (!started && velocity.y > 0f)
                velocity = new Vector2(velocity.x, velocity.y * _verticalDamping);
            else
                velocity = velocity;
            DoJump(velocity, started && _isGrounded);
        }
        public void DoJump(Vector2 velocity, bool playEffect) {
            if (playEffect) {
                _jumpFx.Play();
                _jumpSfx.Play();
                _anim.Jump();
            }
            _rigid.velocity = velocity;
        }
        private void Run(bool started) {
            _maxSpeed = started ? _maxSpeedRun : _maxSpeedWalk;
            if (started)
                _runFx.Play();
            else
                _runFx.Stop();
        }
        private void HandleGroundCheck() {
            var ground = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckSize, _groundLayer);
            if (!_isGrounded && ground) {
                _landFx.Play();
                _landSfx.Play();
                _anim.Land();
            }
            _isGrounded = ground;
            if(!_isGrounded)
                _anim.InAir();
            else
                _anim.Land();
        }
        public void HandleMovement() {
            if (_actor.IsDead) return;
            HandleGroundCheck();
            var horizontalForce = _rigid.velocity.x;
            horizontalForce += MicroController._instance.Input.Movement.x * _speed;
            if (MicroController._instance.Input.Movement.x < -0.1f && !_renderer.flipX)
                _renderer.flipX = true;
            else if (MicroController._instance.Input.Movement.x > 0.1f && _renderer.flipX)
                _renderer.flipX = false;

            if (Mathf.Abs(MicroController._instance.Input.Movement.x) < .01f)
                horizontalForce *= Mathf.Pow(1f - _horizontalDampingWhenStopping, Time.deltaTime * 10f);
            else if (Math.Abs(Mathf.Sign(MicroController._instance.Input.Movement.x) - Mathf.Sign(horizontalForce)) > .01f)
                horizontalForce *= Mathf.Pow(1f - _horizontalDampingWhenTurning, Time.deltaTime * 10f);
            else
                horizontalForce *= Mathf.Pow(1f - _horizontalDampingBasic, Time.deltaTime * 10f);
            
            horizontalForce = Mathf.Clamp(horizontalForce, -_maxSpeed, _maxSpeed);
            _anim.SetMovement(Mathf.Abs(horizontalForce));
            _rigid.velocity = new Vector2(horizontalForce, _rigid.velocity.y);
        }
        public void SetKinematic() {
            _rigid.isKinematic = true;
            _rigid.velocity = Vector2.zero;
        }
        public void ResetController() {
            _rigid.isKinematic = false;
            _rigid.velocity = Vector2.zero;
        }
    }
}