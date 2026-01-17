using UnityEngine;
using System.Collections;

public class WristBand : MonoBehaviour
{
    public MeshRenderer wristBandMeshRenderer;
    public Color normalColor = Color.gray;
    public Color grabColor = Color.green;
    public Color tapColor = Color.red;

    public void GrabSelected()
    {
        StartCoroutine(ChangeColorCoroutine(wristBandMeshRenderer.material.color, grabColor, 0.1f));
    }

    public void GrabUnSelected()
    {
        StartCoroutine(ChangeColorCoroutine(wristBandMeshRenderer.material.color, normalColor, 0.1f));
    }

    public void TapSelected()
    {
        StartCoroutine(ChangeColorCoroutine(wristBandMeshRenderer.material.color, tapColor, 0.1f));
    }

    public void TapUnSelected()
    {
        StartCoroutine(ChangeColorCoroutine(wristBandMeshRenderer.material.color, normalColor, 0.1f));
    }

    private IEnumerator ChangeColorCoroutine(Color color1, Color color2, float duration)
    {
        float elapsed = 0f;
        Material mat = wristBandMeshRenderer.material;
        while (elapsed < duration)
        {
            Color lerpedColor = Color.Lerp(color1, color2, elapsed / duration);
            mat.color = lerpedColor;
            mat.SetColor("_EmissionColor", lerpedColor);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mat.color = color2;
        mat.SetColor("_EmissionColor", color2);
    }
}
