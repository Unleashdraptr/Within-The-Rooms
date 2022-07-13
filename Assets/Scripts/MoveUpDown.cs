using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float Buffer;
    public bool Up;
    public bool Down;

    // Start is called before the first frame update
    void Start()
    {
        Up = true;
        Down = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Buffer <= 0)
        {
            if (Up == true)
            {
                transform.Translate(0, 2.5f, 0);
                Up = false;
                Down = true;
                Buffer = 10;
            }
            else if (Down == true)
            {
                transform.Translate(0, -2.5f, 0);
                Up = true;
                Down = false;
                Buffer = 10;
            }
        }
        else
            Buffer -= 1 * Time.deltaTime;
    }
}
