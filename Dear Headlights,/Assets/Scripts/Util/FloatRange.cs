using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Den {
    /// <summary>
    /// Stores two floats meant to represent a range, and contains several useful functions.
    /// </summary>
    [Serializable]
    public class FloatRange {

        public float min;
        public float max;
        public float size {
            get {
                return max - min;
            }
        }

        public FloatRange(float _min, float _max) {
            min = _min;
            max = _max;
        }

        public float Random {
            get { return UnityEngine.Random.Range(min, max); }
        }

        public float MapToFloatRange(float input, float min, float max) {
            return Den.Math.Map(input, min, max, this.min, this.max);
        }

        public float ClampToFloatRange(float value) {
            return Mathf.Clamp(value, min, max);
        }
    }
}
