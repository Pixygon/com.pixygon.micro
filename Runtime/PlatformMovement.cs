using System;
using UnityEngine;
using Random = System.Random;

namespace Pixygon.Micro {
    public class PlatformMovement : MonoBehaviour {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _maxSpeedWalk = 8f;
        [SerializeField] private float _maxSpeedRun = 12f;
        [SerializeField] private float _jumpPower = 12f;
        [SerializeField] private float _coyoteTimeDuration = .5f;
        [SerializeField] private float _jumpBufferDuration = .5f;
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
        
        private Rigidbody2D _rigid;
        private float _horizontalForce;
        private float _maxSpeed;
        private MicroActor _actor;
        private float _coyoteTime;
        private float _jumpBuffer;

        public bool IsGrounded { get; private set; }
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
            var playFx = false;
            var velocity = _rigid.velocity;
            if (started && IsGrounded) {
                velocity = new Vector2(velocity.x, _jumpPower);
                _coyoteTime = 0f;
                _jumpBuffer = 0f;
                playFx = true;
            } else if (started && !IsGrounded && _coyoteTime > 0f) {
                Debug.Log("Coyote-time jump!");
                velocity = new Vector2(velocity.x, _jumpPower);
                _coyoteTime = 0f;
                _jumpBuffer = 0f;
                playFx = true;
            } else if (started && !IsGrounded && _jumpBuffer > 0f) {
                Debug.Log("Jump-buffer jump!");
                velocity = new Vector2(velocity.x, _jumpPower);
                _coyoteTime = 0f;
                _jumpBuffer = 0f;
                playFx = true;
            } else if (started && !IsGrounded)
                _jumpBuffer = _jumpBufferDuration;
            else if (!started && velocity.y > 0f)
                velocity = new Vector2(velocity.x, velocity.y * _verticalDamping);
            else
                velocity = velocity;

            DoJump(velocity, playFx);
        }

        public void DoJump(Vector2 velocity, bool playEffect) {
            if (playEffect) {
                _jumpFx.Play();
                _jumpSfx.pitch = UnityEngine.Random.Range(0.9f, 1.05f);
                _jumpSfx.Play();
                _anim.Jump();
            }
            _rigid.velocity = velocity;
        }
        private void Run(bool started) {
            if (_actor.IsDead) return;
            _maxSpeed = started ? _maxSpeedRun : _maxSpeedWalk;
            if (started)
                _runFx.Play();
            else
                _runFx.Stop();
        }
        private void HandleGroundCheck() {
            var ground = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckSize, _groundLayer);
            if (!IsGrounded && ground) {
                _landFx.Play();
                _landSfx.pitch = UnityEngine.Random.Range(0.9f, 1.05f);
                _landSfx.Play();
                _anim.Land();
                if (_jumpBuffer > 0f) {
                    Jump(true);
                }
            }

            if (IsGrounded && !ground)
                _coyoteTime = _coyoteTimeDuration;
            IsGrounded = ground;
            if(!IsGrounded)
                _anim.InAir();
            else
                _anim.Land();
        }
        public void HandleMovement(bool cannotMove) {
            if (_actor.IsDead) return;
            HandleGroundCheck();
            if (_coyoteTime > 0f)
                _coyoteTime -= Time.deltaTime;
            if (_jumpBuffer > 0f)
                _jumpBuffer -= Time.deltaTime;
            var horizontalForce = _rigid.velocity.x;
            var move = cannotMove ? 0f : MicroController._instance.Input.Movement.x;
            if (!_actor.Invincible) {
                horizontalForce += move * _speed;
                if (move < -0.1f && !_renderer.flipX)
                    _renderer.flipX = true;
                else if (move > 0.1f && _renderer.flipX)
                    _renderer.flipX = false;
            }
            if (Mathf.Abs(move) < .01f)
                horizontalForce *= Mathf.Pow(1f - _horizontalDampingWhenStopping, Time.deltaTime * 10f);
            else if (Math.Abs(Mathf.Sign(move) - Mathf.Sign(horizontalForce)) > .01f)
                horizontalForce *= Mathf.Pow(1f - _horizontalDampingWhenTurning, Time.deltaTime * 10f);
            else
                horizontalForce *= Mathf.Pow(1f - _horizontalDampingBasic, Time.deltaTime * 10f);
            if(_maxSpeed < _maxSpeedWalk && _runFx.isPlaying)
                _runFx.Stop();
            if(_maxSpeed > _maxSpeedWalk && !_runFx.isPlaying)
                _runFx.Play();
            horizontalForce = Mathf.Clamp(horizontalForce, -_maxSpeed, _maxSpeed);
            _anim.SetMovement(Mathf.Abs(horizontalForce));
            _rigid.velocity = new Vector2(horizontalForce, _rigid.velocity.y);
        }

        public void SetDead() {
            _rigid.isKinematic = true;
            _rigid.velocity = Vector2.zero;
            _runFx.Stop();
        }
        public void SetKinematic() {
        }
        public void ResetController() {
            _rigid.isKinematic = false;
            _rigid.velocity = Vector2.zero;
        }
    }
}