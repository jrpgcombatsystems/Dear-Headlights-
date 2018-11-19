using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Den {
    public class Math {

        /// <summary>
        /// Remaps a value from a given range to another given range.
        /// </summary>
        public static float Map(float x, float inputMin, float inputMax, float outputMin, float outputMax) {
            return (x - inputMin) * (outputMax - outputMin) / (inputMax - inputMin) + outputMin;
        }

        /// <summary>
        /// Remaps a number from a given range to a range of 0 to 1.
        /// </summary>
        public static float Map01(float x, float min, float in_max) {
            return Map(x, min, in_max, 0f, 1f);
        }

        /// <summary>
        /// Wraps a number between a given minimum and maximum value.
        /// </summary>
        public static float Wrap(float x, float min, float max) {
            return (((x - min) % (max - min)) + (max - min)) % (max - min) + min;
        }

        /// <summary>
        /// Wraps an integer between a given minimum and maximum value.
        /// </summary>
        public static int Wrap(int x, int min, int max) {
            return (((x - min) % (max - min)) + (max - min)) % (max - min) + min;
        }

        /// <summary>
        /// Wraps a number between 0 and 1.
        /// </summary>
        public static float Wrap01(float x) {
            return Wrap(x, 0f, 1f);
        }

        /// <summary>
        /// Wraps an integer between 0 and 1.
        /// </summary>
        public static int Wrap01(int x) {
            return Wrap(x, 0, 1);
        }

        /// <summary>
        /// Keeps a given number at 0 or above.
        /// </summary>
        public static float ClampPositive(float value) {
            if (value < 0) { value = 0; }
            return value;
        }

        /// <summary>
        /// Keeps a given number above or equal to a certain value.
        /// </summary>
        public static float KeepAboveOrEqualTo(float value, float min) {
            if (value < min) { value = min; }
            return value;
        } 

        /// <summary>
        /// Returns a random value that is either 1 or -1f.
        /// </summary>
        public static float Either1orNegative1 {
            get {
                float rand = Random.value;
                if (rand < 0.5f) { return -1f; } else { return 1f; }
            }
        }

        /// <summary>
        /// Converts a bool into an integer. False becomes 0 and true becomes 1.
        /// </summary>
        public static int BoolToInt(bool input) {
            if (input) { return 1; }
            else { return 0; }
        }

        /// <summary>
        /// Converts an integer into a bool. 0 becomes false and 1 becomes true.
        /// </summary>
        public static bool IntToBool(int input) {
            if (input == 0) { return false; }
            else if (input == 1) { return true; }
            else {
                Debug.LogError("The given integer was neither 0 or 1. Returning false.");
                return false;
            }
        }

        /// <summary>
        /// Returns the average of the values in the given array.
        /// </summary>
        public static float Average(float[] values) {
            float average = 0f;
            for (int i = 0; i < values.Length; i++) { average += values[i]; }
            return average / values.Length;
        }

        /// <summary>
        /// Returns the average of the values in the given list.
        /// </summary>
        public static float Average(List<float> values) {
            return Average(values.ToArray());
        }

        /// <summary>
        /// Rounds a number to the decimal with the given number of places.
        /// </summary>
        public static float RoundToDecimalPlaces(float value, int decimalPlaces) {
            float multiplier = Mathf.Pow(10, decimalPlaces);
            return Mathf.Round(value * multiplier) / multiplier;
        }
    }
}
