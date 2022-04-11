using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPS : MonoBehaviour
{
    public GameObject MapApi1_obj;
    MapApi MapApi1_script;
    public static float latitude;
    public static float longitude;

    public static float latitudeCopy;
    public static float longitudeCopy;

    // public Text latText, lonText;
    public static GPS instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.location.isEnabledByUser)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
        }
    }
    /* void FixedUpdate()
     {
         latText.text = "LAT: " + GPS.latitude.ToString();
         lonText.text = "LON: " + GPS.longitude.ToString();
     }
     */
    public void Start()
    {
        StartCoroutine(GPSStart());
    }
    //Coroutine
    IEnumerator GPSStart()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;
        // Start service before querying location
        Input.location.Start(5f, 10f);
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }
        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

            //latText.text = "LAT: " + latitude.ToString();
            //lonText.text = "LON: " + longitude.ToString();
        }
        latitudeCopy = Input.location.lastData.latitude;
        longitudeCopy = Input.location.lastData.longitude;
        // Stop service if there is no need to query location updates continuously
        // Input.location.Stop();       
    }
}//END OF THE CLASS