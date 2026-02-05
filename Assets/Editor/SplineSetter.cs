using UnityEngine;
using UnityEditor;
using UnityEngine.Splines;
using Unity.Mathematics;

namespace com.VisionXR.EditorTools
{
    public class SplineSetter : EditorWindow
    {
        [Header("References")]
        public SplineContainer targetSplineContainer;
        public GameObject centerObject;

        [Header("Settings")]
        public float radius = 1.0f;
        public float startAngle = 0f;
        public float endAngle = 180f;
        public int divisions = 7;

        [MenuItem("VisionXR/Spline Setter Tool")]
        public static void ShowWindow()
        {
            GetWindow<SplineSetter>("Spline Setter");
        }

        private void OnGUI()
        {
            GUILayout.Label("Arc Spline Generator", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            targetSplineContainer = (SplineContainer)EditorGUILayout.ObjectField("Target Spline", targetSplineContainer, typeof(SplineContainer), true);
            centerObject = (GameObject)EditorGUILayout.ObjectField("Center Object", centerObject, typeof(GameObject), true);

            EditorGUILayout.Space();
            radius = EditorGUILayout.FloatField("Radius", radius);
            startAngle = EditorGUILayout.FloatField("Start Angle", startAngle);
            endAngle = EditorGUILayout.FloatField("End Angle", endAngle);
            divisions = EditorGUILayout.IntField("Divisions (Knots)", divisions);

            // Ensure we have at least 2 points
            if (divisions < 2) divisions = 2;

            EditorGUILayout.Space();

            if (GUILayout.Button("Set Spline", GUILayout.Height(40)))
            {
                GenerateSpline();
            }
        }

        private void GenerateSpline()
        {
            if (targetSplineContainer == null)
            {
                Debug.LogError("Please assign a Target Spline Container!");
                return;
            }

            // 1. Record for Undo (so you can Ctrl+Z the generation)
            Undo.RecordObject(targetSplineContainer, "Generate Arc Spline");

            // 2. Clear and get reference
            var spline = targetSplineContainer.Spline;
            spline.Clear();

            // 3. Determine base position
            Vector3 centerPos = centerObject != null ? centerObject.transform.position : Vector3.zero;

            // 4. Generate points
            for (int i = 0; i < divisions; i++)
            {
                float t = (float)i / (divisions - 1);

                // Interpolate angles (Degrees to Radians)
                float currentAngle = Mathf.Lerp(startAngle, endAngle, t) * Mathf.Deg2Rad;

                // Calculate Circle Position (X and Z)
                float x = Mathf.Cos(currentAngle) * radius;
                float z = Mathf.Sin(currentAngle) * radius;

                Vector3 worldPos = centerPos + new Vector3(x, 0, z);

                // Convert World Position to Spline Local Position
                float3 localSplinePos = (float3)targetSplineContainer.transform.InverseTransformPoint(worldPos);

                // 5. Add Knot
                // We set tangents to 0 for a "linear" feel, 
                // or you can manually adjust them in the editor for curves.
                spline.Add(new BezierKnot(localSplinePos));
            }

            // 6. Finalize
            // SplineContainer detects changes to the 'Spline' property automatically.
            // We mark it dirty so Unity knows to save the asset/scene changes.
            EditorUtility.SetDirty(targetSplineContainer);

            // Forces the Scene View to redraw the spline path immediately
            SceneView.RepaintAll();

            Debug.Log($"Spline generated with {divisions} knots.");
        }
    }
}