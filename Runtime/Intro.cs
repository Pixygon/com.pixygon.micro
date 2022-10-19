using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Pixygon.Micro {
    public class Intro : MonoBehaviour {
        [SerializeField] private SpriteRenderer _logo;
        [SerializeField] private TextMeshPro _title;
        [SerializeField] private AnimationCurve _curve;

        public async void StartIntro(Action endIntro) {
            _logo.color = new Color(1f, 1f, 1f, 0f);
            _title.color = new Color(1f, 1f, 1f, 0f);
            var timer = 0f;
            while (timer < .5f) {
                timer += Time.deltaTime;
                await Task.Yield();
            }

            timer = 0f;
            while (timer < 2f) {
                _logo.color = new Color(1f, 1f, 1f, _curve.Evaluate(timer * .5f));
                _logo.transform.localScale =
                    Vector3.Lerp(Vector3.one * 5f, Vector2.one * .5f, _curve.Evaluate(timer * .5f));
                _title.color = new Color(1f, 1f, 1f, _curve.Evaluate(timer * .5f));
                _title.transform.position =
                    Vector3.Lerp(Vector3.up * -7f, Vector2.up * -3.5f, _curve.Evaluate(timer * .5f));
                timer += Time.deltaTime;
                await Task.Yield();
            }

            timer = 0f;
            while (timer < 1f) {
                timer += Time.deltaTime;
                await Task.Yield();
            }

            endIntro.Invoke();
        }
    }
}