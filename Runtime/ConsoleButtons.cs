using UnityEngine;

namespace Pixygon.Micro
{
    public class ConsoleButtons : MonoBehaviour
    {
        [SerializeField] private Transform _jumpButton;
        [SerializeField] private Transform _runButton;
        [SerializeField] private Transform _dpadButton;
        [SerializeField] private Transform _homeButton;
        [SerializeField] private Transform _pauseButton;
        [SerializeField] private Transform _leftShoulderButton;
        [SerializeField] private Transform _rightShoulderButton;

        [SerializeField] private Vector3 _jumpUpPos;
        [SerializeField] private Vector3 _jumpDownPos;
        [SerializeField] private Vector3 _runUpPos;
        [SerializeField] private Vector3 _runDownPos;
        [SerializeField] private Vector3 _shoulderLUpPos;
        [SerializeField] private Vector3 _shoulderLDownPos;
        [SerializeField] private Vector3 _shoulderRUpPos;
        [SerializeField] private Vector3 _shoulderRDownPos;
        [SerializeField] private Vector3 _homeUpPos;
        [SerializeField] private Vector3 _homeDownPos;
        [SerializeField] private Vector3 _selectUpPos;
        [SerializeField] private Vector3 _selectDownPos;
        
        private void OnEnable() {
            MicroController._instance.Input._jump += Jump;
            MicroController._instance.Input._run += Run;
            MicroController._instance.Input._home += Home;
            MicroController._instance.Input._pause += Pause;
            MicroController._instance.Input._shoulderL += ShoulderL;
            MicroController._instance.Input._shoulderR += ShoulderR;
            MicroController._instance.Input._move += Dpad;
        }
        private void OnDisable() {
            MicroController._instance.Input._jump -= Jump;
            MicroController._instance.Input._run -= Run;
            MicroController._instance.Input._home -= Home;
            MicroController._instance.Input._pause -= Pause;
            MicroController._instance.Input._shoulderL -= ShoulderL;
            MicroController._instance.Input._shoulderR -= ShoulderR;
            MicroController._instance.Input._move -= Dpad;
        }

        private void Dpad(Vector2 v) {
            _dpadButton.transform.localEulerAngles = new Vector3(v.y*-5f, v.x*-5f, 0);
        }
        private void Jump(bool started) {
            _jumpButton.transform.localPosition = started ? _jumpDownPos : _jumpUpPos;
        }
        private void Run(bool started) {
            _runButton.transform.localPosition = started ? _runDownPos : _runUpPos;
        }
        private void Pause(bool started) {
            _pauseButton.transform.localPosition = started ? _selectDownPos : _selectUpPos;
        }
        private void Home(bool started) {
            _homeButton.transform.localPosition = started ? _homeDownPos : _homeUpPos;
        }
        private void ShoulderL(bool started) {
            _leftShoulderButton.transform.localPosition = started ? _shoulderLDownPos : _shoulderLUpPos;
        }
        private void ShoulderR(bool started) {
            _rightShoulderButton.transform.localPosition = started ? _shoulderRDownPos : _shoulderRUpPos;
        }
    }
}
