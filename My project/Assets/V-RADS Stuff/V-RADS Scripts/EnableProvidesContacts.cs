using UnityEngine;

// This script enables the "Provides Contacts" feature on a CharacterController component.
public class EnableProvidesContacts : MonoBehaviour
{
    void Start()
    {
        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enableOverlapRecovery = true; // Enables Provides Contacts
            
        }
        else
        {
            
        }
    }
}