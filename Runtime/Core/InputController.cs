using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Pixygon.Micro {
    public class InputController : MonoBehaviour {
        
        public UnityAction<Vector2> _move;
        public UnityAction<bool> _jump;
        public UnityAction<bool> _run;
        public UnityAction<bool> _shoulderR;
        public UnityAction<bool> _shoulderL;
        public UnityAction<bool> _pause;
        public UnityAction<bool> _home;
        public UnityAction<bool> _quit;

        private InputAction _moveAction;
        public Vector2 Movement => _moveAction.ReadValue<Vector2>();
        [field: SerializeField] public InputActionAsset InputAsset { get; private set; }
        
        public InputAction JumpAction { get; private set; }
        public InputAction RunAction { get; private set; }
        public InputAction ShoulderRAction { get; private set; }
        public InputAction ShoulderLAction { get; private set; }
        public InputAction PauseAction { get; private set; }
        public InputAction HomeAction { get; private set; }
        public InputAction PrimaryFingerPos { get; private set; }
        public InputAction SecondaryFingerPos { get; private set; }
        public InputAction TouchAction { get; private set; }
        public int TouchZoom { get; private set; }

        private void OnEnable() {
            _moveAction = InputAsset.FindAction("Move");
            _moveAction.Enable();
            JumpAction = InputAsset.FindAction("Jump");
            RunAction = InputAsset.FindAction("Run");
            ShoulderRAction = InputAsset.FindAction("ShoulderR");
            ShoulderLAction = InputAsset.FindAction("ShoulderL");
            PauseAction = InputAsset.FindAction("Select");
            HomeAction = InputAsset.FindAction("Jump");
            PrimaryFingerPos = InputAsset.FindAction("PrimaryFingerPosition");
            SecondaryFingerPos = InputAsset.FindAction("SecondaryFingerPosition");
            TouchAction = InputAsset.FindAction("SecondaryTouchContact");
            TouchAction.started += _ => ZoomStart();
            TouchAction.canceled += _ => ZoomEnd();
        }

        public void Move(InputAction.CallbackContext context) {
            _move?.Invoke(context.ReadValue<Vector2>());
        }
        public void Jump(InputAction.CallbackContext context) {
            if (_jump == null) return;
            if (context.performed)
                _jump.Invoke(true);
            if (context.canceled)
                _jump.Invoke(false);
        }
        public void Run(InputAction.CallbackContext context) {
            if (_run == null) return;
            if (context.performed)
                _run.Invoke(true);
            if (context.canceled)
                _run.Invoke(false);
        }
        public void ShoulderR(InputAction.CallbackContext context) {
            if (_shoulderR == null) return;
            if (context.performed)
                _shoulderR.Invoke(true);
            if (context.canceled)
                _shoulderR.Invoke(false);
        }
        public void ShoulderL(InputAction.CallbackContext context) {
            if (_shoulderL == null) return;
            if (context.performed)
                _shoulderL.Invoke(true);
            if (context.canceled)
                _shoulderL.Invoke(false);
        }
        public void Pause(InputAction.CallbackContext context) {
            if (_pause == null) return;
            if (context.performed)
                _pause.Invoke(true);
            if (context.canceled)
                _pause.Invoke(false);
        }
        public void Home(InputAction.CallbackContext context) {
            if (_home == null) return;
            if (context.performed)
                _home.Invoke(true);
            if (context.canceled)
                _home.Invoke(false);
        }
        public void Quit(InputAction.CallbackContext context) {
            if (_quit == null) return;
            if (context.performed)
                _quit.Invoke(true);
            if (context.canceled)
                _quit.Invoke(false);
        }
        public static async void Rumble(float duration, float intensity = 1f) {
            InputSystem.ResumeHaptics();
            if (Gamepad.current == null) return;
            var curve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            var time = 0f;
            while (time < duration) {
                time += Time.deltaTime;
                Gamepad.current.SetMotorSpeeds(intensity*curve.Evaluate(time/duration), intensity*curve.Evaluate(time/duration));
                await Task.Yield();
            }
            Gamepad.current.SetMotorSpeeds(0f, 0f);
            InputSystem.PauseHaptics();
        }
        
        private Coroutine ZoomCoroutine;
        public void ZoomStart() {
            if (!MicroController._instance.HomeMenuOpen) return;
            ZoomCoroutine = StartCoroutine(ZoomDetection());
            TouchZoom = 0;
        }

        public void ZoomEnd() {
            if (!MicroController._instance.HomeMenuOpen) return;
            StopCoroutine(ZoomCoroutine);
            TouchZoom = 0;
        }

        private IEnumerator ZoomDetection() {
            var prevDistance = 0f;
            while (true) {
                TouchZoom = 0;
                var distance = Vector2.Distance(MicroController._instance.Input.PrimaryFingerPos.ReadValue<Vector2>(),
                    MicroController._instance.Input.SecondaryFingerPos.ReadValue<Vector2>());

                //if (Vector2.Dot(primaryDelta, secondaryDelta) < -.9f) {
                    //1 if swipe, -1 if pinch
                //}
                if (distance > prevDistance) {
                    TouchZoom = -1;
                } else if (distance < prevDistance) {
                    TouchZoom = 1;
                }
                prevDistance = distance;
            }
        }
        

        public void ClearInputs() {
            _move = null;
            _jump = null;
            _run = null;
            _shoulderR = null;
            _shoulderL = null;
            _pause = null;
        }
    }
}