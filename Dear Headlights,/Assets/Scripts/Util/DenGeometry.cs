using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Den {
    public class Geometry {

        /// <summary>
        /// Draw a three-point bezier curve with the given start, end, and control points. Returns a vector array with the given number of positions.
        /// </summary>
        public static Vector3[] ThreePointBezierCurve(Vector3 beginningPoint, Vector3 endPoint, Vector3 controlPoint, int positionCount) {
            float time = 0f;
            float timeStep = 1 / (float) positionCount;

            Vector3[] returnPoints = new Vector3[positionCount];

            for (int i = 0; i < positionCount; i++) {
                Vector3 point1 = Vector3.Lerp(beginningPoint, controlPoint, time);
                Vector3 point2 = Vector3.Lerp(controlPoint, endPoint, time);
                returnPoints[i] = Vector3.Lerp(point1, point2, time);
                time += timeStep;
            }

            return returnPoints;
        }

        /// <summary>
        /// Draws an ellipse with the given radius. Returns as an array of vector positions.
        /// </summary>
        public static Vector3[] Ellipse(float xRadius, float yRadius, int segmentCount) {
            float x;
            float y;
            float z = 0f;

            float angle = 20f;

            Vector3[] positions = new Vector3[segmentCount + 1];

            for (int i = 0; i < positions.Length; i++) {
                x = (Mathf.Sin(Mathf.Deg2Rad * angle) * xRadius);
                y = 0f;
                z = (Mathf.Cos(Mathf.Deg2Rad * angle) * yRadius);

                positions[i] = new Vector3(x, y, z);

                angle += (360f / segmentCount);
            }

            return positions;
        }

        /// <summary>
        /// Draws a circle with the given radius. Returns as an array of vector points.
        /// </summary>
        public static Vector3[] Circle(float radius, int segmentCount) {
            return Ellipse(radius, radius, segmentCount);
        }

        /// <summary>
        /// Draws a circle with every other position 'pinched in' by a given ammount. As seen in the hit game Nerve Damage.
        /// </summary>
        public static Vector3[] NerveDamageCrosshair(float radius, int segments, float pinch, bool pinchOddVectors) {
            float x;
            float y;
            float z = 0f;

            float angle = 0f;

            Vector3[] returnArray = new Vector3[segments + 1];

            for (int i = 0; i < returnArray.Length; i++) {
                float xDeath = 0;
                if ((i + Den.Math.BoolToInt(pinchOddVectors)) % 2 == 0) { xDeath = Den.Math.Map(pinch, 0f, 1f, 0f, radius); }
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * (radius - xDeath * Random.Range(0.9f, 1.1f));
                y = 0f;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * (radius - xDeath * Random.Range(0.9f, 1.1f));

                returnArray[i] = new Vector3(x, y, z);

                angle += (360f / segments);
            }

            return returnArray;
        }
    }
}

