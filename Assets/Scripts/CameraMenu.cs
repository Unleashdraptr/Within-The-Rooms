using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenu : MonoBehaviour
{
    public Vector3[] CameraPositions;
    public int CameraRoomNumber;
    public float WaitTime;
    public bool MoveDown;
    void Update()
    {
        if (CameraRoomNumber == 1 && MoveDown == true || CameraRoomNumber == 5 && MoveDown == true)
        {
            transform.Translate(0, -0.025f, 0);
        }
        else
        {
            WaitTime += 1 * Time.deltaTime;
        }
        if (WaitTime >= 20)
        {
            MoveDown = true;
            CameraRoomNumber += 1;
            if (CameraRoomNumber > 5)
            {
                CameraRoomNumber = 1;
            }
            transform.SetPositionAndRotation(CameraPositions[CameraRoomNumber - 1], transform.rotation);
            WaitTime = 0;
        }
        transform.Rotate(0, 0.025f, 0);
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Transition")
        {
            MoveDown = false;
            yield return new WaitForSeconds(1.75f);
            WaitTime = 20f;
        }
    }
}
