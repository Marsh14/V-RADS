using UnityEngine;

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