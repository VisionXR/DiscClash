using com.VisionXR.GameElements;
using System;
using UnityEngine;

public class StrikerProperties : MonoBehaviour
{
    [Header(" Game Objects")]
    public Rigidbody rb;
    public GameObject HandPosObject;
    public AimLine aimLine;
   

    [Header(" Striker Name")]
    public string strikerName;

    [Header(" Default Properties")]
    public float minAim = 1f;
    public float maxAim = 4f;
    public float minTime = 1f;
    public float maxTime = 2.5f;
    public float minPower = 2f;
    public float maxPower = 3.5f;

    [Header(" Properties")]
    public Color aimColor = Color.white;
    public float aim;
    public float time;
    public float power;


    

    public void OnEnable()
    {
        SetProperties();
    }

    public void SetProperties()
    {
        rb.mass = power;
        rb.linearDamping = time;
        aimLine.SetCutOffLength(aim);
        aimLine.SetColor(aimColor);
    }

    public void ChangeProperties(float power, float aim, float time,Color aimColor)
    {
        
        this.aim = aim;
        this.time = time;
        this.power = power;
        this.aimColor = aimColor;
        SetProperties();

    }
}
