using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSetup : MonoBehaviour
{
    public GameObject Blocker;
    public GameObject Platform;
    public GameObject DeathBlocker;

    public Transform[] Room1BlockerLocations;
    public Transform[] Room3PlatformLocations;
    public Transform[] DropperDeathBlockerLocations;

    public int Resets;

    public GameObject Room1Children;
    public GameObject Room2Children;
    public GameObject Room3Children;
    public GameObject Room4Walls;
    public GameObject DropperChildren;

    public bool[] Room1LocationsTaken;
    public bool[] Room2LocationsTaken;
    public bool[] DropperLocationsTaken;

    public GameObject BlockerStorage;
    public GameObject PlatformStorage;
    public GameObject DeathBlockerStorage;
    // Start is called before the first frame update
    void Start()
    {
        Resets = -1;
        Room1BlockerLocations = Room1Children.gameObject.GetComponentsInChildren<Transform>();
        Room3PlatformLocations = Room3Children.gameObject.GetComponentsInChildren<Transform>();
        DropperDeathBlockerLocations = DropperChildren.gameObject.GetComponentsInChildren<Transform>();
    }

    public void UpdateBlocks()
    {
        UpdateRoom1();
        UpdateRoom2();
        RemovePlatforms();
        UpdateRoom3();
        UpdateRoom4();
        UpdateDropper();
    }


    void UpdateRoom1()
    {
        for (int i = 0; i < Room1BlockerLocations.Length; i++)
        {
            int WakeUp = Random.Range(0, 48);
            if (WakeUp <= Resets && Room1LocationsTaken[i] == false)
            {
                Instantiate(Blocker, Room1BlockerLocations[i].position, Quaternion.Euler(0, 0, 90), BlockerStorage.transform);
                Room1LocationsTaken[i] = true;
            }
        }
    }
    void UpdateRoom2()
    {

    }
    void UpdateRoom3()
    {
        for (int i = 0; i < Room3PlatformLocations.Length; i++)
        {
            int Sleep = Random.Range(0, 28);
            if (Sleep >= Resets)
            {
                Instantiate(Platform, Room3PlatformLocations[i].position, Quaternion.Euler(0, 0, 0), PlatformStorage.transform);
            }

        }
    }
    void UpdateRoom4()
    {
        int Wallspawn = Resets - 1;
        if (Wallspawn >= 0)
        {
            Room4Walls.transform.GetChild(Wallspawn).gameObject.SetActive(true);
        }
    }
    void UpdateDropper()
    {
        int Spawn = Random.Range(1, 10) + Resets;
        if (Spawn > 48)
        {
            Spawn = 48;
        }
        for (int i = 0; i < Spawn; i++)
        {
            int Location = Random.Range(1, 49);
            if (DropperLocationsTaken[Location] == false)
            {
                Instantiate(DeathBlocker, DropperDeathBlockerLocations[Location].position, Quaternion.Euler(0, 0, 0), DeathBlockerStorage.transform);
                DropperLocationsTaken[Location] = true;
            }
        }
    }
    void RemovePlatforms()
    {
        for (int i = PlatformStorage.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(PlatformStorage.transform.GetChild(i).gameObject);
        }

    }
}
