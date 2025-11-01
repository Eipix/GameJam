using UnityEngine;

namespace Common
{
    public class CurveMovement : MonoBehaviour
    {
        public AnimationCurve moveCurve = AnimationCurve.Linear(0, 0, 1, 0);
        
        private Vector3 _startPosition;
        private float _time;

        private float _duration;
        
        private float _previousOffset;
        private void Start()
        {
            _startPosition = transform.position;
            _duration = moveCurve.keys[^1].time;
            _time = Random.Range(0f, _duration);
        }

        private void Update()
        {
            _time += Time.deltaTime;
            
            float normalizedTime = Mathf.Repeat(_time, _duration) / _duration;

            float yOffset = moveCurve.Evaluate(normalizedTime);

            float deltaOffset = yOffset - _previousOffset;

            transform.position += new Vector3(0, deltaOffset, 0);

            _previousOffset = yOffset;
        }
    }
}