using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Den {
    /// <summary>
    /// Stores two integers meant to represent a range, and contains some useful functionality.
    /// </summary>
    [Serializable]
    public class IntRange {

        public int min;
        public int max;

        public IntRange(int _min, int _max) {
            min = _min;
            max = _max;
        }

        // Seems to be broken :( wasn't important enough to fix yet
        public int Random {
            get {
                return UnityEngine.Random.Range(min, max + 1);
            }
        }
    }
}
