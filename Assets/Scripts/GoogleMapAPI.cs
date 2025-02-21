using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMapAPI : MonoBehaviour
{
    [SerializeField]
    private string m_key;

    public enum mapType
    {
        roadmap,
        satellite,
        hybrid,
        terrain
    }


    public bool Maps(RawImage _rawImage, float _lat, float _lon, int _zoom, int _scale, mapType _mapType)
    {
        try
        {
            int mapWidth = (int)_rawImage.GetComponent<RectTransform>().rect.width;
            int mapHeight = (int)_rawImage.GetComponent<RectTransform>().rect.height;

            string url =
            "https://maps.googleapis.com/maps/api/staticmap?" +
            "center=" + _lat + "," + _lon +
            "&zoom=" + _zoom +
            "&size=" + mapWidth + "x" + mapHeight +
            "&scale=" + _scale +
            "&maptype=" + _mapType +
            "&markers=color:blue%7Clabel:S%7C" + _lat + "," + _lon +
            "&key=" + m_key;

            Debug.Log("url : " + url);

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

            if (done == false)
            {
                return false;
            }
            if (_rawImage != null)
            {
                _rawImage.texture = www.texture;
                //_rawImage.SetNativeSize();
            }

        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return false;
        }
        return true;
    }


}