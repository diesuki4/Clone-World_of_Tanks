using UnityEngine;
using System.Collections;


public class LSJ_TrackScroll : MonoBehaviour
{
    /*
     * This script is attached to the "Scroll_Track" in the tank.
     * This script controls the texture scrolling of the Scroll_Track.
    */


    // User options >>
    public bool Is_Left; // Referred to from "Static_Wheel_Parent_CS".
    public Transform Reference_Wheel; // Referred to from "Static_Wheel_Parent_CS".
    public string Reference_Name;
    public string Reference_Parent_Name;
    public float Scroll_Rate = 0.005f;
    public bool Scroll_Y_Axis;
    // << User options

    // For editor script.
    public bool Has_Changed;

    Material thisMaterial;
    public float Delta_Ang; // Referred to from "Static_Wheel_Parent_CS".
    float previousAng;
    Vector2 offset;
    LSJ_MainBodySetting bodyScript;
    bool isRepairing;


    void Start()
    {
        Initialize();
    }


    void Initialize()
    {
        // Find the reference wheels.
        if (Reference_Wheel == null)
        {
            if (string.IsNullOrEmpty(Reference_Name) == false && string.IsNullOrEmpty(Reference_Parent_Name) == false)
            {
                Reference_Wheel = transform.parent.Find(Reference_Parent_Name + "/" + Reference_Name);
            }
        }
        if (Reference_Wheel == null)
        {
            Debug.LogError("'Reference wheel' for Scroll_Track cannot be found.");
            Destroy(this);
            return;
        }

        // Send this reference to all the "Static_Wheel_Parent_CS" in the tank.
        LSJ_StaticWheelParent[] staticWheelParentScripts = transform.parent.GetComponentsInChildren<LSJ_StaticWheelParent>();
        for (int i = 0; i < staticWheelParentScripts.Length; i++)
        {
            staticWheelParentScripts[i].Prepare_With_Scroll_Track(this);
        }

        thisMaterial = GetComponent<Renderer>().material;
        bodyScript = GetComponentInParent<LSJ_MainBodySetting>();
    }


    void Update()
    {
        // Check the tank is visible by any camera.
        if (bodyScript.Visible_Flag)
        {
            Scroll();
        }
    }


    void Scroll()
    {
        var currentAng = Reference_Wheel.localEulerAngles.y;
        Delta_Ang = Mathf.DeltaAngle(currentAng, previousAng);
        if (Scroll_Y_Axis)
        {
            offset.y += Scroll_Rate * Delta_Ang;
        }
        else
        {
            offset.x += Scroll_Rate * Delta_Ang;
        }
        thisMaterial.SetTextureOffset(LSJ_GeneralSettings.Main_Tex_Name, offset);
        previousAng = currentAng;
    }


    void Track_Destroyed_Linkage(bool isLeft)
    { // Called from "Damage_Control_Center_CS".
        if (isLeft != Is_Left)
        { // The direction is different.
            return;
        }

        // Make the track invisible.
        GetComponent<Renderer>().enabled = false;

        // Disable this script.
        this.enabled = false;

        // Switch the flag.
        isRepairing = true;
    }


    void Track_Repaired_Linkage(bool isLeft)
    { // Called from "Damage_Control_Center_CS".

        // Make the track visible.
        GetComponent<Renderer>().enabled = true;

        // Enable this script.
        this.enabled = true;

        // Switch the flag.
        isRepairing = false;
    }


    void Pause(bool isPaused)
    { // Called from "Game_Controller_CS".

        // Check the track is being repaired.
        if (isRepairing)
        {
            return;
        }

        this.enabled = !isPaused;
    }


    void OnDestroy()
    { // Avoid memory leak.
        if (thisMaterial)
        {
            Destroy(thisMaterial);
        }
    }
}

