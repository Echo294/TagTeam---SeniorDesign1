using Unity.VisualScripting;
using UnityEngine;

public class GroundedCollieder : MonoBehaviour
{
    BasicPlayerController playerController;
    void Start()
    {
        if(GetComponentInParent<BasicPlayerController>() != null)
        {
            playerController = GetComponentInParent<BasicPlayerController>();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Grounded");
        playerController.setGrounded(true);
    }
}
