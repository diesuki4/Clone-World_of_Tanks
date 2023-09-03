using System.Collections;
using UnityEngine;

public class LSJ_AIHeadquatersHelper : MonoBehaviour
{
    /*
     * This script is attached to the top object of the tank.
     * This script works in combination with "AI_Headquaters_CS" in the scene.
     * These variables are used for giving the target information to the AI tanks in the scene by "AI_Headquaters_CS" in the "Game_Controller" in the scene.
     * Also the variables are used for detecting and attacking the target by the AI tanks.
    */


    // User options >>
    public float Visibility_Upper_Offset = 1.5f;
    // << User options

    // Referred to from "AI_Headquaters_CS".
    public LSJ_IDSettings ID_Script;
    public Transform Body_Transform; // Referred to from also "AI_CS".
    public LSJ_AI AI_Script;



    void Start()
    {
        Get_Components();
        Send_Reference();
    }


    void Get_Components()
    { // This function is called at the fisrst time, and also when the tank is respawned.
        ID_Script = GetComponent<LSJ_IDSettings>();
        Body_Transform = GetComponentInChildren<Rigidbody>().transform;
        AI_Script = GetComponentInChildren<LSJ_AI>();
    }


    void Send_Reference()
    {
        // Send this reference to "AI_Headquaters_CS" in the scene.
        if (LSJ_AIHeadquaters.Instance)
        {
            LSJ_AIHeadquaters.Instance.Receive_Helpers(this);
        }
    }


    void Respawned()
    { // Called from "Respawn_Controller_CS", when the tank has been respawned.
      // Get the new child components.
        Get_Components();
    }


    void Prepare_Removing()
    { // Called from "Respawn_Controller_CS", just before the tank is removed.
        if (LSJ_AIHeadquaters.Instance)
        {
            LSJ_AIHeadquaters.Instance.Remove_From_List(this);
        }
    }

}
