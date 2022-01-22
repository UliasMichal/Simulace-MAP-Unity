using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPosition : MonoBehaviour
{
    // Start is called before the first frame update

    int counter = 0;
    int pocet = 0;

    private void FixedUpdate()
    {
        if (counter++ % 50 == 0)
        {
            Debug.Log(pocet++);
            Debug.Log(this.transform.position.x);
            Debug.Log(this.transform.position.y);
            Debug.Log(this.transform.position.z);
            Debug.Log(Vector3.Distance(this.transform.position, new Vector3(0, 0, 0)));
            Debug.Log("");
        }
    }
}
