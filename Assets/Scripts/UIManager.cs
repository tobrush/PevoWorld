using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GoogleMapAPI m_GoogleMapAPI;

    [SerializeField]
    GoogleGeocodingAPI m_GeocodingAPI;


    [SerializeField]
    private RawImage View_rawImage;

    public TMPro.TMP_Text placeName;
    public TMPro.TMP_Text address;

    [SerializeField]
    private RawImage m_rawImage;
    [SerializeField]
    private InputField m_let;
    [SerializeField]
    private InputField m_lon;
    [SerializeField]
    private int m_maxZoom; //zoom을 최대 몇까지 할 건지

    public int m_zoom; //현재 zoom 값

    public RectTransform ContentsRect;

    private void Start()
    {
        m_zoom = 16; //기본 16으로 세팅
    }

    //줌 버튼 만들어서 이벤트 등록
    public void ZoomButton(bool _plusMinus)
    {
        ContentsRect.anchoredPosition = new Vector2(-180f,156f);

        if (_plusMinus)
        {
            m_zoom++;
            if (m_zoom > m_maxZoom)
            {
                m_zoom = m_maxZoom;
            }
            else
            {
                //OkButtonEvent(m_zoom);
                OkViewEvent(m_zoom, m_GeocodingAPI.Lat, m_GeocodingAPI.Lng);
            }
        }
        else
        {
            m_zoom--;
            if (m_zoom < 0)
            {
                m_zoom = 0;
            }
            else
            {
                //OkButtonEvent(m_zoom);
                OkViewEvent(m_zoom, m_GeocodingAPI.Lat, m_GeocodingAPI.Lng);
            }
        }
    }

    public void OkButtonEvent(int _zoom)
    {
        RawImage rawImage = m_rawImage;
        float let = float.Parse(m_let.text);
        float lon = float.Parse(m_lon.text);
        int scale = 1;
        
        m_GoogleMapAPI.Maps(rawImage, let, lon, _zoom, scale, GoogleMapAPI.mapType.roadmap);
    }

    public void OkViewEvent(int _zoom, string _let, string _lon)
    {
        RawImage rawImage = m_rawImage;
        float let = float.Parse(_let);
        float lon = float.Parse(_lon);
        int scale = 1;
        Debug.Log(_zoom);
        m_GoogleMapAPI.Maps(rawImage, let, lon, _zoom, scale, GoogleMapAPI.mapType.roadmap);
    }


    public void ViewMap(int _zoom, string _let, string _lon, string PlaceName, string Address)
    {
        RawImage rawImage = View_rawImage;
        float let = float.Parse(_let);
        float lon = float.Parse(_lon);
        int scale = 1;
        m_GoogleMapAPI.Maps(rawImage, let, lon, _zoom, scale, GoogleMapAPI.mapType.roadmap);

        placeName.text = PlaceName;
        address.text = Address;
        View_rawImage.transform.parent.gameObject.SetActive(true);

       
    }

}