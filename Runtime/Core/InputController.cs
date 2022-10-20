﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Pixygon.Micro {
    public class InputController : MonoBehaviour {
        [SerializeField] private InputActionAsset _inputAsset;
        
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

        private void OnEnable() {
            _moveAction = _inputAsset.FindAction("Move");
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
    }
}