using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtility 
{
    public static Vector3 RoundPositionUpto3Decimals(Vector3 position)
    {
        float x = Mathf.Round(position.x * 1000f) / 1000f;
        float y = Mathf.Round(position.y * 1000f) / 1000f;
        float z = Mathf.Round(position.z * 1000f) / 1000f;
        return new Vector3(x, y, z);
    }

    public static Vector3 RoundPositionUpto4Decimals(Vector3 position)
    {
        float x = Mathf.Round(position.x * 10000f) / 10000f;
        float y = Mathf.Round(position.y * 10000f) / 10000f;
        float z = Mathf.Round(position.z * 10000f) / 10000f;
        return new Vector3(x, y, z);
    }

    public static float RoundFloat(float value)
    {
        return Mathf.Round(value * 100f) / 100f;
    }
    public static Vector3 RoundRotationUpto2Decimals(Vector3 eulerAngles)
    {
        float x = Mathf.Round(eulerAngles.x * 10f) / 10f;
        float y = Mathf.Round(eulerAngles.y * 10f) / 10f;
        float z = Mathf.Round(eulerAngles.z * 10f) / 10f;
        return new Vector3(x, y, z);
    }
    public static string FormatVector3(Vector3 vector)
    {
        return string.Format("({0:F3}, {1:F3}, {2:F3})", vector.x, vector.y, vector.z);
    }

    //public Vector3 ParseVector3(string vectorString)
    //{
    //    // Remove the parentheses
    //    if (vectorString.StartsWith("(") && vectorString.EndsWith(")"))
    //    {
    //        vectorString = vectorString.Substring(1, vectorString.Length - 2);
    //    }

    //    // Split the items
    //    string[] sArray = vectorString.Split(',');

    //    // If the string is malformed and doesn't contain exactly 3 parts, return Vector3.zero as a fallback
    //    if (sArray.Length != 3)
    //    {
    //        Debug.Log("ParseVector3: Input string does not contain exactly 3 parts separated by commas.");
    //        return Vector3.zero;
    //    }

    //    // Store as a Vector3
    //    Vector3 result = new Vector3(
    //        float.Parse(sArray[0].Trim(), System.Globalization.CultureInfo.InvariantCulture),
    //        float.Parse(sArray[1].Trim(), System.Globalization.CultureInfo.InvariantCulture),
    //        float.Parse(sArray[2].Trim(), System.Globalization.CultureInfo.InvariantCulture));

    //    return result;
    //}

    /// <summary>
    /// Parses a formatted string back into a Vector3, returning Vector3.zero if any error occurs.
    /// </summary>
    /// <param name="formattedVector">The formatted string representation of a Vector3.</param>
    /// <returns>The parsed Vector3 or Vector3.zero if parsing fails.</returns>
    public static Vector3 ParseVector3(string formattedVector)
    {
        try
        {
            // Remove the parentheses
            formattedVector = formattedVector.Trim('(', ')');

            // Replace multiple periods with a single period
            //formattedVector = System.Text.RegularExpressions.Regex.Replace(formattedVector, @"\.\.+", ".");

            // Split the components
            string[] components = formattedVector.Split(',');

            // Check if we have exactly 3 components
            if (components.Length != 3)
            {
                return Vector3.zero;
            }

            // Parse each component and handle potential parsing errors
            float x = ParseComponent(components[0]);
            float y = ParseComponent(components[1]);
            float z = ParseComponent(components[2]);

            return new Vector3(x, y, z);
        }
        catch
        {
            // Return Vector3.zero if any error occurs
            return Vector3.zero;
        }
    }

    /// <summary>
    /// Parses a single component of the vector, returning 0.0 if parsing fails.
    /// </summary>
    /// <param name="component">The string representation of the component.</param>
    /// <returns>The parsed float value or 0.0 if parsing fails.</returns>
    private static float ParseComponent(string component)
    {
        // Replace multiple periods with a single period
        component = System.Text.RegularExpressions.Regex.Replace(component, @"\.\.+", ".");

        // Attempt to parse the component
        float value;
        if (!float.TryParse(component, out value))
        {
            value = 0.0f; // Default to 0.0 if parsing fails
        }

        return value;
    }



}
