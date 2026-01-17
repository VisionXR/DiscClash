using System;
using System.Globalization;
using UnityEngine;

namespace com.VisionXR.HelperClasses
{
    public static class DataConverter
    {


        /// <summary>
        /// Converts a Vector3 into a comma-separated string with exactly 3 decimal places.
        /// </summary>
        /// <param name="vector">The Vector3 to format.</param>
        /// <returns>A comma-separated string with 3 decimal places for each component.</returns>
        public static string FormatVector3(Vector3 vector)
        {
            return $"{vector.x.ToString("F3", CultureInfo.InvariantCulture)},{vector.y.ToString("F3", CultureInfo.InvariantCulture)},{vector.z.ToString("F3", CultureInfo.InvariantCulture)}";
        }


        /// <summary>
        /// Parses a comma-separated string to return a Vector3.
        /// Assumes the input string is in the format "x,y,z".
        /// </summary>
        /// <param name="vectorString">Comma-separated string representing a Vector3.</param>
        /// <returns>A Vector3 parsed from the input string.</returns>
        public static Vector3 ParseVector3(string vectorString)
        {
            string[] values = vectorString.Split(',');

            if (values.Length != 3)
            {
                throw new FormatException("Input string is not in the correct format. Expected format: x,y,z.");
            }

            // Parse each value and convert to float
            float x = float.Parse(values[0], CultureInfo.InvariantCulture);
            float y = float.Parse(values[1], CultureInfo.InvariantCulture);
            float z = float.Parse(values[2], CultureInfo.InvariantCulture);

            return new Vector3(x, y, z);
        }

        public static string EncodeTransform(Vector3 pos, Vector3 rot)
        {
            return $"{pos.x:F3},{pos.y:F3},{pos.z:F3}&{rot.x:F2},{rot.y:F2},{rot.z:F2}";
        }


        public static string EncodeTransform(Vector3 pos)
        {
            return $"{pos.x:F3},{pos.y:F3},{pos.z:F3}";
        }

        public static string EncodeTransformTwo(Vector3 pos)  
        {
            return $"{pos.x:F2},{pos.y:F2},{pos.z:F2}"; 
        }



        public static TransformData DecodeTransform(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            var parts = data.Split('&');
            if (parts.Length != 2) return null;

            var posParts = parts[0].Split(',');
            var rotParts = parts[1].Split(',');

            if (posParts.Length != 3 || rotParts.Length != 3) return null;

            try
            {
                Vector3 pos = new Vector3(
                    float.Parse(posParts[0]),
                    float.Parse(posParts[1]),
                    float.Parse(posParts[2])
                );

                Vector3 rot = new Vector3(
                    float.Parse(rotParts[0]),
                    float.Parse(rotParts[1]),
                    float.Parse(rotParts[2])
                );

                return new TransformData
                {
                    Position = pos,
                    Rotation = rot
                };
            }
            catch
            {
                return null; // or throw if you prefer to catch errors elsewhere
            }
        }


    }
}
