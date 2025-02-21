using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class UI_TestStatsRadarChart : MonoBehaviour {

    [SerializeField] private Material radarMaterial;
    private Color baseMaterialColor;
    private Stats stats;

    public void SetStats(Stats stats) {
        this.stats = stats;

        transform.Find("power").Find("incBtn").GetComponent<Button_UI>().ClickFunc = () => stats.IncreaseStatAmount(Stats.Type.Power);
        transform.Find("power").Find("decBtn").GetComponent<Button_UI>().ClickFunc = () => stats.DecreaseStatAmount(Stats.Type.Power);

        transform.Find("speed").Find("incBtn").GetComponent<Button_UI>().ClickFunc = () => stats.IncreaseStatAmount(Stats.Type.Speed);
        transform.Find("speed").Find("decBtn").GetComponent<Button_UI>().ClickFunc = () => stats.DecreaseStatAmount(Stats.Type.Speed);

        transform.Find("stamina").Find("incBtn").GetComponent<Button_UI>().ClickFunc = () => stats.IncreaseStatAmount(Stats.Type.Stamina);
        transform.Find("stamina").Find("decBtn").GetComponent<Button_UI>().ClickFunc = () => stats.DecreaseStatAmount(Stats.Type.Stamina);

        transform.Find("lux").Find("incBtn").GetComponent<Button_UI>().ClickFunc = () => stats.IncreaseStatAmount(Stats.Type.Lux);
        transform.Find("lux").Find("decBtn").GetComponent<Button_UI>().ClickFunc = () => stats.DecreaseStatAmount(Stats.Type.Lux);

        transform.Find("guts").Find("incBtn").GetComponent<Button_UI>().ClickFunc = () => stats.IncreaseStatAmount(Stats.Type.Guts);
        transform.Find("guts").Find("decBtn").GetComponent<Button_UI>().ClickFunc = () => stats.DecreaseStatAmount(Stats.Type.Guts);

        transform.Find("sense").Find("incBtn").GetComponent<Button_UI>().ClickFunc = () => stats.IncreaseStatAmount(Stats.Type.Sense);
        transform.Find("sense").Find("decBtn").GetComponent<Button_UI>().ClickFunc = () => stats.DecreaseStatAmount(Stats.Type.Sense);

        // Randomize all Stats
        transform.Find("randomBtn").GetComponent<Button_UI>().ClickFunc = () => {
            stats.SetStatAmount(Stats.Type.Power, Random.Range(Stats.STAT_MIN, Stats.STAT_MAX));
            stats.SetStatAmount(Stats.Type.Speed, Random.Range(Stats.STAT_MIN, Stats.STAT_MAX));
            stats.SetStatAmount(Stats.Type.Stamina, Random.Range(Stats.STAT_MIN, Stats.STAT_MAX));
            stats.SetStatAmount(Stats.Type.Lux, Random.Range(Stats.STAT_MIN, Stats.STAT_MAX));
            stats.SetStatAmount(Stats.Type.Guts, Random.Range(Stats.STAT_MIN, Stats.STAT_MAX));
            stats.SetStatAmount(Stats.Type.Sense, Random.Range(Stats.STAT_MIN, Stats.STAT_MAX));

        };

        // Animate Stats
        bool anim = false;
        FunctionPeriodic.Create(() => {
            if (anim) {
                if (Random.Range(0, 100) < 50) stats.IncreaseStatAmount(Stats.Type.Power); else stats.DecreaseStatAmount(Stats.Type.Power);
                if (Random.Range(0, 100) < 50) stats.IncreaseStatAmount(Stats.Type.Speed); else stats.DecreaseStatAmount(Stats.Type.Speed);
                if (Random.Range(0, 100) < 50) stats.IncreaseStatAmount(Stats.Type.Stamina); else stats.DecreaseStatAmount(Stats.Type.Stamina);
                if (Random.Range(0, 100) < 50) stats.IncreaseStatAmount(Stats.Type.Lux); else stats.DecreaseStatAmount(Stats.Type.Lux);
                if (Random.Range(0, 100) < 50) stats.IncreaseStatAmount(Stats.Type.Guts); else stats.DecreaseStatAmount(Stats.Type.Guts);
                if (Random.Range(0, 100) < 50) stats.IncreaseStatAmount(Stats.Type.Sense); else stats.DecreaseStatAmount(Stats.Type.Sense);

            }
        }, .1f);

        transform.Find("animBtn").GetComponent<Button_UI>().ClickFunc = () => {
            anim = !anim;
            transform.Find("animBtn").Find("text").GetComponent<Text>().text = "ANIM: " + anim.ToString().ToUpper();
        };

        // Flashing Color
        //baseMaterialColor = radarMaterial.color;
        //baseMaterialColor.g = .8f;
        Color color = radarMaterial.color;
        bool increase = true;
        FunctionUpdater.Create(() => {
            color.g += .3f * Time.deltaTime * ((increase) ? 1f : -1f);
            radarMaterial.color = color;

            if (color.g >= 1f) {
                increase = false;
                //color.g = baseMaterialColor.g;
            }

            if (color.g <= .9f) {
                increase = true;
            }
        });

        stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsText();
    }

    private void Stats_OnStatsChanged(object sender, System.EventArgs e) {
        UpdateStatsText();
    }

    private void UpdateStatsText()
    {
        transform.Find("text").GetComponent<Text>().text =
            "POWER: " + stats.GetStatAmount(Stats.Type.Power) + "\n" +
            "SPEED: " + stats.GetStatAmount(Stats.Type.Speed) + "\n" +
            "STAMINA: " + stats.GetStatAmount(Stats.Type.Stamina) + "\n" +
            "SENSE: " + stats.GetStatAmount(Stats.Type.Lux) + "\n" +
            "GUTS: " + stats.GetStatAmount(Stats.Type.Guts) + "\n" +
            "LUX: " + stats.GetStatAmount(Stats.Type.Sense);
    }

}
