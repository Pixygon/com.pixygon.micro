﻿using System;
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
        public UnityAction<bool> _select;
        public UnityAction<bool> _home;
        public UnityAction<bool> _quit;

        private InputAction _moveAction;
        public Vector2 Movement => _moveAction.ReadValue<Vector2>();
        [field: SerializeField] public InputActionAsset InputAsset { get; private set; }

        private void OnEnable() {
            _moveAction = InputAsset.FindAction("Move");
            _moveAction.Enable();
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
        public void Select(InputAction.CallbackContext context) {
            if (_select == null) return;
            if (context.performed)
                _select.Invoke(true);
            if (context.canceled)
                _select.Invoke(false);
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
    }
}