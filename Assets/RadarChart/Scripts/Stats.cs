using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats {

    public event EventHandler OnStatsChanged;

    public static int STAT_MIN = 0;
    public static int STAT_MAX = 99;

    public enum Type {
        Power,
        Speed,
        Stamina,
        Lux,
        Guts,
        Sense,
    }

    private SingleStat powerStat;
    private SingleStat speedStat;
    private SingleStat staminaStat;
    private SingleStat LuxStat;
    private SingleStat gutsStat;
    private SingleStat senseStat;


    public Stats(int powerStatAmount, int speedStatAmount, int staminaStatAmount, int senseStatAmount, int gutsStatAmount, int luxStatAmount) {
        powerStat = new SingleStat(powerStatAmount);
        speedStat = new SingleStat(speedStatAmount);
        staminaStat = new SingleStat(staminaStatAmount);
        LuxStat = new SingleStat(luxStatAmount);
        gutsStat = new SingleStat(gutsStatAmount);
        senseStat = new SingleStat(senseStatAmount);

    }


    private SingleStat GetSingleStat(Type statType) {
        switch (statType)
        {
            default:
            case Type.Power: return powerStat;
            case Type.Speed: return speedStat;
            case Type.Stamina: return staminaStat;
            case Type.Lux: return LuxStat;
            case Type.Guts: return gutsStat;
            case Type.Sense: return senseStat;

        }
    }
    
    public void SetStatAmount(Type statType, int statAmount) {
        GetSingleStat(statType).SetStatAmount(statAmount);
        if (OnStatsChanged != null) OnStatsChanged(this, EventArgs.Empty);
    }

    public void IncreaseStatAmount(Type statType) {
        SetStatAmount(statType, GetStatAmount(statType) + 1);
    }

    public void DecreaseStatAmount(Type statType) {
        SetStatAmount(statType, GetStatAmount(statType) - 1);
    }

    public int GetStatAmount(Type statType) {
        return GetSingleStat(statType).GetStatAmount();
    }

    public float GetStatAmountNormalized(Type statType) {
        return GetSingleStat(statType).GetStatAmountNormalized();
    }



    /*
     * Represents a Single Stat of any Type
     * */
    private class SingleStat {

        private int stat;

        public SingleStat(int statAmount) {
            SetStatAmount(statAmount);
        }

        public void SetStatAmount(int statAmount) {
            stat = Mathf.Clamp(statAmount, STAT_MIN, STAT_MAX);
        }

        public int GetStatAmount() {
            return stat;
        }

        public float GetStatAmountNormalized() {
            return (float)stat / STAT_MAX;
        }
    }
}
