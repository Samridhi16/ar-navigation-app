
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json;


public class MapApi : MonoBehaviour
{
    // public Text thistext;
    public InputField mainInputField;

    public GameObject route_obj;
    Route route_script;

    public string uri;
    public string uri1 = "http://dev.virtualearth.net/REST/v1/Locations?CountryRegion=IN&addressLine=";
    public string uri2 = "&key=Av-XeuV_E_XqIJvlFKRfWrM5m-fiNpgC83I-DggNyAOQZYXWoNYR7BFSkwfNewUZ ";
    //for storing address
    public string destination;

    /*public void MapStart()
    {
        Debug.Log("MapStart");
        //AddValue();
    }*/

    public void AddValue()
    {
        destination = mainInputField.text.ToString();
        Debug.Log("iN aDD VALUE");
        MapStartCo();
    }

    public void MapStartCo()
    {
        Debug.Log("Reached");
        Debug.Log(destination);
        //uri = "http://dev.virtualearth.net/REST/v1/Locations?CountryRegion=IN&addressLine=+destination+&key=Av-XeuV_E_XqIJvlFKRfWrM5m-fiNpgC83I-DggNyAOQZYXWoNYR7BFSkwfNewUZ ";
        uri = uri1 + destination + uri2;
        Debug.Log("Samridi");
        Debug.Log(uri);
        StartCoroutine(Request());
    }

    //Coroutine
    IEnumerator Request()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            yield return www.SendWebRequest();
            Debug.Log(www.downloadHandler.text);
            CreateList(www.downloadHandler.text);
        }
    }
    private void Update()
    {
    }
    void CreateList(string jsonString)
    {
        Debug.Log("aditya");
        RootObject theplaces = new RootObject();
        Newtonsoft.Json.JsonConvert.PopulateObject(jsonString, theplaces);
        Debug.Log(theplaces.resourceSets[0].resources[0].geocodePoints[0].coordinates[0]);
        Debug.Log(theplaces.resourceSets[0].resources[0].geocodePoints[0].coordinates[1]);
        Debug.Log("end!");
        route_script = route_obj.GetComponent<Route>();
        route_script.ContinueHere();

        // Debug.Log("end!");
    }
}//End of the class

/*
 To interact with other script
 
    public GameObject route_obj;
    Route route_script;
    route_script = route_obj.GetComponent<Route>();
    route_script.ContinueHere();  
 */


public class Point
{
    public string type { get; set; }
    public List<double> coordinates { get; set; }
}

public class Address
{
    public string addressLine { get; set; }
    public string adminDistrict { get; set; }
    public string adminDistrict2 { get; set; }
    public string countryRegion { get; set; }
    public string formattedAddress { get; set; }
    public string locality { get; set; }
    public string postalCode { get; set; }
}

public class GeocodePoint
{
    public string type { get; set; }
    public List<double> coordinates { get; set; }
    public string calculationMethod { get; set; }
    public List<string> usageTypes { get; set; }
}

public class Resource
{
    public string __type { get; set; }
    public List<double> bbox { get; set; }
    public string name { get; set; }
    public Point point { get; set; }
    public Address address { get; set; }
    public string confidence { get; set; }
    public string entityType { get; set; }
    public List<GeocodePoint> geocodePoints { get; set; }
    public List<string> matchCodes { get; set; }
}

public class ResourceSet
{
    public int estimatedTotal { get; set; }
    public List<Resource> resources { get; set; }
}

public class RootObject
{
    public string authenticationResultCode { get; set; }
    public string brandLogoUri { get; set; }
    public string copyright { get; set; }
    public List<ResourceSet> resourceSets { get; set; }
    public int statusCode { get; set; }
    public string statusDescription { get; set; }
    public string traceId { get; set; }
}
