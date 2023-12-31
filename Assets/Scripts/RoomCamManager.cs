using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamManager : MonoBehaviour
{
    public GameObject virtualCam;
    void Awake()
    {
        virtualCam = transform.Find("Virtual Camera").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            virtualCam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            virtualCam.SetActive(false);
        }
    }

}
