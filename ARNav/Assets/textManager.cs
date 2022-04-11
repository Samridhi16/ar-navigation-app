using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textManager : MonoBehaviour
{

    public Text lat, lon;
    // Update is called once per frame
    void FixedUpdate()
    {
        lat.text = "LAT: " + GPS.latitude.ToString();
        lon.text = "LON: " + GPS.longitude.ToString();

    }
}
