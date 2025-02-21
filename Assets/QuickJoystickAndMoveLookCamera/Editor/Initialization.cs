using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Initialization : MonoBehaviour
{

    [MenuItem("Tools/QuickJoystickAndMoveLookCamera")]
    public static void QuickThirdPersonController()
    {
        GameObject c = new GameObject("QJAMLC_Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))
        {
            layer = LayerMask.NameToLayer("UI")
        };
        c.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject mla = AssetDatabase.LoadAssetAtPath("Assets/QuickJoystickAndMoveLookCamera/Prefabs/moveLookArea.prefab", typeof(GameObject)) as GameObject;
        GameObject imla = Instantiate(mla, c.transform);
        imla.name = mla.name;

        GameObject ja = AssetDatabase.LoadAssetAtPath("Assets/QuickJoystickAndMoveLookCamera/Prefabs/joystickArea.prefab", typeof(GameObject)) as GameObject;
        GameObject ija = Instantiate(ja, c.transform);
        ija.GetComponent<PlayerController>().cameraController = imla.GetComponent<CameraController>();
        ija.name = ja.name;

        if (!FindObjectOfType(typeof(EventSystem)))
        {
            GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem.SetActive(true);
        }
    }

}