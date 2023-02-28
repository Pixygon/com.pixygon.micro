using System;
using UnityEngine;

namespace Pixygon.Micro.PixielMicro
{
    public class MovementCalculator : MonoBehaviour
    {
        public static float CalculateForce(float startForce, MovementConfig mov, bool running)
        {
            var horizontalForce = startForce;
            var move = MicroController._instance.Input.Movement.x;
            horizontalForce += move * mov.Speed;
            if (Mathf.Abs(move) < .01f)
                horizontalForce *= Mathf.Pow(1f - mov.HorizontalDampingWhenStopping, Time.deltaTime * 10f);
            else if (Math.Abs(Mathf.Sign(move) - Mathf.Sign(horizontalForce)) > .01f)
                horizontalForce *= Mathf.Pow(1f - mov.HorizontalDampingWhenTurning, Time.deltaTime * 10f);
            else
                horizontalForce *= Mathf.Pow(1f - mov.HorizontalDampingBasic, Time.deltaTime * 10f);
            var f = running ? mov.MaxSpeedRun : mov.MaxSpeedWalk;
            return Mathf.Clamp(horizontalForce, -f, f);
        }
    }
}