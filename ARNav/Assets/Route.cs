using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//using RouteWay;
using Newtonsoft.Json;

public class Route : MonoBehaviour
{
   // public GameObject arrow;
    public GameObject map_obj;
    MapApi map_script;


    public string source;
    public string destination;

    public float StartLatitude;
    public float StartLongitude;

    public string getRouteLink;

    public void ContinueHere()
    {
        //0-*arrow.SetActive(true);
        //Debug.Log("Reached Route.cs");
        GetCoord();
        GetRoute();
        StartCoroutine(Request());
        // Debug.Log("Leaving Route.cs");
    }


    void GetCoord()
    {
        map_script = map_obj.GetComponent<MapApi>();
        destination = map_script.destination;

        StartLatitude = GPS.latitudeCopy;
        StartLongitude = GPS.longitudeCopy;
        source = StartLatitude.ToString() + "," + StartLongitude.ToString();
    }

    public void GetRoute()
    {
        string url1 = "http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0=";
        string url2 = source;
        string url3 = "&wp.1=" + destination;
        string url4 = "&avoid=minimizeTolls&key=AhRUwNkdNCy9v2wC_klN5Gw4qPXirs2I_Egb6KOK5Dt9pLqzI1JWBlXdY7kXpo4U";
        //URL: http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0=SOURCE&wp.1=+DESTINATION+&avoid=minimizeTolls&key=AhRUwNkdNCy9v2wC_klN5Gw4qPXirs2I_Egb6KOK5Dt9pLqzI1JWBlXdY7kXpo4U

        //getRouteLink = url1 + url2 + url3 + url4; USE THIS AFTERWARDS
        getRouteLink = url1 + "pune" + url3 + url4;

        Debug.Log("ROute working!!!!!!!!!!!!!");
        Debug.Log(getRouteLink);
    }
    //Coroutine
    IEnumerator Request()
    {
        using (UnityWebRequest ww1 = UnityWebRequest.Get(getRouteLink))
        {
            Debug.Log("Working.....");
            yield return ww1.SendWebRequest();
            Debug.Log(ww1.downloadHandler.text);
            CreateList(ww1.downloadHandler.text);
        }
    }
    void CreateList(string jsonString)
    {
        string[] InstructionArr;
        long hr, v, min, sec;

        RouteWay.RootObject ro1 = new RouteWay.RootObject();
        Newtonsoft.Json.JsonConvert.PopulateObject(jsonString, ro1);
        RouteWay.RouteLeg rl = ro1.resourceSets[0].resources[0].routeLegs[0];
        RouteWay.Resource res = ro1.resourceSets[0].resources[0];
        RouteWay.ResourceSet rset = ro1.resourceSets[0];
        List<RouteWay.ItineraryItem> IterItems = ro1.resourceSets[0].resources[0].routeLegs[0].itineraryItems;
        int size = ro1.resourceSets[0].resources[0].routeLegs[0].itineraryItems.Count;
        InstructionArr = new string[size];
        sec = Convert.ToInt32(res.travelDuration);
        hr = sec / 3600;
        v = sec % 3600;
        min = v / 60;
        sec = v % 60;
        Debug.Log("Total Travel Duration" + hr + ":" + min + ":" + sec + "\n");
        Debug.Log("route.cs" + res.travelDistance + "\n" + res.travelDuration);
        Debug.Log("size of list" + size);
        int InstCntr = 0;
        foreach (var rs in IterItems)
        {
            Debug.Log("Route.cs maneuver type:" + rs.instruction.maneuverType);
            Debug.Log("towardsroad :"+rs.towardsRoadName);
            Debug.Log("traveldist :"+rs.travelDistance);
            InstructionArr[InstCntr] = rs.instruction.text;
            Debug.Log("inst text"+rs.instruction.text);
            //String s = rs.instruction.maneuverType.ToString();
            //int dist = ;
            //Debug.Log("Route.cs compass:" + rs.details[0].compassDegrees);
        }
    }
    /*void WorldVirtualCoord()
    {
       for(int itr1 = 0; itr1 < size; itr1++)
       {
            if (itr1 == 0)
            {
                //Use Source Co-ordinates
                //Augment Start Sign
            }
            if (itr1 == size - 1)
            {
                //Augment Stop Sign
            }

       }
    }*/
}
namespace RouteWay
{
    public class ActualEnd
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }
    public class ActualStart
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Point
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Address
    {
        public string adminDistrict { get; set; }
        public string adminDistrict2 { get; set; }
        public string countryRegion { get; set; }
        public string formattedAddress { get; set; }
        public string locality { get; set; }
    }

    public class GeocodePoint
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
        public string calculationMethod { get; set; }
        public List<string> usageTypes { get; set; }
    }

    public class EndLocation
    {
        public List<double> bbox { get; set; }
        public string name { get; set; }
        public Point point { get; set; }
        public Address address { get; set; }
        public string confidence { get; set; }
        public string entityType { get; set; }
        public List<GeocodePoint> geocodePoints { get; set; }
        public List<string> matchCodes { get; set; }
    }

    public class Shield
    {
        public List<string> labels { get; set; }
        public int roadShieldType { get; set; }
    }

    public class RoadShieldRequestParameters
    {
        public int bucket { get; set; }
        public List<Shield> shields { get; set; }
    }

    public class Detail
    {
        public int compassDegrees { get; set; }
        public List<int> endPathIndices { get; set; }
        public List<string> locationCodes { get; set; }
        public string maneuverType { get; set; }
        public string mode { get; set; }
        public List<string> names { get; set; }
        public string roadType { get; set; }
        public List<int> startPathIndices { get; set; }
        public RoadShieldRequestParameters roadShieldRequestParameters { get; set; }
    }

    public class Instruction
    {
        public object formattedText { get; set; }
        public string maneuverType { get; set; }
        public string text { get; set; }
    }

    public class ManeuverPoint
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Warning
    {
        public string origin { get; set; }
        public string severity { get; set; }
        public string text { get; set; }
        public string to { get; set; }
        public string warningType { get; set; }
    }

    public class ItineraryItem
    {
        public string compassDirection { get; set; }
        public List<Detail> details { get; set; }
        public string exit { get; set; }
        public string iconType { get; set; }
        public Instruction instruction { get; set; }
        public bool isRealTimeTransit { get; set; }
        public ManeuverPoint maneuverPoint { get; set; }
        public int realTimeTransitDelay { get; set; }
        public string sideOfStreet { get; set; }
        public string tollZone { get; set; }
        public string towardsRoadName { get; set; }
        public string transitTerminus { get; set; }
        public double travelDistance { get; set; }
        public int travelDuration { get; set; }
        public string travelMode { get; set; }
        public List<Warning> warnings { get; set; }
        public List<string> signs { get; set; }
    }

    public class EndWaypoint
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
        public string description { get; set; }
        public bool isVia { get; set; }
        public string locationIdentifier { get; set; }
        public int routePathIndex { get; set; }
    }

    public class StartWaypoint
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
        public string description { get; set; }
        public bool isVia { get; set; }
        public string locationIdentifier { get; set; }
        public int routePathIndex { get; set; }
    }

    public class RouteSubLeg
    {
        public EndWaypoint endWaypoint { get; set; }
        public StartWaypoint startWaypoint { get; set; }
        public double travelDistance { get; set; }
        public int travelDuration { get; set; }
    }

    public class Point2
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Address2
    {
        public string adminDistrict { get; set; }
        public string adminDistrict2 { get; set; }
        public string countryRegion { get; set; }
        public string formattedAddress { get; set; }
        public string locality { get; set; }
    }

    public class GeocodePoint2
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
        public string calculationMethod { get; set; }
        public List<string> usageTypes { get; set; }
    }

    public class StartLocation
    {
        public List<double> bbox { get; set; }
        public string name { get; set; }
        public Point2 point { get; set; }
        public Address2 address { get; set; }
        public string confidence { get; set; }
        public string entityType { get; set; }
        public List<GeocodePoint2> geocodePoints { get; set; }
        public List<string> matchCodes { get; set; }
    }

    public class RouteLeg
    {
        public ActualEnd actualEnd { get; set; }
        public ActualStart actualStart { get; set; }
        public List<object> alternateVias { get; set; }
        public int cost { get; set; }
        public string description { get; set; }
        public EndLocation endLocation { get; set; }
        public List<ItineraryItem> itineraryItems { get; set; }
        public string routeRegion { get; set; }
        public List<RouteSubLeg> routeSubLegs { get; set; }
        public StartLocation startLocation { get; set; }
        public double travelDistance { get; set; }
        public int travelDuration { get; set; }
    }

    public class Resource
    {
        public string __type { get; set; }
        public List<double> bbox { get; set; }
        public string id { get; set; }
        public string distanceUnit { get; set; }
        public string durationUnit { get; set; }
        public List<RouteLeg> routeLegs { get; set; }
        public string trafficCongestion { get; set; }
        public string trafficDataUsed { get; set; }
        public double travelDistance { get; set; }
        public int travelDuration { get; set; }
        public int travelDurationTraffic { get; set; }
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

}
