using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatsRadarChart : MonoBehaviour {

    [SerializeField] private Material radarMaterial;
    [SerializeField] private Texture2D radarTexture2D;

    [SerializeField] private Material radarMaterial2;
    [SerializeField] private Texture2D radarTexture2D2;

    private Stats stats;
    private CanvasRenderer radarMeshCanvasRenderer;

    private Stats stats2;
    private CanvasRenderer radarMeshCanvasRenderer2;

    private void Awake() {
        radarMeshCanvasRenderer = transform.Find("radarMesh1").GetComponent<CanvasRenderer>();
        radarMeshCanvasRenderer2 = transform.Find("radarMesh2").GetComponent<CanvasRenderer>();
    }

    public void SetStats(Stats stats) {
        this.stats = stats;
        stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsVisual();
    }

    private void Stats_OnStatsChanged(object sender, System.EventArgs e) {
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual() {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[7];
        Vector2[] uv = new Vector2[7];
        int[] triangles = new int[3 * 6];

        float angleIncrement = 360f / 6f ;
        float radarChartSize = 145f;

        Vector3 powerVertex = Quaternion.Euler(0, 0, -angleIncrement * 0 +30) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Power);
        int powerVertexIndex = 1;
        Vector3 speedVertex = Quaternion.Euler(0, 0, -angleIncrement * 1 + 30) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Speed);
        int speedVertexIndex = 2;
        Vector3 staminaVertex = Quaternion.Euler(0, 0, -angleIncrement * 2 + 30) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Stamina);
        int staminaVertexIndex = 3;
        Vector3 luxVertex = Quaternion.Euler(0, 0, -angleIncrement * 3 + 30) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Sense);
        int luxVertexIndex = 4;
        Vector3 gutsVertex = Quaternion.Euler(0, 0, -angleIncrement * 4 + 30) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Guts);
        int gutsVertexIndex = 5;
        Vector3 senseVertex = Quaternion.Euler(0, 0, -angleIncrement * 5 + 30) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Lux);
        int senseVertexIndex = 6;


        vertices[0] = Vector3.zero;
        vertices[powerVertexIndex]  = powerVertex;
        vertices[speedVertexIndex] = speedVertex;
        vertices[staminaVertexIndex]   = staminaVertex;
        vertices[luxVertexIndex] = luxVertex;
        vertices[gutsVertexIndex]  = gutsVertex;
        vertices[senseVertexIndex] = senseVertex;


        uv[0]                   = Vector2.zero;
        uv[powerVertexIndex]   = Vector2.one;
        uv[speedVertexIndex]  = Vector2.one;
        uv[staminaVertexIndex]    = Vector2.one;
        uv[luxVertexIndex] = Vector2.one;
        uv[gutsVertexIndex]   = Vector2.one;
        uv[senseVertexIndex] = Vector2.one;

        triangles[0] = 0;
        triangles[1] = powerVertexIndex;
        triangles[2] = speedVertexIndex;

        triangles[3] = 0;
        triangles[4] = speedVertexIndex;
        triangles[5] = staminaVertexIndex;

        triangles[6] = 0;
        triangles[7] = staminaVertexIndex;
        triangles[8] = luxVertexIndex;

        triangles[9]  = 0;
        triangles[10] = luxVertexIndex;
        triangles[11] = gutsVertexIndex;

        triangles[12] = 0;
        triangles[13] = gutsVertexIndex;
        triangles[14] = senseVertexIndex;  

        triangles[15] = 0;
        triangles[16] = senseVertexIndex;
        triangles[17] = powerVertexIndex;


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(radarMaterial, radarTexture2D);
    }

}
