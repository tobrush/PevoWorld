using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
public class AddressComponent
{
    public string long_name { get; set; }
    public string short_name { get; set; }
    public List<string> types { get; set; }
}

public class Location
{
    public double lat { get; set; }
    public double lng { get; set; }
}

public class Northeast
{
    public double lat { get; set; }
    public double lng { get; set; }
}

public class Southwest
{
    public double lat { get; set; }
    public double lng { get; set; }
}

public class Viewport
{
    public Northeast northeast { get; set; }
    public Southwest southwest { get; set; }
}

public class Geometry
{
    public Location location { get; set; }
    public string location_type { get; set; }
    public Viewport viewport { get; set; }
}

public class Result
{
    public List<AddressComponent> address_components { get; set; }
    public string formatted_address { get; set; }
    public Geometry geometry { get; set; }
    public string place_id { get; set; }
    public List<string> types { get; set; }
}

public class RootObject
{
    public List<Result> results { get; set; }
    public string status { get; set; }
}
public class GoogleGeocodingAPI : MonoBehaviour
{
    public Toggle OriginToggle;
    public GameObject MapImage;

    public UIManager uiManager;

    public TMP_InputField search;

    public string apiKey;

    public TMP_Text answer;

    public string Lat;
    public string Lng;

    public GameObject SearchResult;

    public void CancleMyPlace()
    {
        search.text = "";
        OriginToggle.isOn = true;
        MapImage.SetActive(false);
    }



    public void PlaceSerch()
    {
        SearchResult.SetActive(true);

        try
        {
            //string url = "https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + search.text + "&key=" + apiKey; // ????????
            string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + search.text + "&key=" + apiKey + "&language=ko"; // ????????
            //Debug.Log("url : " + url);

            WWW www = new WWW(url);

            int delay = 1000;
            int timer = 0;
            bool done = false;

            while (delay > timer)
            {
                System.Threading.Thread.Sleep(1);
                timer++;
                if (www.isDone)
                {
                    done = true;
                    break;
                }
            }

            if (answer != null)
            {

                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(www.text);
                
               
                foreach (var resultObject in rootObject.results)
                {
                    var geometry = resultObject.geometry;
                    var location = geometry.location;
                    var lat = location.lat;
                    var lng = location.lng;
                    Lat = lat.ToString();
                    Lng = lng.ToString();
                 
                    var address = resultObject.formatted_address;
                    //answer.text = www.text;
                    answer.text = "주소 : " + address;
                }


                uiManager.OkViewEvent(16, Lat, Lng);

                MapImage.SetActive(true);
            }

        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
}

